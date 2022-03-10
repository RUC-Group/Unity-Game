using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    public int speed;
    int score = 0;
    int Health = 100;
    Spawner spawner;
    public Transform door;

    // Start is called before the first frame update
    void Start(){
        spawner = GameObject.FindGameObjectWithTag("newDoor").GetComponent<Spawner>();
    }

    // Update is called once per frame
    void Update(){
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
        Vector3 direction = input.normalized; 
        Vector3 velocity = direction * speed;
        Vector3 position = velocity * Time.deltaTime;

        transform.Translate(position);
    }

    void OnTriggerEnter(Collider triggerCollider) {
        if (triggerCollider.tag == "Treasure"){
            Destroy (triggerCollider.gameObject);
            score++;
            print("Score: " + score);   
        }

        if (triggerCollider.tag == "Spike"){
            Health--;
            print("Health: " + Health);   
        }
        if(triggerCollider.tag == "newDoor"){
            if (spawner.passed == false){
                spawner.passed = true;
                spawner.makeRoom(0,0);
            } 
        }
    }
}
