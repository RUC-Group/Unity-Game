using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex{
    public Vector3 pos {get;set;}
    public int index {get;set;}
    public List<Edge> outEdges {get;set;}
    public bool visited  {get;set;}
    
    //constructor
    public Vertex(Vector3 pos, int i){
        this.pos = pos;
        this.index = i;
        outEdges = new List<Edge>();
        visited = false;    
    }

    public void addEdgeToList(Edge e){
        outEdges.Add(e);
    }
}

