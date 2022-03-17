using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour{
    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject doorTile;

    void Start() {
        makeFloor();
    }

    void makeFloor(){
        int floorSize = 10;
        int roomX = 0;
        int roomY = 0;
        Floor firstFloor = new Floor();
        for (int i = 0; i < floorSize; i++){
            makeRoom(roomX,roomY);
            //firstFloor.setRoom(i, makeRoom(roomX,roomY));
            //roomX = trueRoomSize * tileSize + roomX;
            roomY = trueRoomSize * tileSize + roomY;
        }
    }
    

    
    int trueRoomSize = 3 + 2;
    int tileSize = 5;
    Room makeRoom(int spawnX, int spawnZ){
        Room newRoom = new Room();
        int localX = 0;
        int doorSpawnPos = 3;
        for (var x = spawnX; x < trueRoomSize + spawnX; x++){
            int localZ = 0;
            for (var z = spawnZ; z < trueRoomSize + spawnZ; z++){
                if (x == spawnX && z == spawnZ){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 270));
                    //newRoom.setTile(localX, localZ, cornerTile);
                } 
                else if (x == spawnX + doorSpawnPos && z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(doorTile, position, Quaternion.Euler(Vector3.down * 180));
                    //newRoom.setTile(localX, localZ, doorTile);
                }
                else if (x == spawnX + trueRoomSize - 1 && z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                    //newRoom.setTile(localX, localZ, cornerTile);
                } 
                else if (x == spawnX +  trueRoomSize - 1 && z == spawnZ + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 0));
                    //newRoom.setTile(localX, localZ, cornerTile);
                } 
                else if (z == spawnZ + trueRoomSize - 1 && x == spawnX + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 180));
                    //newRoom.setTile(localX, localZ, cornerTile);
                } 
                else if (x == spawnX + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                    //newRoom.setTile(localX, localZ, wallTile);
                } 
                else if (z == spawnZ + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                    //newRoom.setTile(localX, localZ, wallTile);
                } 
                else if (z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 180));
                    //newRoom.setTile(localX, localZ, wallTile);
                } 
                else if (x == spawnX + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                    //newRoom.setTile(localX, localZ, wallTile);
                } 
                else{
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    GameObject randomTile = pickTile();
                    Instantiate(randomTile, position, Quaternion.identity);
                    //newRoom.setTile(localX, localZ, randomTile);
                }
                localZ++;
            }
            localX++;
        }
        return newRoom;
    }

    GameObject pickTile(){
        int randNum = Random.Range(0,4);
        switch (randNum){
            case 0:
                return emptyTile;
            case 1:
                return spikeTile;
            case 2:
                return enemyTile;
            case 3:
                return treasureTile;
            default:
                return null;
        }
    }

}
