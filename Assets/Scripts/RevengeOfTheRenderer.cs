using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeOfTheRenderer : MonoBehaviour{
    void Start() {
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh) && gameObject.tag != "Enemy"){
           mesh.enabled = false; 
        }
    }
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
