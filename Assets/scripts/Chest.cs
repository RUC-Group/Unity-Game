using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour{
    public bool open {get;set;}
    //called once when the game starts, closes the chest if open.
    private void Start(){
        open = false;
    }
    //called once per frame, checks if chest is open or closed.
    private void Update() {
        if(open){
            gameObject.transform.Find("ClosedLid").gameObject.SetActive(false);
            gameObject.transform.Find("OpenLid").gameObject.SetActive(true);
        }
        else{
            gameObject.transform.Find("ClosedLid").gameObject.SetActive(true);
            gameObject.transform.Find("OpenLid").gameObject.SetActive(false);
        }
    }
}

