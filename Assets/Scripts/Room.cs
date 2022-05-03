using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour{
    GameObject[,] roomTiles = new GameObject[7,7];

    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject door;

    public int posX;
    public int posZ;
    public int roomSize = 7;

    Vector2 globalPosition;
    
    public void setRoom(int posX, int posZ, int roomSize){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;

        for (var i = 1; i < roomSize-1; i++){
            for( var j = 1; j< roomSize-1; j++){
                setTile(i,j);
            }
        }

    }
    
    public void setTile(int x, int z){
        roomTiles[x,z] = pickTile();
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

    public void setPosition(Vector2 position){
        globalPosition = position;
    }

    public int indexToUnitPos(int i){
        i = (5*5+15)*i;
        return i;
    }

    public void createDoor(int dir){
        Quaternion rotation;
        Vector3 position;

        if (dir == 2){
            Destroy(roomTiles[3,0]);
            position = new Vector3(3*5 + indexToUnitPos(posX),0,-1*5 + indexToUnitPos(posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else if(dir == 3){
            Destroy(roomTiles[6,3]);
            position = new Vector3(7*5 + indexToUnitPos(posX),0,3*5 + indexToUnitPos(posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        } else if(dir == 0){
            Destroy(roomTiles[3,6]);
            position = new Vector3(3*5 + indexToUnitPos(posX),0,7*5 + indexToUnitPos(posZ));
            rotation = Quaternion.Euler(Vector3.down * 90);
        } else {
            Destroy(roomTiles[0,3]);
            position = new Vector3(-1*5 + indexToUnitPos(posX),0,3*5 + indexToUnitPos(posZ));
            rotation = Quaternion.Euler(Vector3.down * 0);
        }
        Instantiate(door, position, rotation);
    }

    public async void showRoom(){
        for(int i = 0; i < roomSize; i++){
            for(int j = 0; j< roomSize; j++){
                var position = new Vector3(i*5 + indexToUnitPos(posX), 0, j*5 + indexToUnitPos(posZ));

                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    Instantiate(cornerTile, position, Quaternion.identity);
                }else if(i == 0 || i == roomSize -1){
                    roomTiles[i,j] = wallTile;
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                }else if ( j== roomSize - 1 || j == 0){
                    roomTiles[i,j] = wallTile;
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                }
                else{
                    Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
    }

    
}
