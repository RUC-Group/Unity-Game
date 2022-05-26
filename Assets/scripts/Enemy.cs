using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour{
    AdjacencyGraph roomGrid;
    List<Transform> waypoints = new List<Transform>();
    Vector3 playerPos;
    Vector3 velocity;
    Vector3 directionToPlayer;
    Vector3 displacementFromPlayer;
    int health = 100;
    bool enemyAlive = true;
    float detectionRangeMod = 1;
    float distanceToTarget;
    float speed = 5;
    float detectionRange = 5;
    float waitTime = .1f;
    float longestEdge = 4.5f;
    float lastScan = 0;
    List<Vector3> pathToFollow;

    public void setWaypoints(List<Transform> w){
        waypoints.Add(transform);
        waypoints.Add(GameObject.FindGameObjectWithTag("Player").transform);

        foreach (Transform waypoint in w){
            waypoints.Add(waypoint);
        }
    }

    void createGrid(){
        roomGrid = new AdjacencyGraph();

        foreach (Transform waypoint in waypoints){
            Vertex v = new Vertex(waypoint.position);
            roomGrid.addVertex(v);
        }

        foreach (Vertex v in roomGrid.GetVertices()){
            foreach (Vertex v2 in roomGrid.GetVertices()){
                float distBetween = (float)dist(v.pos, v2.pos);

                if (distBetween < longestEdge && distBetween != 0) {
                    roomGrid.addEdge(v, v2, distBetween);
                }
            }
        }
    } 

    double dist(Vector3 a, Vector3 b){
        return Math.Pow(Math.Pow((b.x - a.x),2) + Math.Pow((b.y - a.y),2) + Math.Pow((b.z - a.z),2), (float).5f); // https://www.engineeringtoolbox.com/distance-relationship-between-two-points-d_1854.html
    }

    bool checkScanTimer(){
        float timeStamp = Time.time;

        if(timeStamp<2 || timeStamp - lastScan>2){
            return true;
        }else{
            return false;
        }
    }

    // Update is called once per frame
    void Update(){
        if(health < 0 ){
            killEnemy();
        }else{
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
            displacementFromPlayer = playerPos - transform.position;
            directionToPlayer = displacementFromPlayer.normalized;
            velocity = directionToPlayer * speed;
            distanceToTarget = displacementFromPlayer.magnitude;

            if(detectionRange * detectionRangeMod > distanceToTarget){
                followPlayer();
            }else{
                returnToIdle();
            }
        }
    }

    void followPlayer(){

        transform.localScale = new Vector3(1,2,1);
        detectionRangeMod = 4; //expands detection range via multiplication

        if(checkScanTimer()){
            lastScan = Time.time;
            createGrid();
            pathToFollow = findPath(dijkstra(), roomGrid.GetVertices()[1]);
            StartCoroutine(followPath(pathToFollow));
        }
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
        if(pathToFollow != null){
            Vector3 shift = new Vector3(0,2,0);
            for(var i =0; i<pathToFollow.Count; i++){
                Gizmos.DrawSphere(pathToFollow[i] + shift, .3f);
                if(i < pathToFollow.Count - 1){
                    Gizmos.DrawLine(pathToFollow[i] + shift, pathToFollow[1+i] + shift);
                }
            }
        }
    }

    IEnumerator followPath(List<Vector3> pathPoints){
        int targetWaypointIndex = 0;
        Vector3 targetWaypoint = new Vector3(pathPoints[targetWaypointIndex].x,transform.position.y,pathPoints[targetWaypointIndex].z);
        float timeStamp = Time.time;

        while (targetWaypointIndex<= pathPoints.Count-1 && !checkScanTimer()){
            timeStamp = Time.time;
            targetWaypoint = new Vector3(pathPoints[targetWaypointIndex].x,transform.position.y,pathPoints[targetWaypointIndex].z);
            transform.position = Vector3.MoveTowards(transform.position,targetWaypoint,speed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetWaypoint) < 0.001f){
                targetWaypointIndex++;
                yield return new WaitForSeconds(waitTime);
            }
            lastScan=0;
            yield return null;  
        }
    }
        
    

    
    List<Vector3> findPath(Dictionary<Vertex,Vertex> input, Vertex target){
        List<Vector3> res = new List<Vector3>();
        Vertex temp = target;
        Vertex startVertex = roomGrid.GetVertices()[0];

        if (temp != null && startVertex != null){
            while (!temp.Equals(startVertex) && !checkScanTimer()){
                res.Add(temp.pos);
                temp = input[temp];

                if(temp==null){
                    temp = startVertex;
                    res = new List<Vector3>();
                    print("temp is null");
                    break;
                }
            }
            res.Add(temp.pos);
            res.Reverse();
        }
        return res; 
    }
    

    Dictionary<Vertex,Vertex> dijkstra(){
        Dictionary<Vertex,float> d = new Dictionary<Vertex,float>();
        Dictionary<Vertex,Vertex> p = new Dictionary<Vertex,Vertex>();
        MinHeap<Pair> q = new MinHeap<Pair>();

        foreach (Vertex v in roomGrid.GetVertices()){

            d[v] = 100.0f;
            p[v] = null;
            q.insert(new Pair(v, d[v]));
        }

        d[roomGrid.GetVertices()[0]] = 0.0f;
        while (!q.isEmpty()&&!checkScanTimer()){
            Pair u = q.extractMin();
            foreach (Edge e in u.v.outEdges){
                float alt = d[u.v] + e.weight;
                if (alt < d[e.to]){
                    d[e.to] = alt;
                    p[e.to] = u.v;
                }
            }
        }
        return p;
    }
}