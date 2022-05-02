using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour{
    List<Room> rooms;
    //Room[] rooms;

    public GameObject room;

    int floorSize=5;
    int tileSize = 5;

    public Floor(int floorSize){
        this.floorSize = floorSize;
    }

    void Start(){
        rooms=new List<Room>();
        int count = 0;
        int roomX = 0;
        int roomZ = 0;
        floorSize=5;
        count++;
        rooms.Add(makeRoom(roomX, roomZ));
        for (int i = 0; i < floorSize-1; i++){
            count++;
            Room lastRoom = rooms[i];
            roomZ = lastRoom.posZ + lastRoom.roomSize * 5 + 15;
            rooms.Add(makeRoom(roomX,roomZ));
        }
        //print("created " + count + " rooms.");
        showFloor();
    }

    // Update is called once per frame
    void Update(){
    }

    /*public void setRoom(int index, Room inputRoom){
        rooms[index] = inputRoom;
    }*/

    public void showFloor(){
        foreach(Room room in rooms){
            var position = new Vector3(0, 0, room.posZ);
            Instantiate(room, position, Quaternion.identity);
            room.showRoom();
        }
    }

    public Room makeRoom(int roomX, int roomZ){
        int roomSize = 5;
        var position = new Vector3(0, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);;
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize);
        int count=0;

        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                count++;
                newRoom.setTile(i,j);
            }
        }
        // print("room has " + count + " tiles.");
        return newRoom;
    }
}
