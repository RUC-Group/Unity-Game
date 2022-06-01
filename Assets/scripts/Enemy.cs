using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour{
    public GameObject coin;
    AdjacencyGraph roomGrid;
    public float speed = 5;
    public int health = 100;
    public int damage = 10;
    Dictionary<Vertex,Vertex> dijkstraRes;

    Player player;
    List<Transform> waypoints = new List<Transform>();
    Vector3 playerPos;
    Vector3 velocity;
    Vector3 directionToPlayer;
    Vector3 displacementFromPlayer;
    bool enemyAlive = true;
    float detectionRangeMod = 1;
    float distanceToTarget;
    float detectionRange = 20;
    float waitTime = .1f;
    float longestEdge = 4.5f;
    float lastScan = 0;
    float timeStamp;
    int turnSpeed = 360;
    List<Vector3> pathToFollow;
    float angle;
    float lastDamageTime = 0;
    float lastDamageDealtTime = 0;

    public void setWaypoints(List<Transform> w){
        waypoints.Add(transform);
        waypoints.Add(GameObject.FindGameObjectWithTag("Player").transform);

        foreach (Transform waypoint in w){
            waypoints.Add(waypoint);
        }
    }

    void createGrid(){
        roomGrid = new AdjacencyGraph();
        int i = 0;
        foreach (Transform waypoint in waypoints){
            Vertex v = new Vertex(waypoint.position,i);
            roomGrid.addVertex(v);
            i++;
        }

        foreach (Vertex v in roomGrid.GetVertices()){
            foreach (Vertex v2 in roomGrid.GetVertices()){
                float distBetween = Vector3.Distance(v.pos, v2.pos);

                if (distBetween < longestEdge && distBetween != 0) {
                    roomGrid.addEdge(v, v2, distBetween);
                }
            }
        }
    } 

    bool checkScanTimer(){
        timeStamp = Time.time;
        return timeStamp<2 || timeStamp - lastScan>2;
    }

    // Update is called once per frame
    void Update(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(!enemyAlive){
            transform.Find("SpottetMarker").gameObject.SetActive(false);
            Quaternion target = Quaternion.Euler(0,90,90);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        }
        else if(health <= 0){
            killEnemy();
        }else{
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
            displacementFromPlayer = playerPos - transform.position;
            directionToPlayer = displacementFromPlayer.normalized;
            velocity = directionToPlayer * speed;
            distanceToTarget = displacementFromPlayer.magnitude;

            if(detectionRange> distanceToTarget){
                Vector3 dirToLookTarget = (player.transform.position - transform.position).normalized;
                float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
                float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle + 270, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                if (distanceToTarget>2) followPlayer();
                else {
                    timeStamp = Time.time;
                    if(timeStamp - lastDamageDealtTime>2){
                        player.takeDamage(damage);
                        lastDamageDealtTime = timeStamp;
                    }
                }
            }else{
                returnToIdle();
            }
        }
        
    }

    void followPlayer(){  
        if(pathToFollow != null){
            transform.Find("SpottetMarker").gameObject.SetActive(true);
        }else{
            transform.Find("SpottetMarker").gameObject.SetActive(false);
        }
        if(checkScanTimer()){
            lastScan = Time.time;
            createGrid();
            dijkstraRes = dijkstra();
            pathToFollow = findPath(dijkstraRes, roomGrid.GetVertices()[1]);
            StartCoroutine(followPath(pathToFollow));
        }
    }

    void returnToIdle(){
        transform.Find("SpottetMarker").gameObject.SetActive(false);
        detectionRangeMod = 1;   
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        //walk on spikes
        if (triggerCollider.tag == "Spike" && enemyAlive){
            takeDamage(10);
        } else if(triggerCollider.tag == "Player Sword" && enemyAlive){
            takeDamage(25);
        }
    }

    public void takeDamage( int damage){
        float timeStamp = Time.time;
        if(timeStamp - lastDamageTime>1){

            health-=damage;
            lastDamageTime = timeStamp;
        }

        
    }

    void killEnemy(){
        enemyAlive = false;
        Destroy(transform.GetComponent<Rigidbody>());
        Destroy(transform.GetComponent<BoxCollider>());
        Instantiate(coin, transform);
    }

    
    //draws Gizmos (3d objects that can only be seen in the editor and is not displayed on player camera)
    private void OnDrawGizmos(){
        
        if(pathToFollow != null){
            Gizmos.color = Color.yellow;
            Vector3 shift = new Vector3(0,5,0);
            for(var i =0; i<pathToFollow.Count; i++){
                Gizmos.DrawSphere(pathToFollow[i] + shift, .3f);
                if(i < pathToFollow.Count - 1){
                    Gizmos.DrawLine(pathToFollow[i] + shift, pathToFollow[1+i] + shift);
                }
            }
        }/*
        if(dijkstraRes !=null){
            Gizmos.color = Color.green;

            Vector3 shift = new Vector3(0,2,0);
            foreach (Vertex v in dijkstraRes.Keys){
                if(v.outEdges.Count < 0){
                    Gizmos.color = Color.red;
                }else{
                    Gizmos.color = Color.green;
                }
                Gizmos.DrawSphere(v.pos + shift, .3f);
                if(dijkstraRes[v]!=null){
                    Gizmos.DrawLine(v.pos + shift,dijkstraRes[v].pos + shift);
                }
            }
        }
        if(roomGrid != null){
            Gizmos.color = Color.white;
            foreach (Vertex v in roomGrid.GetVertices()){
                Gizmos.DrawSphere(v.pos, .3f);
                foreach (Edge e in v.outEdges){
                    Gizmos.DrawLine(e.from.pos,e.to.pos);
                }
            }
        }*/
        
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
                if(input[temp]==null){
                    temp = startVertex;
                    res = new List<Vector3>();
                    break;
                }else{
                    res.Add(temp.pos);
                    temp = input[temp];
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
        List<Pair> vertexPairs = new List<Pair>();

        foreach (Vertex v in roomGrid.GetVertices()){
            d[v] = 100.0f;
            p[v] = null;
            Pair newPair = new Pair(v, d[v]);
            vertexPairs.Add(newPair);
            q.insert(newPair);
        }
        d[roomGrid.GetVertices()[0]] = 0.0f;
        Pair pair = vertexPairs[0];
        pair.d = d[roomGrid.GetVertices()[0]];
        var pos = q.getPosition(pair);
        q.decreaseKey(pos);
        while (!q.isEmpty()){
            Pair u = q.extractMin();
            print("pair" + u.d);
            foreach (Edge e in u.v.outEdges){
                float alt = d[u.v] + e.weight;
                if (alt < d[e.to]){
                    d[e.to] = alt;
                    p[e.to] = u.v;
                     pair = vertexPairs[e.to.index];
                    pair.d = d[e.to];
                    pos = q.getPosition(pair);
                    q.decreaseKey(pos);
                }
            }
        }
        return p;
    }
}