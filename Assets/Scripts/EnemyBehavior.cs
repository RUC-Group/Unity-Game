using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour{
    Vector3 playerPos;
    int health = 100;
    bool enemyAlive = true;
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
        if(health < 0 ){
            killEnemy();

        }

        //if enemy is far from player, walk to player
        if(distanceToTarget > 1.5 && enemyAlive == true){
            transform.Translate(velocity * Time.deltaTime);   
        }
        // else, dmg player or smt.
        else if (enemyAlive == false){
        } 
        else{
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
