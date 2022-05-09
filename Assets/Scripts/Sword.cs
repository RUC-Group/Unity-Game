using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool active=false;
    private float timeCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update(){
        if(active){
            if(transform.rotation==Quaternion.Euler(0,0,150)){
                transform.rotation=Quaternion.Lerp(Quaternion.Euler(90,0,0),Quaternion.Euler(90,170,0),timeCount);
            }
            timeCount = timeCount + Time.deltaTime*2;
            if(transform.rotation==Quaternion.Euler(90,170,0)){
                timeCount = 0.0f;
                active = false;
            }
        }
    }

    public void attack(){
        //transform.rotation=Quaternion.Euler(90,45,0);
        active=true;
    }

    public async void updatePosition(Transform transform,Vector3 position){
        this.transform.rotation = transform.rotation;
        print(this.transform.rotation.eulerAngles.y);
        if(active){
            this.transform.position=new Vector3((float)(transform.position.x+(100*transform.rotation.eulerAngles.x)),(float)(transform.position.y-0.2),(float)(transform.position.z+(100*transform.rotation.eulerAngles.z)));
        }else{
            this.transform.position=new Vector3((float)transform.position.x+(1*Mathf.Sin((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)),transform.position.y+1,transform.position.z+(1*Mathf.Cos((this.transform.rotation.eulerAngles.y+45)*Mathf.Deg2Rad)));
            //this.transform.rotation=Quaternion.Euler(0,0,150);
        }
    }
}
