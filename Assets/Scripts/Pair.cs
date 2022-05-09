using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pair{
    public Vertex v {get;set;}
    public int d {get; set;}
    public int index;
    public Pair(Vertex v, int d, int i){
        this.v = v;
        this.d = d;
        this.index = i; 
    }

}
