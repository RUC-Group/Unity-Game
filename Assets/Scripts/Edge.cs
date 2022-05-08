using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge{
    public Vertex from {get;set;}
    public Vertex to {get;set;} 
    public float weight {get;set;}
    public Edge(Vertex from, Vertex to, float weight){
        this.from = from;
        this.to = to;
        this.weight = weight;
    }
    public bool compareTo(Edge o){
        return this.weight > o.weight;
    }
}
