using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject doorTile;
    public int roomSize = 3;
    public bool passed = false;
    
    // Start is called before the first frame update
    void Start(){
    }

    GameObject pickTile(){
        int randNum = Random.Range(0,4);
        print(randNum);
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

    public void makeRoom(int spawnX, int spawnY){
        int trueRoomSize = roomSize + 2;
        int tileSize = 5;
        int doorX = 3;
        for (var x = spawnX; x < trueRoomSize + spawnX; x++){
            for (var y = spawnY; y < trueRoomSize + spawnY; y++){
                if (x == spawnX && y == spawnY){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 270));
                } 
                else if (x == spawnX + doorX && y == spawnY + trueRoomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(doorTile, position, Quaternion.Euler(Vector3.down * 180));
                }
                else if (x == spawnX + trueRoomSize - 1 && y == spawnY + trueRoomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (x == spawnX +  trueRoomSize - 1 && y == spawnY + 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 0));
                } 
                else if (y == spawnY + trueRoomSize - 1 && x == spawnX + 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 180));
                } 
                else if (x == spawnX + 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                } 
                else if (y == spawnY + 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                } 
                else if (y == spawnY + trueRoomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 180));
                } 
                else if (x == spawnX + trueRoomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else{
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(pickTile(), position, Quaternion.identity);
                }
                
            }
        }
    }
}
