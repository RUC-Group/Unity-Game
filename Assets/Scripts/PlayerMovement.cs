using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    public GameObject swordGameObject;
    Sword sword;
    public int speed;
    int score = 0;
    int health = 100;

    int keyAmount=0;

    bool alive=true;

    float lastDamageTime=0;
    Door door;

    // Start is called before the first frame update
    void Start(){
        swordGameObject=Instantiate(swordGameObject, new Vector3(0, 0,0),Quaternion.Euler(Vector3.down * 0));
        sword = swordGameObject.GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update(){
        //swordGameObject = GameObject.FindGameObjectWithTag("Player Sword");
        sword = swordGameObject.GetComponent<Sword>();
        if(alive){
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
            Vector3 direction = input.normalized; 
            Vector3 velocity = direction * speed;
            Vector3 position = velocity * Time.deltaTime;

            transform.Translate(position);
            sword.move(transform.position);
        }else{
            Quaternion target = Quaternion.Euler(0,90,90);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        }
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
        if(triggerCollider.tag == "Gate"){
            if(keyAmount==2){
                Destroy (triggerCollider.gameObject);
                print("You can pass");
            }else{
                print("You dont have enough keys");
            }
        }
    }

    public void getDamage(){
        float timeStamp = Time.time;
        if(timeStamp - lastDamageTime>1){
            health-=25;
            print(health);
            lastDamageTime = timeStamp;
        }
        if(health<=0){
            alive=false;
        }
    }
}