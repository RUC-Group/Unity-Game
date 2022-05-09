using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour{
    public GameObject room;
    public GameObject player;
    public GameObject playerSword;
    public GameObject hallway;
    int floorSize = 5;
    List<Room> roomsList;
    Room[,] rooms;
    List<Room> spawnedRooms;

    public Floor(int floorSize){
        this.floorSize = floorSize;
    }

    void Start(){
        floorAlgorithm2();
        //print("created " + count + " rooms.");
        showFloor();
    }

    // Update is called once per frame
    void Update(){
    }

    public void showFloor(){
        int count = 0;
        foreach(Room room in spawnedRooms){
            room.showRoom();
            count ++;
        }
        print("amount of rooms spawned: " + count);
    }

    public void createHallway(int dir, Room r){
        Quaternion rotation;
        Vector3 position;

        if (dir == 2){
            position = new Vector3(3*5 + r.indexToUnitPos(r.posX),0,-1*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else if(dir == 3){
            position = new Vector3(7*5 + r.indexToUnitPos(r.posX),0,3*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        } else if(dir == 0){
            position = new Vector3(3*5 + r.indexToUnitPos(r.posX),0,7*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else {
            position = new Vector3(-1*5 + r.indexToUnitPos(r.posX),0,3*5 + r.indexToUnitPos(r.posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        }
        Instantiate(hallway, position, rotation);
    }

    public void floorAlgorithm2(){
        int floorSize = 1000;
        rooms = new Room[floorSize,floorSize];
        spawnedRooms = new List<Room>();
        Room pickedRoom;
        int numberOfRooms = Random.Range(10,15);
        print("number of rooms to spawn: " + numberOfRooms);
        int indexX = (int)floorSize/2;
        int indexZ = (int)floorSize/2;
        
        rooms[indexX,indexZ] = makeRoom(indexX,indexZ,"spawn room");
        pickedRoom = rooms[indexX,indexZ];
        Instantiate(player, new Vector3((5*5+15)*indexX + 12, 10,(5*5+15)*indexZ + 12),Quaternion.Euler(Vector3.down * 0));
        spawnedRooms.Add(pickedRoom);
        while (numberOfRooms > 0){
            int dir = Random.Range(0,4);
            int spawnCount = Random.Range(1,5);

            switch (dir){
                case 0:
                    for (var i = 0; i < spawnCount + 1; i++){
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
                case 3:
                    for (var i = 0; i < spawnCount + 1; i++){
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
                default:
                    print("Error!");
                    break;
            }
            pickedRoom = pickRoom(spawnedRooms);
        }
        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom(spawnedRooms);
        }
        pickedRoom.changeRoom("end gate room");
        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom(spawnedRooms);
        }
        pickedRoom.changeRoom("key room");

        while(pickedRoom.typeOfRoom!=null){
            pickedRoom = pickRoom(spawnedRooms);
        }
        pickedRoom.changeRoom("key room");
    }


    public Room pickRoom(List<Room> a){
        int num = Random.Range(0,a.Count);
        return a[num];
    }

    public Room makeRoom(int roomX, int roomZ){
        int roomSize = 7;
        var position = new Vector3(roomX, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize);

        return newRoom;
    }

    public Room makeRoom(int roomX, int roomZ, string typeOfRoom){
        int roomSize = 7;
        var position = new Vector3(roomX, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize,typeOfRoom);

        return newRoom;
    }
}
