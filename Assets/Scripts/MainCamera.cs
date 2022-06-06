using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour{
    public Transform player;

    // Update is called once per frame. Follows the player gameObject
    void Update(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.transform.position + new Vector3((float)-0.53,(float) 18.38,(float) -20.45);
    }
}