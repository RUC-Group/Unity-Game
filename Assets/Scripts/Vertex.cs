using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class vertex, used in adjacency graph
public class Vertex{
    public Vector3 pos {get;set;}
    public int index {get;set;}
    public List<Edge> outEdges {get;set;}
    public bool visited  {get;set;}
    
    //constructor of vertex
    public Vertex(Vector3 pos, int i){
        this.pos = pos;
        this.index = i;
        outEdges = new List<Edge>();
        visited = false;    
    }
    //adds an edge to the list of edges 
    public void addEdgeToList(Edge e){
        outEdges.Add(e);
    }
}

