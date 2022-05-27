using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSphere : MonoBehaviour{
   Transform player;

    void Update(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
    }

    void OnTriggerStay(Collider c){
        c.gameObject.GetComponent<MeshRenderer>().enabled = true;
        print("collision");
    }

    private void OnTriggerExit(Collider c){
        c.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
