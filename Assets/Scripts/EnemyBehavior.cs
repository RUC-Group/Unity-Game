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
    public float waitTime = .3f;

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
            }else{

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

/*

    IEnumerator followPath(Vector3[] waypoints){
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        while (true){
            transform.position = Vector3.MoveTowards(transform.position,targetWaypoint,speed * Time.deltaTime);
            if(transform.position == targetWaypoint){
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;  
        }
    }
        
    Vector3[] waypoints = new Vector3[path.childCount];
    for (var i = 0; i < waypoints.Length; i++){
        waypoints[i] = path.GetChild(i).position;
    }

    StartCoroutine(followPath(waypoints));

    void OnDrawGizmos(){
        Vector3 startPosition = path.GetChild(0).position;
        Vector3 prevPosition = startPosition;
        foreach (Transform waypoint in path){
            Gizmos.DrawSphere(waypoint.position,.3f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }   
        Gizmos.DrawLine(prevPosition,startPosition);
    }
*/
