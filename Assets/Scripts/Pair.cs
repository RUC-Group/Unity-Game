using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pair:IComparable<Pair>{
    public Vertex v {get;set;}
    public float d {get; set;}
    public int index;
    public Pair(Vertex v, float d){
        this.v = v;
        this.d = d;
    }

    public int CompareTo(Pair o){
        if(this.d > o.d){
            return 1;
        }
        return 0;
    }

}
