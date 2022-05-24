using UnityEngine;
 
public class StaticTest : MonoBehaviour {
    public static int x;
    public int y;
 
    public void Start() {
        x = Random.Range(0, 10000);
        y = Random.Range(0, 10000);
 
        print("X is " + x + " and Y is " + y);
    }
}
