using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {
    Room[] rooms;
    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
    }

    public void setRoom(int index, Room inputRoom){
        rooms[index] = inputRoom;
    }
}
