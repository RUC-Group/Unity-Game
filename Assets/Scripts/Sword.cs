using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool active=false;

    float timeCount = 0.0f;
    float speed = 60;
    float yRotation = 0;

    // Update is called once per frame
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

    public void attack(){
        active=true;
    }

    public void updatePosition(Transform transform,Vector3 position){
        if(active){
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x+90,transform.rotation.eulerAngles.y-20,transform.rotation.eulerAngles.z-60);
        }else{
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z+60);
        }
        this.transform.position=new Vector3((float)transform.position.x+(1*Mathf.Sin((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)),transform.position.y,transform.position.z+(1*Mathf.Cos((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)));
    }
}
