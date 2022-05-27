using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeOfTheRenderer : MonoBehaviour{
    void OnTriggerEnter(Collider c){
        if(c.tag == "RenderSphere" && !gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            
        }
    } 

    void OnTriggerExit(Collider c){
        if(c.tag == "RenderSphere" && gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }  
}
