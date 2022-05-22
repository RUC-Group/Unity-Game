using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex{
    Vector3 pos {get;set;}
    List<Edge> outEdges {get;set;}
    bool visited = false;
    //constructor
    public Vertex(Vector3 pos){
        this.pos = pos;
        outEdges = new List<Edge>();

    }

    public void addEdgeToList(Edge e){
        outEdges.Add(e);
    }
}

