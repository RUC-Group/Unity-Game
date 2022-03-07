using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public int roomSize = 3;
    
    // Start is called before the first frame update
    void Start(){
        int tileSize = 5;
        for (var x = 0; x < roomSize; x++){
            for (var y = 0; y < roomSize; y++){
                var position = new Vector3(x * tileSize, 0, y * tileSize);
                Instantiate(pickTile(), position, Quaternion.identity);
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
