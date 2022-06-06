using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Floor : MonoBehaviour{
    public GameObject room;
    public GameObject player;
    public GameObject hallway;

    int floorSize = 31;

    int minRoomsAmount = 5;
    int roomSize;

    int maxRoomsAmount = 10;
    Room[,] rooms;
    List<Room> spawnedRooms = new List<Room>();
    List<Transform> waypoints = new List<Transform>();


    void Start(){
        floorAlgorithm();
        showFloor();
        waypoints = getAllWaypoints(getTiles());
    }

    //Instantiate every gameObject inside of room
    public void showFloor(){
        int count = 0;
        foreach(Room room in spawnedRooms){
            room.showRoom();
            count ++;
        }
    }


    //create a random layout of rooms
    public void floorAlgorithm(){
        rooms = new Room[floorSize,floorSize];
        int numberOfRooms = UnityEngine.Random.Range(minRoomsAmount,maxRoomsAmount);
        int indexX = maxRoomsAmount-1/2;
        int indexZ = maxRoomsAmount-1/2;
        
        rooms[indexX,indexZ] = makeRoom(indexX,indexZ,"spawn room");
        Room pickedRoom = rooms[indexX,indexZ];
        Instantiate(player, new Vector3(((roomSize-2)*5+15)*indexX + 19, 1,((roomSize-2)*5+15)*indexZ + 15),Quaternion.Euler(Vector3.down * 0));
        spawnedRooms.Add(pickedRoom);
        while (numberOfRooms > 0){
            int dir = UnityEngine.Random.Range(0,4);
            int spawnCount = UnityEngine.Random.Range(1,5);
            
            switch (dir){
                case 0:
                    for (var i = 0; i < spawnCount + 1; i++){
                        if(numberOfRooms<=0)    break;
                        if (spawnCount != i && rooms[pickedRoom.posX,pickedRoom.posZ-i]==null){
                            Room r = makeRoom(pickedRoom.posX,pickedRoom.posZ-i);
                            Room pr = rooms[pickedRoom.posX,pickedRoom.posZ-i+1];
                            pr.createDoor(2);
                            r.createDoor(dir);
                            createHallway(dir, r);
                            rooms[pickedRoom.posX,pickedRoom.posZ-i] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    }
                    break;
                case 1:
                    for (var i = 0; i < spawnCount + 1; i++){
                        if(numberOfRooms<=0)    break;
                        if (spawnCount != i && rooms[pickedRoom.posX+i,pickedRoom.posZ]==null){
                            Room r = makeRoom(pickedRoom.posX+i,pickedRoom.posZ);
                            Room pr = rooms[pickedRoom.posX + i -1,pickedRoom.posZ];
                            pr.createDoor(3);
                            r.createDoor(dir);
                            createHallway(dir, r);
                            rooms[pickedRoom.posX+i,pickedRoom.posZ] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    }
                    break;
                case 2:
                    for (var i = 0; i < spawnCount + 1; i++){
                        if(numberOfRooms<=0)    break;
                        if (spawnCount != i && rooms[pickedRoom.posX,pickedRoom.posZ+ i]==null){
                            Room r = makeRoom(pickedRoom.posX,pickedRoom.posZ+i);
                            Room pr = rooms[pickedRoom.posX,pickedRoom.posZ+i-1];
                            pr.createDoor(0);
                            r.createDoor(dir);
                            createHallway(dir, r);
                            rooms[pickedRoom.posX,pickedRoom.posZ+i] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    }
                    break;
                default:
                    for (var i = 0; i < spawnCount + 1; i++){
                        if(numberOfRooms<=0)    break;
                        if (spawnCount != i && rooms[pickedRoom.posX-i,pickedRoom.posZ]==null){
                            Room r = makeRoom(pickedRoom.posX-i,pickedRoom.posZ);
                            Room pr = rooms[pickedRoom.posX-i+1,pickedRoom.posZ];
                            pr.createDoor(1);
                            r.createDoor(dir);
                            createHallway(dir, r);
                            rooms[pickedRoom.posX-i,pickedRoom.posZ] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    }
                    break;
            }
            pickedRoom = pickRoom();
        }

        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom();
        }
        pickedRoom.changeRoom("end gate room");

        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom();
        }
        pickedRoom.changeRoom("key room");

        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom();
        }
        pickedRoom.changeRoom("key room");
    }

    //method that makes a room at the coordinate (roomX,0,roomZ)
    public Room makeRoom(int roomX, int roomZ, string typeOfRoom=null){
        roomSize = 7;
        var position = new Vector3(roomX, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();

        newRoom.setRoom(roomX,roomZ,roomSize,typeOfRoom);

        return newRoom;
    }

    //methode that randomly picks a already spawned room
    public Room pickRoom(){
        return spawnedRooms[UnityEngine.Random.Range(0,spawnedRooms.Count)];
    }

    //creates a hallwayTile 
    public void createHallway(int dir, Room r){
        Quaternion rotation;
        Vector3 position;
        
        if (dir == 2){
            position = new Vector3(((int)roomSize/2)*5 + r.indexToUnitPos(r.posX),0,-1*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else if(dir == 3){
            position = new Vector3((roomSize)*5 + r.indexToUnitPos(r.posX),0,(((int)roomSize/2))*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        } else if(dir == 0){
            position = new Vector3(((int)roomSize/2)*5 + r.indexToUnitPos(r.posX),0,(roomSize)*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else {
            position = new Vector3(-1*5 + r.indexToUnitPos(r.posX),0,(( (int)roomSize/2))*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        }

        Instantiate(hallway, position, rotation);
    }

    //returns the list of spawned rooms
    public List<Room> getSpawnedRooms(){
        return spawnedRooms;
    }

    //method that returns a list of the Transform component of all tiles in the game
    public List<Transform> getTiles(){
        List<Transform> allTiles = new List<Transform>();

        foreach (Room r in spawnedRooms){
            GameObject[,] roomTiles = r.getRoomTiles();
            for (var i = 0; i < roomTiles.Length/roomSize; i++){
                for (var j = 0; j < roomTiles.Length/roomSize; j++){
                    allTiles.Add(roomTiles[i,j].transform);
                }
            }
        }
        return allTiles;
    }

    // Method to return all the waypoints from a list of tiles
    List<Transform> getAllWaypoints(List<Transform> tiles){ 

        foreach (Transform tile in tiles){
            for (var i = 0; i < tile.childCount; i++){
                if (tile.GetChild(i).gameObject.tag == "pathHolder"){
                    for (var j = 0; j < tile.transform.GetChild(i).childCount; j++){
                        if(tile.GetChild(i).transform.GetChild(j).gameObject.tag == "PathFindingWAypoint"){
                            waypoints.Add(tile.GetChild(i).transform.GetChild(j).transform);
                        }
                    }
                }
            }
        }

        return waypoints;
    }
}
