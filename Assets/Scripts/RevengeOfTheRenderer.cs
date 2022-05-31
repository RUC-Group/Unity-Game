using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeOfTheRenderer : MonoBehaviour{

    void Start() {
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
           mesh.enabled = false; 
        }
    }
    void OnTriggerEnter(Collider c){
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
            if(c.tag == "RenderSphere" && !mesh.enabled){
                gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    } 

    void OnTriggerExit(Collider c){
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer mesh)){
            if(c.tag == "RenderSphere" && mesh.enabled){
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }  
}
