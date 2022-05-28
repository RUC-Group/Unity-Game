using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFade : MonoBehaviour{
    float alpha;
    public Material mat; 
    Color c;
    // Start is called before the first frame update
    void Start(){
        alpha = 2;
    }

    // Update is called once per frame
    void Update(){
        if(alpha > 0){
            alpha -= .01f;
            c = mat.color;
            c.a = alpha;
            mat.color = c;
        }
    }
}
