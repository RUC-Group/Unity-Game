using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour{
    public GameObject room;
    public GameObject player;
    public GameObject hallway;
    int floorSize = 5;
    List<Room> roomsList;
    Room[,] rooms;
    List<Room> spawnedRooms;
    List<Transform> waypoints;

    public Floor(int floorSize){
        this.floorSize = floorSize;
    }

    void Start(){
        floorAlgorithm2();
        showFloor();
        waypoints = getAllWaypoints(getTiles());
        print(waypoints.Count + " waypoints in the map");
    }

    // Update is called once per frame
    void Update(){
    }
    public List<Room> getSpawnedRooms(){
        return spawnedRooms;
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
        int numberOfRooms = 3; //Random.Range(99,100);
        print("number of rooms to spawn: " + numberOfRooms);
        int indexX = Random.Range(150,151);
        int indexZ = Random.Range(150,151);
        
        rooms[indexX,indexZ] = makeRoom(indexX,indexZ);
        pickedRoom = rooms[indexX,indexZ];
        Instantiate(player, new Vector3((5*5+15)*indexX + 12, 10,(5*5+15)*indexZ + 12),Quaternion.Euler(Vector3.down * 0));
        spawnedRooms.Add(pickedRoom);
        while (numberOfRooms > 0){
            int dir = Random.Range(0,4);
            int spawnCount = 1;//Random.Range(1,5);

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
    }


    public Room pickRoom(List<Room> a){
        int num = Random.Range(0,a.Count);
        return a[num];
    }

    // Method to return all the waypoints from a list of tiles
    List<Transform> getAllWaypoints(List<GameObject> tiles){
        
        waypoints = new List<Transform>();
        foreach (GameObject tile in tiles){
            
            for (var i = 0; i < tile.transform.childCount; i++){
                
                if (tile.transform.GetChild(i).gameObject.tag == "pathHolder"){
                    
                    for (var j = 0; j < tile.transform.GetChild(i).childCount; j++){

                        if(tile.transform.GetChild(i).transform.GetChild(j).gameObject.tag == "PathFindingWAypoint"){
                            waypoints.Add(tile.transform.GetChild(i).transform.GetChild(j));
                            
                        }
                    }
                }
            }
        }
        return waypoints;
    }

    //method that returns a list of all tiles in the game
    public List<GameObject> getTiles(){
        List<GameObject> allTiles = new List<GameObject>();
        foreach (Room r in spawnedRooms){
            GameObject[,] roomTiles = r.getRoomTiles();
            for (var i = 0; i < roomTiles.Length/7; i++){
                for (var j = 0; j < roomTiles.Length/7; j++){
                    allTiles.Add(roomTiles[i,j]);
                }
            }
        }
        return allTiles;
    }

    void OnDrawGizmos(){
        foreach (Transform waypoint in waypoints){
            Gizmos.DrawSphere(waypoint.position,.3f);
        }   
    }
    public Room makeRoom(int roomX, int roomZ){
        int roomSize = 7;
        var position = new Vector3(roomX, 0, roomZ);
        GameObject newRoomObject = Instantiate(room,position,Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.setRoom(roomX,roomZ,roomSize);

        return newRoom;
    }
}
