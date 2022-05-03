using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour{
    public GameObject room;
    public GameObject player;
    public float test = 0;
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

    public void floorAlgorithm2(){
        int floorSize = 1000;
        rooms = new Room[floorSize,floorSize];
        spawnedRooms = new List<Room>();
        Room pickedRoom;
        int numberOfRooms = Random.Range(99,100);
        print("number of rooms to spawn: " + numberOfRooms);
        int indexX = Random.Range(150,151);
        int indexZ = Random.Range(150,151);
        
        rooms[indexX,indexZ] = makeRoom(indexX,indexZ);
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
                            r.createDoor(dir);
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
                            r.createDoor(dir);
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
                            r.createDoor(dir);
                            rooms[pickedRoom.posX,pickedRoom.posZ+i] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    }
                    break;
                default:
                    for (var i = 0; i < spawnCount + 1; i++){
                        if (spawnCount != i && rooms[pickedRoom.posX-i,pickedRoom.posZ]==null){
                            Room r = makeRoom(pickedRoom.posX-i,pickedRoom.posZ);
                            r.createDoor(dir);
                            rooms[pickedRoom.posX-i,pickedRoom.posZ] = r;
                            spawnedRooms.Add(r);
                            numberOfRooms--;
                        }
                    } 
                    break;
            }
            
            pickedRoom = pickRoom(spawnedRooms);
        }
    }


    public Room pickRoom(List<Room> a){
        int num = Random.Range(0,a.Count);
        return a[num];
    }
/*
    public void floorAlgorithm(){
        roomsList = new List<Room>();
        int count = 0;
        int roomX = 0;
        int roomZ = 0;
        floorSize = 5;
        count++;
        rooms.Add(makeRoom(roomX, roomZ));
        for (int i = 0; i < floorSize-1; i++){
            count++;
            Room lastRoom = rooms[i];
            roomZ = lastRoom.posZ + lastRoom.roomSize;
            rooms.Add(makeRoom(roomX,roomZ));
        }
    }

    public void floorAlgorithm3(){
        roomsList = new List<Room>();
        int count = 0;
        int roomX = 0;
        int roomZ = 0;
        floorSize = 5;
        count++;
        rooms.Add(makeRoom(roomX, roomZ));
        for (int i = 0; i < floorSize; i++){
            count++;
            Room lastRoom = rooms[i];
            roomZ = i;
            rooms.Add(makeRoom(roomX,roomZ));
        }
    }
*/
    public Room makeRoom(int roomX, int roomZ){
        int roomSize = 7;
        var position = new Vector3(roomX, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize);

        return newRoom;
    }
}
