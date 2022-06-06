using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSphere : MonoBehaviour{
   Transform player;

    //gameObject stays on the player
    void Update(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
    }
}
