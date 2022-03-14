using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Door[] allDoors;
    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject doorTile;
    public int roomSize = 3;

    // Start is called before the first frame update
    void Start(){
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
    public void makeRoom(int spawnX, int spawnZ){
        int trueRoomSize = roomSize + 2;
        int tileSize = 5;
        int localX = -3;
        int doorSpawnPos = 3;
        for (var x = spawnX; x < trueRoomSize + spawnX; x++){
            float localZ = 0;
            for (var z = spawnZ; z < trueRoomSize + spawnZ; z++){
                if (x == spawnX && z == spawnZ){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 270));
                } 
                else if (x == spawnX + doorSpawnPos && z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(doorTile, position, Quaternion.Euler(Vector3.down * 180));
                }
                else if (x == spawnX + trueRoomSize - 1 && z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (x == spawnX +  trueRoomSize - 1 && z == spawnZ + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 0));
                } 
                else if (z == spawnZ + trueRoomSize - 1 && x == spawnX + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 180));
                } 
                else if (x == spawnX + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                } 
                else if (z == spawnZ + 0){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                } 
                else if (z == spawnZ + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 180));
                } 
                else if (x == spawnX + trueRoomSize - 1){
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else{
                    var position = new Vector3(spawnX + localX * tileSize, 0, spawnZ + localZ * tileSize);
                    Instantiate(pickTile(), position, Quaternion.identity);
                }
                localZ++;
            }
            localX++;
        }
    }
}
