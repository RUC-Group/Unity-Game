using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour{
    bool up = true;
    int currTime = -5;
    int rotate = 0;
    bool active = false;
    
    // Start is called before the first frame update, determine initial orientation relative to world
    void Start(){
        rotate = UnityEngine.Random.Range(0,180); 
    }

    // Update is called once per frame, determine coin movement to be bobbing up and down whilst rotating
    void Update(){
        if(active){
            if(up){
                currTime++;
                transform.position = new Vector3(transform.position.x,transform.position.y + .03f,transform.position.z);
            }
            else{
                currTime--;
                transform.position = new Vector3(transform.position.x,transform.position.y - .03f,transform.position.z);
            }
            if(currTime == 20){
                up = false;
            }else if(currTime == -20){
                up = true;
            }
            rotate++; 
            transform.rotation = Quaternion.Euler(0,rotate,0);
        }       
    }

    //This method is automatically called by Unity, detect initial collision with renderer-sphere to enable mesh-renderer
    void OnTriggerEnter(Collider c){
        if(c.tag == "RenderSphere" && !gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            active = true;
            
        }
    } 

    //This method is automatically called by Unity, detect final collision with renderer-sphere to disable mesh-renderer
    void OnTriggerExit(Collider c){
        if(c.tag == "RenderSphere" && gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            active = false;
        }
    }  
}