using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{
    Sword swordModel;
    BoxCollider swordHitBox;
    GameUI gameUI;
    GameObject model;
    Vector3 input;

    public GameObject coin;
    public AudioSource a;
    public AudioClip[] footsteps;
    public AudioSource audioSource;
    public GameObject swordGameObject;
    public bool invincible {get;set;}

    bool lastHopUp = false;
    float lastScan = 0;
    int score = 0;
    int health = 101;
    int keyAmount = 0;
    float lastDamageTime = 0;
    float lastFootStep = 0;
    float lasthop = 0;
    float lastHeal = 0;
    float lastStamina = 0;
    float timeStamp;
    int stamina = 101;
    int maxStamina = 101;
    int disableTime = 0;
    bool SoundPlaying = false;
    GameObject infoBoard;

    // Start is called before the first frame update
    void Start(){
        audioSource = GetComponent<AudioSource>();
        swordModel = transform.Find("sword").gameObject.transform.GetComponent<Sword>();
        model = transform.Find("Model").gameObject;
        swordHitBox = model.transform.Find("SwordHitbox").gameObject.transform.GetComponent<BoxCollider>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        gameUI.currHealth = health;
        gameUI.score = score;
        gameUI.stamina = stamina;
        invincible=false;
    }

    // Update is called once per frame
    void Update(){
        GameObject tempCanvas = GameObject.Find("PauseMenu");
        GameObject tempOptionCanvas = GameObject.Find("PauseOPTIONSmenu");
        if(Input.GetKeyDown(KeyCode.Escape) && !tempCanvas.GetComponent<Canvas>().enabled && !tempOptionCanvas.GetComponent<Canvas>().enabled){
            tempCanvas.GetComponent<Canvas>().enabled = true; 
        }

        //if player is alive
        if(health > 0){
            // movement direction
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            if(y == 1 && x == 1){
                input = new Vector3(1,0,0); 
            }else if(y == 1 && x == 0){
                input = new Vector3(1,0,1); 
            }else if(y == 1 && x == -1){
                input = new Vector3(0,0,1); 
            }else if(y == 0 && x == -1){
                input = new Vector3(-1,0,1); 
            }else if(y == -1 && x == -1){
                input = new Vector3(-1,0,0); 
            }else if(y == -1 && x == 0){
                input = new Vector3(-1,0,-1); 
            }else if(y == -1 && x == 1){
                input = new Vector3(0,0,-1); 
            }else if(y == 0 && x == 1){
                input = new Vector3(1,0,-1); 
            }else{
                input = new Vector3(0,0,0); 
            }

            //footsteps sound
            timeStamp = Time.time;
            if(input != Vector3.zero && !SoundPlaying){
                var i = UnityEngine.Random.Range(0,footsteps.Length);
                AudioClip footstepClip = footsteps[i]; 
                audioSource.clip = footstepClip;
                SoundPlaying = true;
                audioSource.Play();
                lastFootStep = timeStamp;
            }else if(SoundPlaying && timeStamp - lastFootStep>.5){
                SoundPlaying = false;
            }

            // Dash
            if(Input.GetKeyDown(KeyCode.LeftShift) && stamina >=30){
                invincible = true;
                StartCoroutine(dash(transform.Find("Model").Find("Dash Destination").position + new Vector3(0,1,0)));
                stamina -= 30;
            }
            //stamina
            if(stamina<maxStamina){
                addStamina(10);
            }
            //visual bounce
            Vector3 direction = input.normalized; 
            Vector3 velocity = direction * 8;
            Vector3 position = velocity * Time.deltaTime;
            transform.Translate(position);
            if(position!=Vector3.zero){
                timeStamp = Time.time;
                if(timeStamp - lasthop>.2f && !lastHopUp){
                    model.transform.position = model.transform.position + new Vector3(0,.5f,0); 
                    lasthop = timeStamp;
                    lastHopUp = true;
                }else if(timeStamp - lasthop>.2f && lastHopUp){
                    model.transform.position = model.transform.position + new Vector3(0,-.5f,0); 
                    lasthop = timeStamp;
                    lastHopUp = false;
                }
                model.transform.rotation = Quaternion.LookRotation(position) * Quaternion.Euler(0,270,0);
            }
            // look direction
            x = Input.GetAxisRaw("LookHorizontal");
            y = Input.GetAxisRaw("LookVertical");
            if(y == 1 && x == 1){
                input = new Vector3(1,0,0); 
            }else if(y == 1 && x == 0){
                input = new Vector3(1,0,1); 
            }else if(y == 1 && x == -1){
                input = new Vector3(0,0,1); 
            }else if(y == 0 && x == -1){
                input = new Vector3(-1,0,1);  
            }else if(y == -1 && x == -1){
                input = new Vector3(-1,0,0); 
            }else if(y == -1 && x == 0){
                input = new Vector3(-1,0,-1); 
            }else if(y == -1 && x == 1){
                input = new Vector3(0,0,-1); 
            }else if(y == 0 && x == 1){
                input = new Vector3(1,0,-1); 
            }else{
                input = new Vector3(0,0,0); 
            }
            direction = input.normalized; 
            velocity = direction * 8;
            Vector3 lookDirection = velocity * Time.deltaTime;
            

            if(input!=Vector3.zero){
                model.transform.rotation = Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0,270,0);
            }
            swordModel.updatePosition(model.transform,position);
            if (Input.GetKeyDown(KeyCode.Space) && stamina>10 && !swordModel.active){
                stamina-=20;
                swordModel.attack();
                swordHitBox.enabled=true;
            }else{
                swordHitBox.enabled=false;
            }
        // else if player is dead
        }else{
            health=0;
            SceneManager.LoadScene(2);
        }
        // send UI information
        gameUI.stamina = stamina;
        gameUI.currHealth = health;
        gameUI.score = score;
        disableTime--;
    }


    //hitbox events
    void OnTriggerStay(Collider triggerCollider){
        //collision with bonfire
        if(triggerCollider.tag == "Bonfire" && health < 101){
            addHealth();
        }
        //collision with chest
        if(triggerCollider.tag == "chest"){
            infoBoard = GameObject.Find("ChestInfo");
            if(!triggerCollider.transform.GetComponent<Chest>().open){
                infoBoard.transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            }
            disableTime = 50;
            if(Input.GetKeyDown(KeyCode.E)){
                triggerCollider.GetComponent<AudioSource>().Play();
                infoBoard.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
                if(!triggerCollider.transform.GetComponent<Chest>().open){
                    Instantiate(coin,triggerCollider.transform);
                }
                triggerCollider.GetComponent<Chest>().open = true;
            }
        }

        //collision with spikes
        if (triggerCollider.tag == "Spike"){
            timeStamp = Time.time;
            if(timeStamp - lastDamageTime>2 && !invincible){
                takeDamage(10);
                lastDamageTime = timeStamp;
            }  
        }
    }

    // when leaving a collider
    void OnTriggerExit(Collider triggerCollider) {
        //leaving collision of a with coin
        if (triggerCollider.tag == "Treasure"){
            Destroy (triggerCollider.gameObject);
            score++;
            if(score%10==0) health+=20;  
        }
     
        //leaving collision of a with key
        if(triggerCollider.tag == "Key"){
            a.Play();
            GameObject[] keyArray=GameObject.FindGameObjectsWithTag("Key");
            keyAmount=3-keyArray.Length;
            if(keyAmount==1) gameUI.key1.color=new Color32(255, 255, 225, 225);
            if(keyAmount==2) gameUI.key2.color=new Color32(255, 255, 225, 225);
            Destroy(triggerCollider.gameObject);   
        }

        //leaving collision of a with gate
        if(triggerCollider.tag == "Gate"){
            infoBoard = GameObject.Find("keyInfo");
            infoBoard.transform.GetChild(0).GetComponent<Canvas>().enabled = true;
            disableTime = 50;
            if(keyAmount>=2){
                SceneManager.LoadScene(3);
            }
        }
        if(disableTime<0 && infoBoard != null){
            infoBoard.transform.GetChild(0).GetComponent<Canvas>().enabled = false;
        }
    }
    
    //method that checks if some time has passed between now (timeStamp), and some other moment (lastScan)
    bool checkScanTimer(int input){
        float timeStamp = Time.time;
        return timeStamp<input || timeStamp - lastScan>input;
    }

    //dash, made as a coroutine
    IEnumerator dash(Vector3 destination){
        while (Vector3.Distance(transform.position, destination) > 1f){
            transform.position = Vector3.MoveTowards(transform.position,destination ,1);
            yield return null;
        }
        invincible = false;
    }

    //deal damage to the player
    public void takeDamage(int input){
        health-=input;
    }

    // add stamina to the player
    private void addStamina(int staminaToAdd){
        float timeStamp = Time.time;
        if(timeStamp - lastStamina>.5){
            stamina+=staminaToAdd;
            lastStamina = timeStamp;
        }
    }

    //Adds health to the player
    public void addHealth(){
        float timeStamp = Time.time;
        if(timeStamp - lastHeal>1){
            health+=5;
            lastHeal = timeStamp;
        }
    }
    // draws the position the player will be on if he dashes as a gizmo
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.Find("Model").Find("Dash Destination").position, .3f);
    }
}