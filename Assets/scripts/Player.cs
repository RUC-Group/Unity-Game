using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
    public int speed;
    int score = 0;
    int health = 100;


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
        }
    }
}