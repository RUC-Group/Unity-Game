using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour{
    List<Room> rooms;
    //Room[] rooms;
    Spawner spawner = new Spawner();

    public GameObject room;

    int floorSize=5;
    int tileSize = 5;

    public Floor(int floorSize){
        this.floorSize = floorSize;
    }
    // Start is called before the first frame update
    /*void Start(){
        rooms=new GameObject[floorSize];
        int roomX = 0;
        int roomZ = 0;
        GameObject newRoom = makeRoom(roomX,roomZ);
        rooms[0]=newRoom;
        print("room gen");
         for (int i = 1; i < floorSize; i++){
            print("room gen");
            GameObject lastRoom = rooms[i];
            roomZ+=lastRoom.GetComponent<Room>().posZ+lastRoom.GetComponent<Room>().roomSize;
            newRoom = makeRoom(roomX,roomZ);
            rooms[i]=newRoom;
        }
    }*/

    void Start(){
        rooms=new List<Room>();
        int count=0;
        int roomX = 0;
        int roomZ = 0;
        floorSize=5;
        count++;
        rooms.Add(makeRoom(roomX, roomZ, 0));
        for (int i = 0; i < floorSize-1; i++){
            count++;
            Room lastRoom = rooms[i];
            roomZ = lastRoom.posZ + lastRoom.roomSize * 5 + 15;
            print(roomZ);
            rooms.Add(makeRoom(roomX,roomZ, i + 1));
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
            print(room.nameOfRoom);
        }
    }

    public Room makeRoom(int roomX, int roomZ, int name){
        int roomSize = 5;
        var position = new Vector3(0, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);;
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize, name);
        int count=0;

        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                count++;
                newRoom.setTile(i,j);
            }
        }
        //print("room has "+count+" tiles.");
        return newRoom;
    }
}
