using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour{
    bool up = true;
    int currTime = -5;
    int rotate = 0;
    bool active = false;
    // Start is called before the first frame update
    void Start(){
        rotate = UnityEngine.Random.Range(0,180); 
         
    }

    // Update is called once per frame
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

    void OnTriggerEnter(Collider c){
        if(c.tag == "RenderSphere" && !gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            active = true;
            
        }
    } 

    void OnTriggerExit(Collider c){
        if(c.tag == "RenderSphere" && gameObject.GetComponent<MeshRenderer>().enabled){
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            active = false;
        }
    }  
}
