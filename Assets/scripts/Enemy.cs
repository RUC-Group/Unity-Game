using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour{
    Dictionary<Vertex, Vertex> pathToPlayer;
    AdjacencyGraph roomGrid;
    List<Transform> waypoints = new List<Transform>();
    Vector3 playerPos;
    Vector3 velocity;
    Vector3 directionToPlayer;
    Vector3 displacementFromPlayer;
    int health = 100;
    bool enemyAlive = true;
    int i;
    float detectionRangeMod = 1;
    float distanceToTarget;
    
    
    public float speed;
    public float detectionRange;
    public float waitTime = .3f;
    public int longestEdge = 5;

    // Start is called before the first frame update
    void Start() {
    }

    public void setWaypoints(List<Transform> w){
        waypoints.Add(transform);
        waypoints.Add(GameObject.FindGameObjectWithTag("Player").transform);
        foreach (var waypoint in w){
            waypoints.Add(waypoint);
        }
    }

    /*
    void setRoomGrid(AdjacencyGraph graph){
        roomGrid = graph;
        playerEnemyGrid = graph;

    }*/
    

    void createGrid(List<Transform> w){
        roomGrid = new AdjacencyGraph();
        List<Transform> waypoints = w;
        foreach (Transform waypoint in waypoints){
            Vertex v = new Vertex(waypoint.position);
            roomGrid.addVertex(v);
            for (int i = 0; i < waypoints.Count; i++){
                if (dist(waypoint.position, waypoints[i].position) < longestEdge && (v.outEdges.Count != 8)){
                    roomGrid.addEdge(v, new Vertex(waypoints[i].position), (float)dist(waypoint.position, waypoints[i].position));
                }
            }
        }
    } 

    double dist(Vector3 a, Vector3 b){
        return Math.Pow(Math.Pow((b.x - a.x),2) + Math.Pow((b.y - a.y),2) + Math.Pow((b.z - a.z),2), (float).5f); // https://www.engineeringtoolbox.com/distance-relationship-between-two-points-d_1854.html
    }

    // Update is called once per frame
    void Update(){
        if(health < 0 ){
            killEnemy();
        }
        else{
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
            displacementFromPlayer = playerPos - transform.position;
            directionToPlayer = displacementFromPlayer.normalized;
            velocity = directionToPlayer * speed;
            distanceToTarget = displacementFromPlayer.magnitude;
            if(detectionRange * detectionRangeMod > distanceToTarget){
                followPlayer();
            }
            else{
                returnToIdle();
            }
        }
    }

    void followPlayer(){
        transform.localScale = new Vector3(1,2,1);
        detectionRangeMod = 2; //expands detection range via multiplication
        //if enemy is far from player, pursue player
        createGrid(waypoints);
        //StartCoroutine(followPath(findPath(dijkstra(roomGrid), roomGrid.GetVertices()[1])));
        transform.Translate(velocity * Time.deltaTime);  
    }

    void returnToIdle(){
        transform.localScale = new Vector3(1,1,1);
        detectionRangeMod = 1;   
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        //walk on spikes
        if (triggerCollider.tag == "Spike" && enemyAlive == true){
            health--;
        }
    }
    void killEnemy(){
        enemyAlive = false;
        Quaternion target = Quaternion.Euler(0,90,90);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
    }

    
    //draws Gizmos (3d objects that can only be seen in the editor and is not displayed on player camera)
    void OnDrawGizmos(){
        if(roomGrid !=null){
            // draw adjacency tree for the enemies
            foreach (Vertex v in roomGrid.GetVertices()){
                Gizmos.DrawSphere(v.pos,.3f);
                foreach (Edge e in v.outEdges){
                    Gizmos.DrawLine(e.from.pos, e.to.pos);
                }
            }
        }
    }

    IEnumerator followPath(List<Vector3> pathPoints){
        print("path size " + pathPoints.Count);
        transform.position = pathPoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = pathPoints[targetWaypointIndex];
        while (true){
            transform.position = Vector3.MoveTowards(transform.position,targetWaypoint,speed * Time.deltaTime);
            if(transform.position == targetWaypoint){
                targetWaypointIndex = (targetWaypointIndex + 1) % pathPoints.Count;
                targetWaypoint = pathPoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;  
        }
    }
        
    

    
    List<Vector3> findPath(Dictionary<Vertex,Vertex> input, Vertex target){
        List<Vector3> res = new List<Vector3>();
        Vertex temp = target;
        Vertex value;
        //print("size of input " + input.Count);

        foreach (var k in input.Keys){
            if(k.Equals(temp)){
                if(input[k]!=null){
                    value = input[k];
                    res.Add(value.pos);
                    temp = k;
                }
                i++;
            }
        }
        return res; 
    }
    


    Dictionary<Vertex,Vertex> dijkstra(AdjacencyGraph graph){
        Dictionary<Vertex,float> d = new Dictionary<Vertex,float>();
        Dictionary<Vertex,Vertex> p = new Dictionary<Vertex,Vertex>();
        MinHeap<Pair> q = new MinHeap<Pair>();
        foreach (Vertex v in graph.GetVertices()){
            d[v] = 100.0f;
            p[v] = null;
            q.insert(new Pair(v, d[v]));
        }
        d[graph.GetVertices()[0]] = 0.0f;
        while (!q.isEmpty()){
            Pair u = q.extractMin();
            foreach (Edge e in u.v.outEdges){
                float alt = d[u.v] + e.weight;
                if (alt < d[e.to]){
                    print("aloha");
                    d[e.to] = alt;
                    p[e.to] = u.v;
                }
            }
        }
        
        return p;
    }

    

    
    /*
    Dictionary<Vertex,Vertex> findPath(Dictionary<Vertex,Vertex> input, Vertex target){
        Dictionary<Vertex,Vertex> res = new Dictionary<Vertex,Vertex>();
        foreach (Vertex v in input){
            if(v == target){
            }
        }
    }

    Dictionary<Vertex,Vertex> dijkstra(Vertex s, AdjacencyGraph graph){

        MinHeap<Pair> q = new MinHeap<Pair>();
        Dictionary<Vertex,int> d = new Dictionary<Vertex,int>();
        Dictionary<Vertex,Vertex> p = new Dictionary<Vertex,Vertex>();
        List<Vertex> allVertecies = graph.GetVertices();
        for (var i = 0; i < allVertecies.Count; i++){
            d[allVertecies[i]] = 100; // big value to simulate inf distance
            p[allVertecies[i]] = null;
            q.insert(new Pair(allVertecies[i], d[allVertecies[i]], i));
        }
        while (!q.isEmpty()){
            Pair pair = q.extractMin();
            foreach (Edge e in pair.v.getEdgeList()){
                if (d[e.from] + e.weight < d[e.to]){
                    d[e.to] = d[e.from] + (int)e.weight;
                    p[e.to] = e.from;
                    q.decreaseKey(pair.index);
                }
            }
        }
        return p;
    }

    void OnDrawGizmos(){
        foreach (Vertex v in Dictionary){
            Gizmos.DrawLine(v.pos, Dictionary[v].pos);            
        }
    }
    */
}

/*

    
    Vector3[] waypoints = new Vector3[path.childCount];
    for (var i = 0; i < waypoints.Length; i++){
        waypoints[i] = path.GetChild(i).position;
    }

    StartCoroutine(followPath(waypoints));

    void OnDrawGizmos(){
        Vector3 startPosition = path.GetChild(0).position;
        Vector3 prevPosition = startPosition;
        foreach (Transform waypoint in path){
            Gizmos.DrawSphere(waypoint.position,.3f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }   
        Gizmos.DrawLine(prevPosition,startPosition);
    }
*/