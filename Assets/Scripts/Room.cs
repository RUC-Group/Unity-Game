using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour{
    GameObject[,] roomTiles = new GameObject[5,5];

    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject doorTile;

    public int posX;
    public int posZ;
    public int roomSize;

    Vector2 globalPosition;
    
    public void setRoom(int posX, int posZ, int roomSize){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;

        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
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

    public int indexToUnitePos(int i){
        i = (5*5+15)*i;
        return i;
    }

    public async void showRoom(){
        for(int i = -1; i < roomSize+1; i++){
            for(int j = -1; j< roomSize+1; j++){
                var position = new Vector3(i*5 + indexToUnitePos(posX), 0, j*5 + indexToUnitePos(posZ));

                if(i == -1 && j == -1 || i == -1 && j == roomSize || i == roomSize && j == -1 || i == roomSize && j == roomSize){
                    Instantiate(cornerTile, position, Quaternion.identity);
                }else if(i == -1 || i == roomSize){
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 90));
                }else if ( j== roomSize || j == -1){
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * 0));
                }
                else{
                    Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
    }

    
}
