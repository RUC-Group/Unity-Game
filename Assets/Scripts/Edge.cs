using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class edge, used in the adjacency graph to connect 
public class Edge{
    public Vertex from {get;set;}
    public Vertex to {get;set;} 
    public float weight {get;set;}
    //constructor for edge
    public Edge(Vertex from, Vertex to, float weight){
        this.from = from;
        this.to = to;
        this.weight = weight;
    }
    // compares this edge with another edge by comparing their weights
    public bool compareTo(Edge o){
        return this.weight > o.weight;
    }
}
