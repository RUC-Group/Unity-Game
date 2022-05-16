using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair{
    public Vertex v {get;set;}
    public float d {get; set;}
    public int index;
    public Pair(Vertex v, float d){
        this.v = v;
        this.d = d;
    }

}
