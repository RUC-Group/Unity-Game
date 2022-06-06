using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour{
    public GameObject coin;
    public float speed = 5;
    public int health = 100;
    public int damage = 10;
    public AudioSource a;

    Dictionary<Vertex,Vertex> dijkstraRes;
    AdjacencyGraph roomGrid;
    Player player;
    List<Transform> waypoints = new List<Transform>();
    List<Vector3> pathToFollow;
    Vector3 playerPos;
    Vector3 velocity;
    Vector3 directionToPlayer;
    Vector3 displacementFromPlayer;

    bool enemyAlive = true;
    bool lastHopUp = false;
    double timeOfDeath;
    float distanceToTarget;
    float detectionRange = 20;
    float lasthop;
    float waitTime = .1f;
    float longestEdge = 3;
    float lastScan = 0;
    float timeStamp;
    float angle;
    float lastDamageTime = 0;
    float lastDamageDealtTime = 0;
    int turnSpeed = 360;

    // set the list of waypoints in the enemy to the list of transforms given, w
    public void setWaypoints(List<Transform> w){
        waypoints.Add(transform);
        waypoints.Add(GameObject.FindGameObjectWithTag("Player").transform);
        foreach (Transform waypoint in w){
            waypoints.Add(waypoint);
        }
    }

    // creates an adjacency graph, where the enemy can walk
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

    // method that checks if 2 seconds have passed from now(timestamp) until a moment (lastScan)
    bool checkScanTimer(){
        timeStamp = Time.time;
        return timeStamp<2 || timeStamp - lastScan>2;
    }

    // Update is called once per frame
    void Update(){
        timeStamp = Time.time;
        //if enemy died more than 2 seconds ago, sink into the ground
        if(timeStamp-timeOfDeath> 2 && !enemyAlive){
            transform.position -= new Vector3(0,.01f,0);
        }
        // if enemy died more than 10 seconds ago, delete enemy
        if(timeStamp-timeOfDeath> 10 && !enemyAlive){
            Destroy(transform.gameObject);
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        // if enemy is dead, turn off spottet marker and lay down
        if(!enemyAlive){
            transform.Find("SpottetMarker").gameObject.SetActive(false);
            Quaternion target = Quaternion.Euler(0,90,90);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        }
        // if enemy is below 0 health but not dead, kill enemy
        else if(health <= 0){
            killEnemy();
        // else if enemy is alive    
        }else{
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position; 
            displacementFromPlayer = playerPos - transform.position;
            directionToPlayer = displacementFromPlayer.normalized;
            velocity = directionToPlayer * speed;
            distanceToTarget = displacementFromPlayer.magnitude;

            // if player is within detectionrange
            if(detectionRange> distanceToTarget){
                Vector3 dirToLookTarget = (player.transform.position - transform.position).normalized;
                float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;
                float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle + 270, turnSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.up * angle;
                //if distance to the player is below 1.5 follow the player 
                if (distanceToTarget>1.5f) {
                    followPlayer();    
                } // else dmg the player 
                else {
                    timeStamp = Time.time;
                    if(timeStamp - lastDamageDealtTime>2 && !player.invincible){
                        player.takeDamage(damage);
                        lastDamageDealtTime = timeStamp;
                    }
                }
            } // else if the player is not within detection distance, return to idle
            else{
                returnToIdle();
            }
        }
        
    }
    // method that activates dijkstra's algorithm and makes the enemy follow the player
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

    // returns to idle
    void returnToIdle(){
        transform.Find("SpottetMarker").gameObject.SetActive(false);
    }

    //hitbox events
    void OnTriggerStay(Collider triggerCollider) {
        //walk on spikes
        if (triggerCollider.tag == "Spike" && enemyAlive){
            takeDamage(10);
        } 
        // collide with the players sword
        if(triggerCollider.tag == "Player Sword" && enemyAlive){
            print("sword");
            if(timeStamp - lasthop>.2f && !lastHopUp){
                transform.position = transform.position + new Vector3(0,.5f,0); 
                lasthop = timeStamp;
                lastHopUp = true;
            }else if(timeStamp - lasthop>.2f && lastHopUp){
                transform.position = transform.position + new Vector3(0,-.5f,0); 
                lasthop = timeStamp;
                lastHopUp = false;
            }
            takeDamage(35);
        }
    }

    // deal damage to the enemy
    public void takeDamage( int damage){
        float timeStamp = Time.time;
        if(timeStamp - lastDamageTime>1){
            health-=damage;
            lastDamageTime = timeStamp;
        }
    }

    // kill enemy
    void killEnemy(){
        enemyAlive = false;
        transform.GetComponent<Rigidbody>().isKinematic=false;
        Destroy(transform.GetComponent<Rigidbody>());
        Destroy(transform.GetComponent<BoxCollider>());
        timeOfDeath = Time.time;
        var position = transform.position + new Vector3(0,1,0);
        Instantiate(coin, position, Quaternion.Euler(Vector3.down * 0));
        
    }

    
    //draws Gizmos (3d objects that can only be seen in the editor and is not displayed on player camera)--- feel free to uncomment which ever to see their effect----
    private void OnDrawGizmos(){
        /*
        // shows in a form of gizmos, the path the enemy will follow to the player
        if(pathToFollow != null){
            Gizmos.color = Color.white;
            Vector3 shift = new Vector3(0,5,0);
            for(var i =0; i<pathToFollow.Count; i++){
                Gizmos.DrawSphere(pathToFollow[i] + shift, .3f);
                if(i < pathToFollow.Count - 1){
                    Gizmos.DrawLine(pathToFollow[i] + shift, pathToFollow[1+i] + shift);
                }
            }
        }

        // shows in a form of gizmos, the result dijkstra returns
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

        // shows in a form of gizmos, the adjacency graph
        if(roomGrid != null){
            Gizmos.color = Color.white;
            foreach (Vertex v in roomGrid.GetVertices()){
                Gizmos.DrawSphere(v.pos, .3f);
                foreach (Edge e in v.outEdges){
                    Gizmos.DrawLine(e.from.pos,e.to.pos);
                }
            }
        } */
    }

    // method that follows a path in the form of vector3
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
        
    // takes the output from dijkstra's algorthm as unput and one specific vertes and returns a list of vecter3's 
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
    
    //dijkstras algorithm, takes an adjacency graph and returns the shortest path from the enemy to every point in the graph 
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