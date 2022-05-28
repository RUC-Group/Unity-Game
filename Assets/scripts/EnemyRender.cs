using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRender : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider c){
        if(c.tag == "RenderSphere"){
            gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider c){
        if(c.tag == "RenderSphere"){
            gameObject.SetActive(false);
        }
    }
}
