using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour{
    public bool active=false;
    AudioSource audioSource;
    public AudioClip[] swings;
    AudioClip swing;
    

    float timeCount = 0.0f;
    float speed = 60;
    float yRotation = 0;
    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame. increases sword rotation every frame to do swing animation
    void Update(){
        if(active){
            if (yRotation<=160){
                timeCount = timeCount + Time.deltaTime*2;
                yRotation += timeCount * speed;
                transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-45+yRotation,transform.rotation.eulerAngles.z);
            }else{
                active=!active;
                yRotation=0;
                timeCount = 0.0f;
            }
        }
    }

    //starts swing sound
    private void playSwing(){
        var i = UnityEngine.Random.Range(0,swings.Length);
        swing = swings[i];
        audioSource.clip = swing;
        audioSource.Play();
    }

    //calls playSwing() and flips active variable
    public void attack(){
        playSwing();
        active=true;
    }

    //puts sword gameObject on the player. different base rotation based on whether sword idle or swinging
    public void updatePosition(Transform transform,Vector3 position){
        if(active){
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x+90,transform.rotation.eulerAngles.y-20,transform.rotation.eulerAngles.z-60);
        }else{
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z+60);
        }
        this.transform.position=new Vector3((float)transform.position.x+(1*Mathf.Sin((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)),transform.position.y,transform.position.z+(1*Mathf.Cos((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)));
    }
}
