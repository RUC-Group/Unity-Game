using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    public int speed;
    int score = 0;
    int health = 100;

    int keyAmount=0;
    Door door;

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
        Vector3 direction = input.normalized; 
        Vector3 velocity = direction * speed;
        Vector3 position = velocity * Time.deltaTime;

        transform.Translate(position);
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        // walkinto coin/treasure
        if (triggerCollider.tag == "Treasure"){
            Destroy (triggerCollider.gameObject);
            score++;
            print("Score: " + score);   
        }
        //walk on spikes
        if (triggerCollider.tag == "Spike"){
            health--;
            //print("Player health: " + health);   
        }
        if(triggerCollider.tag == "Key"){
            Destroy (triggerCollider.gameObject);
            keyAmount++;
            print(keyAmount);

        }
        /*
        //walk through door
        if(triggerCollider.tag == "newDoor"){
            Door triggerDoor = triggerCollider.gameObject.GetComponent<Door>();
            if( triggerDoor.passed == false){
                Destroy (triggerCollider.gameObject);
                triggerDoor.passed = true;
                int doorPositionX = (int)triggerDoor.transform.position.x;
                int doorPositionZ = (int)triggerDoor.transform.position.z;
            }
        }
        */
    }
}