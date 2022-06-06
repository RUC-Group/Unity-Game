using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeOfTheRenderer : MonoBehaviour{

    // called once when the game starts, turns off mesh of everything
    void Start() {
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
           mesh.enabled = false; 
        }
    }
    //everything that collides with the render sphere gets loaded
    void OnTriggerEnter(Collider c){
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
            if(c.tag == "RenderSphere" && !mesh.enabled){
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    } 

    // everything that leaves the renderesphere gets "unloaded"
    void OnTriggerExit(Collider c){
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
            if(c.tag == "RenderSphere" && mesh.enabled){
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }  
}
