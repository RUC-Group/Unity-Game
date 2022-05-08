using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge{
    Vertex from;
    Vertex to; 
    float weight;
    public Edge(Vertex from, Vertex to, float weight){
        this.from = from;
        this.to = to;
        this.weight = weight;
    }
    public bool compareTo(Edge o){
        return this.weight > o.weight;
    }

    public Vertex getTo(){
        return to;
    }

    public Vertex getFrom(){
        return from;
    }
}
