using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    //Load main menu scene, which is sceneBuildIndex 0
    public void returnToMainMenu(){
        SceneManager.LoadScene(0);
    }
}
