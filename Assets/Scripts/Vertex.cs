using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex{
    Vector3 pos;
    List<Edge> outEdges;
    bool visited = false;
    //constructor
    public Vertex(Vector3 pos){
        this.pos = pos;
        outEdges = new List<Edge>();

    }

    public void addEdgeToList(Edge e){
        outEdges.Add(e);
    }

    public Vector3 getPos(){
        return pos;
    }

    public List<Edge> getEdgeList(){
        return this.outEdges;
    }

}
