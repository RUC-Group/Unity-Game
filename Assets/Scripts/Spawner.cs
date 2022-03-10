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
    public int roomSize = 3;
    
    // Start is called before the first frame update
    void Start(){
        int tileSize = 5;
        for (var x = 0; x < roomSize; x++){
            for (var y = 0; y < roomSize; y++){

                /*
                switch (x)
                {
                    case 0:
                        var position = new Vector3(x * tileSize, 0, y * tileSize);
                        Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                    case roomSize - 1:
                        var position = new Vector3(x * tileSize, 0, y * tileSize);
                        Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                    case 0:
                        var position = new Vector3(x * tileSize, 0, y * tileSize);
                        Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                    case 0:
                        var position = new Vector3(x * tileSize, 0, y * tileSize);
                        Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                                                                            
                    default:
                    rotation = Quaternion.identity;
                }
                */
                if (x == 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 270));
                } 
                else if (y == 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                } 
                else if (y == roomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 180));
                } 
                else if (x == roomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (x == roomSize - 1 && y == roomSize - 1){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (x == roomSize - 1 && y == 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (y == roomSize - 1 && x == 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else if (x == 0 && y == 0){
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(cornerTile, position, Quaternion.Euler(Vector3.down * 90));
                } 
                else{
                    var position = new Vector3(x * tileSize, 0, y * tileSize);
                    Instantiate(pickTile(), position, Quaternion.identity);
                }
                
            }
        }
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
}
