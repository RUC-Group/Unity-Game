using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour{
    public Transform player;
    public float speed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update(){
        Vector3 displacementFromPlayer = player.position - transform.position;
        Vector3 directionToPlayer = displacementFromPlayer.normalized;
        Vector3 velocity = directionToPlayer * speed;

        transform.Translate(velocity * Time.deltaTime);
        
    }
}
