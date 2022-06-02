using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour{

    GameObject[,] roomTiles;

    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject symbolwall;
    public GameObject swordNShieldwall;
    public GameObject gobletWall;
    public GameObject headwall;
    public GameObject cagewall;
    public GameObject bookwall; 
    public GameObject cornerTile;
    public GameObject door;
    public GameObject bonfireTile;
    public GameObject bigEnemy;
    public GameObject hordeEnemy;
    public GameObject chestTile;

    public GameObject keyTile;

    public GameObject endGate;

    public int posX; 
    public int posZ;
    public int roomSize;
    int roomTreasureCount;
    int roomMaxTreasure = 5;

    public string typeOfRoom = null;
    
    

    // Method to return all the waypoints from a list of tiles
    public List<Transform> getWaypointsForRoom(){
        List<Transform> l = new List<Transform>();
        foreach (GameObject tile in roomTiles){
            for (var i = 0; i < tile.transform.childCount; i++){
                if (tile.transform.GetChild(i).gameObject.tag == "pathHolder"){
                    for (var j = 0; j < tile.transform.GetChild(i).childCount; j++){
                        if(tile.transform.GetChild(i).transform.GetChild(j).gameObject.tag == "PathFindingWAypoint"){
                            l.Add(tile.transform.GetChild(i).transform.GetChild(j).transform);
                        }
                    }
                }
            }
        }
        return l;
    }

    public GameObject[,] getRoomTiles(){
        return roomTiles;
    }

    public void changeRoom(string typeOfRoom){
        this.typeOfRoom=typeOfRoom;
        for (var i = 1; i < roomSize-1; i++){
                for( var j = 1; j< roomSize-1; j++){
                    if(i == (roomSize-1)/2 && j == (roomSize-1)/2){
                        if(this.typeOfRoom == "end gate room"){
                            roomTiles[i,j] = endGate;
                        }else if(this.typeOfRoom == "key room"){
                            roomTiles[i,j] = keyTile;
                        }
                    }else{
                        roomTiles[i,j] = emptyTile;
                    }
                }
            }
    }
    public void setRoom(int posX, int posZ, int roomSize, string typeOfRoom=null){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;
        this.typeOfRoom=typeOfRoom;
        this.roomTiles = new GameObject[roomSize,roomSize];
    	
        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j] = cornerTile;
                }else if(i == 0 || i == roomSize -1 || j== roomSize - 1 || j == 0){
                    if(j== roomSize - 1){
                        roomTiles[i,j] = pickWall();
                    }else if(i == roomSize -1){
                        roomTiles[i,j] = pickWall();
                    }else{
                        roomTiles[i,j] = wallTile;
                    }
                }else{
                    if(typeOfRoom!=null){
                        roomTiles[i,j] = emptyTile;
                        if(typeOfRoom == "spawn room"){
                            roomTiles[(int)roomSize/2,(int)roomSize/2] = enemyTile;
                            roomTiles[2,2] = spikeTile;
                            roomTiles[2,4] = spikeTile;
                            roomTiles[4,2] = spikeTile; 
                            roomTiles[4,4] = spikeTile;
                            roomTiles[3,2] = spikeTile;
                            roomTiles[5,2] = spikeTile;
                            roomTiles[3,4] = spikeTile;
                            roomTiles[1,4] = spikeTile;
                        }
                    }else{
                        roomTiles[i,j] = pickTile(i,j);
                    }
                }
            }
        }
    }

    GameObject pickTile(int x, int y){
        //Determination of tileID based off of algo
        int tileID = 0;
        int roomSizeMinusOne = roomSize - 1;
        if (x == roomSizeMinusOne/2 && y == 1 || x == roomSizeMinusOne-1 && y == roomSizeMinusOne/2 || x == roomSizeMinusOne/2 && y == roomSizeMinusOne-1 || x == 1 && y == roomSizeMinusOne/2){ //if there's a door adjacant to this tile... (3,0+1)(6-1,3)(3,6-1)(0+1,3)
            tileID = 0; //...set this tile to be empty
        }else if (x == 1 && y == 1 || x == roomSizeMinusOne-1 && y == 1 || x == 1 && y == roomSizeMinusOne-1 || x == roomSizeMinusOne-1 && y == roomSizeMinusOne-1 || x == roomSizeMinusOne/2 && y == roomSizeMinusOne/2){ //if tile is a corner or at center of room
            int r = UnityEngine.Random.Range(0,5);
            if (r < 3 && roomTreasureCount < roomMaxTreasure){
                tileID = 3; 
                roomTreasureCount ++;
            }else{
                tileID = UnityEngine.Random.Range(0,3);
            }
        } else if(x == 1 || x == 2 || x == roomSizeMinusOne-1 || x == roomSizeMinusOne-2){ //if tile surrounds where a treasure could be on x axis
            if (y == 1 || y == 2 || y == roomSizeMinusOne-1 || y == roomSizeMinusOne-2 && roomTiles[x,y] != treasureTile){ //if tile surrounds where a treasure could be on y axis, and ISN'T treasure
                int r = UnityEngine.Random.Range(0,4);
                if(r <= 1){
                    tileID = 2;
                } else if(r == 2){
                    tileID = 0;
                } else {
                    tileID = 1;
                }                
            }
        } else {
            tileID = UnityEngine.Random.Range(0,2);
        }

        //Set tile based off of tileID
        switch (tileID){
            case 0:
                return emptyTile;
            case 1:
                return spikeTile;
            case 2:
                return pickEnemy();
            case 3:
                return pickTreasure();
            default:
                return null;
        }
    }

    GameObject pickTreasure(){
        int r = UnityEngine.Random.Range(0,3);
        switch (r){
            case 0:
                return chestTile;
            default:
                return treasureTile;
        }
    }

    public GameObject pickEnemy(){
        int r = UnityEngine.Random.Range(0,3);
        //return enemyTile;
        // issue with the horde tile, rn  it just returns normal enemies always
        switch (r){
            case 0:
                return bigEnemy;
            case 1:
                return hordeEnemy;
            default:
                return enemyTile;
        }
    }

    public GameObject pickWall(){
        int r = UnityEngine.Random.Range(0,10);
        switch (r){
            case 0:
                return cagewall;
            case 1:
                return headwall;
            case 2:
                return swordNShieldwall;
            case 3:
                return gobletWall;
            case 4:
                return symbolwall;
            case 5: 
                return bookwall;
            default:
                return wallTile;
        }
    }

    

    public int indexToUnitPos(int i){
        return ((roomSize-2)*5+15)*i;
    }



    public void createDoor(int dir){
        if (dir == 2){
            roomTiles[(int)roomSize/2,0] = door;
            
        } else if(dir == 3){
            roomTiles[roomSize-1,(int)roomSize/2] = door;
            
        } else if(dir == 0){
            roomTiles[(int)roomSize/2,roomSize-1] = door;
            
        } else {
            roomTiles[0,(int)roomSize/2] = door;
        }
    }

    public void showRoom(){        
        for(int i = 0; i < roomSize; i++){
            for(int j = 0; j< roomSize; j++){
                var position = new Vector3(i*5 + indexToUnitPos(posX), 0, j*5 + indexToUnitPos(posZ));
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }else if(i == 0){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 90));
                }else if ( j== roomSize - 1){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 0));
                }else if(i == roomSize -1){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 270));
                }else if (j == 0){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 180));
                }else{
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
        giveEnemyWaypoints();
    }

    void giveEnemyWaypoints(){
        List<Transform> waypoints = getWaypointsForRoom();
        for(int i = 1; i < roomSize-1; i++){
            for(int j = 1; j< roomSize-1; j++){
                if(roomTiles[i,j].tag == "enemyTile"){
                    Enemy[] enemies = roomTiles[i,j].GetComponentsInChildren<Enemy>();
                    foreach (Enemy enemy in enemies){
                        enemy.setWaypoints(waypoints);
                    }
                }
            }
        }
    }
    
}

