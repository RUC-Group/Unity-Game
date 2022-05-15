using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool active=false;
    private float timeCount = 0.0f;
    private bool start=false;

    private float speed = 5;
    float yRotation = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update(){
        if(active){
            if (timeCount<=2){
                yRotation += timeCount * speed;
                transform.rotation=Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y-90+yRotation,transform.rotation.eulerAngles.z);
            }else{
                yRotation=0;
                timeCount = 0.0f;
                active = false;
            }
            //print(timeCount);
            timeCount = timeCount + Time.deltaTime*2;
        }
    }

    public void attack(){
        active=true;
        start=true;
    }

    public async void updatePosition(Transform transform,Vector3 position){
        this.transform.position=new Vector3((float)transform.position.x+(1*Mathf.Sin((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)),transform.position.y+1,transform.position.z+(1*Mathf.Cos((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)));
        
        if(active){
            this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x+90,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
        }else{
            this.transform.rotation = transform.rotation;
        }
    }
}
