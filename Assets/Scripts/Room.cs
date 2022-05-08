using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Room : MonoBehaviour{
    GameObject[,] roomTiles = new GameObject[7,7];

    public GameObject emptyTile;
    public GameObject spikeTile;
    public GameObject enemyTile;
    public GameObject treasureTile;
    public GameObject wallTile;
    public GameObject cornerTile;
    public GameObject door;
    public GameObject hallway;

    public int posX;
    public int posZ;
    public int roomSize = 7;
    public int longestEdge = 5;

    AdjacencyGraph roomGrid;
    
    //constructor ... kindof XD
    public void setRoom(int posX, int posZ, int roomSize){
        this.posX=posX;
        this.posZ=posZ;
        this.roomSize=roomSize;
        for (var i = 0; i < roomSize; i++){
            for( var j = 0; j< roomSize; j++){
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j] = cornerTile;
                }else if(i == 0 || i == roomSize -1 || j== roomSize - 1 || j == 0){
                    roomTiles[i,j] = wallTile;
                }else{
                    roomTiles[i,j] = pickTile();
                }
            }
        }
        
    }

    public void createRoomGrid(){
        roomGrid = new AdjacencyGraph();
        List<Transform> waypoints = getWaypointsForRoom();
        foreach (Transform waypoint in waypoints){
            Vertex v = new Vertex(waypoint.position);
            roomGrid.addVertex(v);
            for (int i = 0; i < waypoints.Count; i++){
                if (dist(waypoint.position, waypoints[i].position) < longestEdge && (v.getEdgeList().Count != 8)){
                    roomGrid.addEdge(v, new Vertex(waypoints[i].position), (float)dist(waypoint.position, waypoints[i].position));
                }
            }
        }
    }

    public double dist(Vector3 a, Vector3 b){
        return Math.Pow(Math.Pow((b.x - a.x),2) + Math.Pow((b.y - a.y),2) + Math.Pow((b.z - a.z),2), (float).5f); // https://www.engineeringtoolbox.com/distance-relationship-between-two-points-d_1854.html
    }

    // Method to return all the waypoints from a list of tiles
    public List<Transform> getWaypointsForRoom(){
        List<Transform> l = new List<Transform>();
        foreach (GameObject tile in roomTiles){
            for (var i = 0; i < tile.transform.childCount; i++){
                if (tile.transform.GetChild(i).gameObject.tag == "pathHolder"){
                    for (var j = 0; j < tile.transform.GetChild(i).childCount; j++){
                        if(tile.transform.GetChild(i).transform.GetChild(j).gameObject.tag == "PathFindingWAypoint"){
                            l.Add(tile.transform.GetChild(i).transform.GetChild(j).transform);
                        }
                    }
                }
            }
        }
        return l;
    }

    public GameObject[,] getRoomTiles(){
        return roomTiles;
    }

    GameObject pickTile(){
        int randNum = UnityEngine.Random.Range(0,4);
        switch (randNum){
            case 0:
                return emptyTile;
            case 1:
                return spikeTile;
            case 2:
                return enemyTile;
            case 3:
                return treasureTile;
            default:
                return null;
        }
    }

    //draws Gizmos (3d objects that can only be seen in the editor and is not displayed on player camera)
    void OnDrawGizmos(){
        // draw adjacency tree for the enemies
        foreach (Vertex v in roomGrid.GetVertices()){
            Gizmos.DrawSphere(v.getPos(),.3f);
            foreach (Edge e in v.getEdgeList()){
                Gizmos.DrawLine(e.from.getPos(), e.to.getPos());
            }
        }
    }

    public int indexToUnitPos(int i){
        return (5*5+15)*i;
    }



    public void createDoor(int dir){
        if (dir == 2){
            roomTiles[3,0] = door;
            
        } else if(dir == 3){
            roomTiles[6,3] = door;
            
        } else if(dir == 0){
            roomTiles[3,6] = door;
            
        } else {
            roomTiles[0,3] = door;
        }
    }

    public void showRoom(){        
        for(int i = 0; i < roomSize; i++){
            for(int j = 0; j< roomSize; j++){
                var position = new Vector3(i*5 + indexToUnitPos(posX), 0, j*5 + indexToUnitPos(posZ));
                if(i == 0 && j == 0 || i == 0 && j == roomSize-1 || i == roomSize-1 && j == 0 || i == roomSize-1 && j == roomSize-1){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }else if(i == 0 || i == roomSize -1){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 90));
                }else if ( j== roomSize - 1 || j == 0){
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.Euler(Vector3.down * 0));
                }else{
                    roomTiles[i,j]=(GameObject)Instantiate(roomTiles[i,j], position, Quaternion.identity);
                }
            }
        }
    }

    
}

