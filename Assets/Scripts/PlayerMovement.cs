using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    public int speed;
    int score = 0;
    int health = 100;
    Spawner spawner;
    Door door;

    // Start is called before the first frame update
    void Start(){
        spawner = GameObject.FindGameObjectWithTag("RoomSpawner").GetComponent<Spawner>();
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
            print("Player health: " + health);   
        }
        
        //walk through door
        if(triggerCollider.tag == "newDoor"){
            Door triggerDoor = triggerCollider.gameObject.GetComponent<Door>();
            if( triggerDoor.passed == false){
                Destroy (triggerCollider.gameObject);
                triggerDoor.passed = true;
                int doorPositionX = (int)triggerDoor.transform.position.x;
                int doorPositionZ = (int)triggerDoor.transform.position.z;
                spawner.makeRoom(doorPositionX, doorPositionZ);
            }
        }
        
    }
}
/*
    void OnTriggerEnter(Collider other){
        if(other.tag == "newDoor"){
            other.passed = true;
                int doorPositionX = (int)GameObject.FindGameObjectWithTag("newDoor").transform.position.x;
                int doorPositionZ = (int)GameObject.FindGameObjectWithTag("newDoor").transform.position.z;
                spawner.makeRoom(doorPositionX, doorPositionZ);
        }
    }
}
*/
