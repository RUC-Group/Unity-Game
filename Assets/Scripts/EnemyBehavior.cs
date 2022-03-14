using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour{
    Vector3 playerPos;
    int health = 100;
    int i;
    public float speed;
    // Start is called before the first frame update
    void Start() {
           
    }
    // Update is called once per frame
    void Update(){
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
        Vector3 displacementFromPlayer = playerPos - transform.position;
        Vector3 directionToPlayer = displacementFromPlayer.normalized;
        Vector3 velocity = directionToPlayer * speed;
        float distanceToTarget = displacementFromPlayer.magnitude;

        //if enemy is far from player, walk to player
        if(distanceToTarget > 1.5){
            transform.Translate(velocity * Time.deltaTime);   
        } 
        // else, dmg player or smt.
        else{
            
        }   
        
    }
    
    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        //walk on spikes
        if (triggerCollider.tag == "Spike"){
            health--;
            print("Health: " + health);   
        }
    }
}
