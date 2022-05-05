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

    public GameObject endGate;

    public int posX;
    public int posZ;
    public int roomSize = 7;

    public string typeOfRoom = null;

    Vector2 globalPosition;
    
    public void setRoom(int posX, int posZ, int roomSize){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;
        this.typeOfRoom=null;

        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j] = cornerTile;
                }else if(i == 0 || i == roomSize -1 || j== roomSize - 1 || j == 0){
                    roomTiles[i,j] = wallTile;
                }else{
                    roomTiles[i,j] = pickTile();
                }
            }
        }
    }

    public void changeRoom(string typeOfRoom){
        this.typeOfRoom=typeOfRoom;
        if(this.typeOfRoom == "end gate room"){
            for (var i = 1; i < roomSize-1; i++){
                for( var j = 1; j< roomSize-1; j++){
                    if(i == (roomSize-1)/2 && j == (roomSize-1)/2){
                        roomTiles[i,j] = endGate;
                    }else{
                        roomTiles[i,j] = emptyTile;
                    }
                }
            }
        }
    }
    public void setRoom(int posX, int posZ, int roomSize, string typeOfRoom){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;
        this.typeOfRoom=typeOfRoom;
    	
        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j] = cornerTile;
                }else if(i == 0 || i == roomSize -1 || j== roomSize - 1 || j == 0){
                    roomTiles[i,j] = wallTile;
                }else{
                    roomTiles[i,j] = emptyTile;
                }
            }
        }
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
        if (dir == 2){
            roomTiles[3,0] = door;
            
        } else if(dir == 3){
            roomTiles[6,3] = door;
            
        } else if(dir == 0){
            roomTiles[3,6] = door;
            
        } else {
            roomTiles[0,3] = door;
        }
    }

    public async void showRoom(){
        for(int i = 0; i < roomSize; i++){
            for(int j = 0; j< roomSize; j++){
                var position = new Vector3(i*5 + indexToUnitPos(posX), 0, j*5 + indexToUnitPos(posZ));
                

                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }else if(i == 0 || i == roomSize -1){
                    Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 90));
                }else if ( j== roomSize - 1 || j == 0){
                    Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 0));
                }else{
                    Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
    }

    
}
