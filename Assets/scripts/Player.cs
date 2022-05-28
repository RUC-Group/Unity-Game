using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour{

    public GameObject swordGameObject;
    Sword swordModel;

    BoxCollider swordHitBox;
    GameUI gameUI;
    GameObject model;
    Vector3 input;
    public GameObject coin;
    public Transform pauseMenu;
    public GameObject what;

    float lastScan = 0;
    int score = 0;
    int health = 101;
    int keyAmount = 0;
    float lastDamageTime = 0;
    float timeStamp;
    float lastHeal = 0;
    float lastStamina = 0;
    int stamina = 101;
    int maxStamina = 101;

    // Start is called before the first frame update
    void Start(){
        //swordGameObject=Instantiate(swordGameObject, new Vector3(0, 0,0),Quaternion.Euler(Vector3.down * 0));
        swordModel = transform.Find("sword").gameObject.transform.GetComponent<Sword>();
        model = transform.Find("Model").gameObject;
        swordHitBox = model.transform.Find("SwordHitbox").gameObject.transform.GetComponent<BoxCollider>();
        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        gameUI.currHealth = health;
        gameUI.score = score;
        gameUI.stamina = stamina;
    }

   

    // Update is called once per frame
    void Update(){
        GameObject tempCanvas = GameObject.Find("PauseMenu");
        GameObject tempOptionCanvas = GameObject.Find("PauseOPTIONSmenu");
        if(Input.GetKeyDown(KeyCode.Escape) && !tempCanvas.GetComponent<Canvas>().enabled && !tempOptionCanvas.GetComponent<Canvas>().enabled){
            if(tempCanvas != null){
               tempCanvas.GetComponent<Canvas>().enabled = true; 
            }
        }

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

            // Dash
            if(Input.GetKeyDown(KeyCode.LeftShift) && stamina > 50){
                lastScan = Time.time;
                StartCoroutine(dash(transform.Find("Model").Find("Dash Destination").position + new Vector3(0,1,0)));
                stamina -= 50;
            }
            if(stamina<maxStamina){
                addStamina(5);

            }
          
           // Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
            Vector3 direction = input.normalized; 
            Vector3 velocity = direction * 8;
            Vector3 position = velocity * Time.deltaTime;

            transform.Translate(position);

            //Vector3 lookDirection=new Vector3(Input.GetAxisRaw("LookHorizontal"),0 ,Input.GetAxisRaw("LookVertical"));
            if(position!=Vector3.zero){
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
          

           // Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"),0 ,Input.GetAxisRaw("Vertical"));   
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

        }else{
            health=0;
            SceneManager.LoadScene(2);
        }
        gameUI.stamina = stamina;
        gameUI.currHealth = health;
        gameUI.score = score;
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {

        // walkinto coin/treasure
        if (triggerCollider.tag == "Treasure"){
            Destroy (triggerCollider.gameObject);
            score++;

            if(score%10==0) health+=20;  
        }
        if(triggerCollider.tag == "Bonfire" && health < 101){
            getHealth();
        }

        //walk on spikes
        if (triggerCollider.tag == "Spike"){
            getDamage();   
        }

        if(triggerCollider.tag == "chest"){
            if(!triggerCollider.GetComponent<Chest>().open){
                Instantiate(coin,triggerCollider.transform);
            }
            triggerCollider.GetComponent<Chest>().open = true;
        }
     
        if (triggerCollider.tag == "ChineseSuicidePreventionMethod"){
            transform.position = new Vector3(transform.position.x,4000000,transform.position.z);
            health -= 50;
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
                SceneManager.LoadScene(3);
            }
        }
    }

    bool checkScanTimer(int input){
        float timeStamp = Time.time;
        return timeStamp<input || timeStamp - lastScan>input;
    }


    IEnumerator dash(Vector3 destination){
        while (Vector3.Distance(transform.position, destination) > 0.5f){
            transform.position = Vector3.MoveTowards(transform.position,destination ,1);
            yield return null;
        }
    }

    public void getDamage(){
        timeStamp = Time.time;

        if(timeStamp - lastDamageTime>2){
            health-=10;
            lastDamageTime = timeStamp;
        }
    }

    private void addStamina(int staminaToAdd){
        float timeStamp = Time.time;

        if(timeStamp - lastStamina>.5){
            stamina+=staminaToAdd;
            lastStamina = timeStamp;
        }
    }

    public void getHealth(){
        float timeStamp = Time.time;

        if(timeStamp - lastHeal>1){
            health+=5;
            lastHeal = timeStamp;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.Find("Model").Find("Dash Destination").position, .3f);
    }
}