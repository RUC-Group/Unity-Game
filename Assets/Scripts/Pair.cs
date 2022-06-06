using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// pair used inside of dijkstra's algorithm in player
public class Pair:IComparable<Pair>{
    public Vertex v {get;set;}
    public float d {get; set;}
    
    //constructor of pair
    public Pair(Vertex v, float d){
        this.v = v;
        this.d = d;
    }

    //compareTo that compares a pair to a pair based on their d value
    public int CompareTo(Pair o){
        if(this.d > o.d){
            return 1;
        }
        else if(this.d == o.d){
            return 0;

        }else{
            return -1;
        }
    }
}
