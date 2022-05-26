using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{

    public GameObject swordGameObject;
    Sword sword;
    GameUI gameUI;
    GameObject model;

    int score = 0;
    int health = 100;
    int keyAmount = 0;
    float lastDamageTime = 0;

    // Start is called before the first frame update
    void Start(){
        swordGameObject=Instantiate(swordGameObject, new Vector3(0, 0,0),Quaternion.Euler(Vector3.down * 0));
        sword = swordGameObject.transform.GetComponent<Sword>();
        model = transform.Find("Model").gameObject;
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
    }

    // Update is called once per frame
    void Update(){
        if(health > 0){
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
            Vector3 direction = input.normalized; 
            Vector3 velocity = direction * 8;
            Vector3 position = velocity * Time.deltaTime;

            transform.Translate(position);

            if(position!=Vector3.zero){
                model.transform.rotation = Quaternion.LookRotation(position) * Quaternion.Euler(0,270,0);
            }

            sword.updatePosition(model.transform,position);

            if (Input.GetKeyDown(KeyCode.Space))
            {
              sword.attack();
            }
        }else{
            health=0;
            SceneManager.LoadScene(2);
        }
        gameUI.healthPoints=health;
        gameUI.score=score;
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {

        // walkinto coin/treasure
        if (triggerCollider.tag == "Treasure"){
            Destroy (triggerCollider.gameObject);
            score++;

            if(score%10==0) health+=20;  
        }

        //walk on spikes
        if (triggerCollider.tag == "Spike"){
            getDamage();   
        }

        if(triggerCollider.tag == "Key"){
            Destroy (triggerCollider.gameObject);
            GameObject[] keyArray=GameObject.FindGameObjectsWithTag("Key");
            keyAmount=3-keyArray.Length;
            if(keyAmount==1) gameUI.key1.color=new Color32(255, 255, 225, 225);
            if(keyAmount==2) gameUI.key2.color=new Color32(255, 255, 225, 225);
        }

        if(triggerCollider.tag == "Gate"){
            if(keyAmount>=2){
                Destroy (triggerCollider.gameObject);
                SceneManager.LoadScene(2);
            }
        }
    }

    public void getDamage(){

        float timeStamp = Time.time;

        if(timeStamp - lastDamageTime>1){
            health-=10;
            print(health);
            lastDamageTime = timeStamp;
        }
    }
}