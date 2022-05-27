using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevengeOfTheRenderer : MonoBehaviour{
    void OnTriggerStay(Collider c){
        if(c != null){
            print("abeBah");
        }
        if(c.tag == "RenderSphere"){
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            print("hello");
        }
        print("outside");
    }   
}
