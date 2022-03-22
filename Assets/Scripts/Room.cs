using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour{
    GameObject[,] roomTiles = new GameObject[5,5];
    public int nameOfRoom;

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
    
    public void setRoom(int posX, int posZ, int roomSize, int name){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;
        this.nameOfRoom = name;
    }
    

    // Start is called before the first frame update
    void Start(){
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

    public async void showRoom(){
        for(int i = -1; i < roomSize+1; i++){
            for(int j = -1; j< roomSize+1; j++){
                var position = new Vector3(i*5, 0, j*5 + posZ);
                var rotation = ;
                
                if(i == -1 && j==-1 || i==-1 && j==roomSize || i==roomSize && j == -1 || i==roomSize && j ==roomSize){
                    Instantiate(cornerTile, position, Quaternion.identity);
                }else if(i==-1 || j == -1 || i== roomSize || j == roomSize){
                    Instantiate(wallTile, position, Quaternion.Euler(Vector3.down * rotation));
                }
                else{
                    Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
    }

    
}
