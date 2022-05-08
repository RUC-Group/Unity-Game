using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacencyGraph{
    List<Vertex> vertecies;
    public AdjacencyGraph(){
        vertecies = new List<Vertex>();
    }

    //adds a vertex to the adjacency graph
    public void addVertex(Vertex v){
        vertecies.Add(v);
    }

    public void addEdge(Vertex from, Vertex to, float weight){
        if(!contains(vertecies, from) && contains(vertecies,to)){
            Debug.Log("missing vertecies from graph");
        }
        else{
            Edge newE = new Edge(from, to, weight);
            Edge newErev = new Edge(to, from, weight);
            from.addEdgeToList(newE);
            to.addEdgeToList(newErev);
        }
    }

    public List<Vertex> GetVertices(){
        return vertecies;
    }

    public bool contains(List<Vertex> l, Vertex element){
        for (var i = 0; i < l.Count; i++){
            if (l[i] == element){
                return true;
            }
        }
        return false;
    }

}
