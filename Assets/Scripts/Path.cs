using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour{
    Floor floor;
    List<Transform> waypoints;

    
    // Start is called before the first frame update
    void Start(){
        floor = GameObject.FindGameObjectWithTag("floor").GetComponent<Floor>();
        waypoints = getAllWaypoints(getTiles());
        
    }

    // Update is called once per frame
    void Update(){
        
    }
    // Method to return all the waypoints from a list of tiles
    List<Transform> getAllWaypoints(List<GameObject> tiles){
        waypoints = new List<Transform>();
        foreach (GameObject tile in tiles){
            for (var i = 0; i < tile.transform.childCount+1; i++){
                if(tile.transform.GetChild(i).tag == "pathFindingWAypoint"){
                    waypoints.Add(tile.transform.GetChild(i));
                }
            }
        }
        print(waypoints.Count + "waypoints in the map");
        return waypoints;
    }

    //method that returns a list of all tiles in the game
    public List<GameObject> getTiles(){
         print("after Abe Abe"); 
        List<GameObject> allTiles = new List<GameObject>();
        List<Room> spawnedRooms = floor.getSpawnedRooms();
        print("after Abe Abe2");  
        print("Abe Abe" + spawnedRooms.Count);  
        foreach (Room r in spawnedRooms){
            GameObject[,] roomTiles = r.getRoomTiles();
            for (var i = 0; i < spawnedRooms.Count; i++){
                for (var j = 0; j < spawnedRooms.Count; j++){
                    allTiles.Add(roomTiles[i,j]);
                }
            }
        }
        return allTiles;
    }
}
