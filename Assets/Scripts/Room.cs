using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    GameObject[][] roomTiles;

    Vector2 globalPosition;
    
    

    // Start is called before the first frame update
    void Start(){

    }
    public void setTile(int x, int z, GameObject input){
        roomTiles[x][z] = input;
    }

    public void setPosition(Vector2 position){
        globalPosition = position;
    }

    
}
