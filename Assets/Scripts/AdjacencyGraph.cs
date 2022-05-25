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
        //Add edges to the adjacency graph if the two vertices exist in the adjacency graph
        if(contains(vertecies, from) && contains(vertecies,to)){
            Edge newE = new Edge(from, to, weight);
            Edge newErev = new Edge(to, from, weight);
            from.addEdgeToList(newE);
            to.addEdgeToList(newErev);
        }
    }

    public List<Vertex> GetVertices(){
        //return list of all verticies
        return vertecies;
    }

    public bool contains(List<Vertex> l, Vertex element){
        //checks if the given vertex exist in the adjacency graph
        for (var i = 0; i < l.Count; i++){
            if (l[i] == element){
                return true;
            }
        }
        return false;
    }
}
