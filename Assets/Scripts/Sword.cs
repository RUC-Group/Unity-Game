using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public bool active=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatePosition(Vector3 position){
        if(active){
            
        }else{
        transform.position=new Vector3(position.x+1,position.y+3,position.z+1);
        transform.rotation=Quaternion.Euler(0,0,150);
        }
    }
}
