using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour{
    Vector3 playerPos;
    int health = 100;
    bool enemyAlive = true;
    int i;
    public float speed;
    public float detectionRange;
    float detectionRangeMod = 1;
    // Start is called before the first frame update
    void Start() {
           
    }
    // Update is called once per frame
    void Update(){

        if(health < 0 ){
            killEnemy();

        }
        // else, dmg player or smt.
        else if (enemyAlive == false){
        } 
        else{
        }

        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
        Vector3 displacementFromPlayer = playerPos - transform.position;
        Vector3 directionToPlayer = displacementFromPlayer.normalized;
        Vector3 velocity = directionToPlayer * speed;
        float distanceToTarget = displacementFromPlayer.magnitude;

        //if player is within enemy detection range...
        if(detectionRange * detectionRangeMod > distanceToTarget  && enemyAlive == true){
            transform.localScale = new Vector3(1,2,1);
            detectionRangeMod = 2; //expands detection range via multiplication
            //if enemy is far from player, pursue player
            if(distanceToTarget > 1.5){
                transform.Translate(velocity * Time.deltaTime);  
            }
        }else{
            transform.localScale = new Vector3(1,1,1);
            detectionRangeMod = 1;            
        }   
    }
    
    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        //walk on spikes
        if (triggerCollider.tag == "Spike" && enemyAlive == true){
            health--;
            //print("Enemy health: " + health);   
        }
    }
    void killEnemy(){
        enemyAlive = false;
        Quaternion target = Quaternion.Euler(0,90,90);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
    }
}
