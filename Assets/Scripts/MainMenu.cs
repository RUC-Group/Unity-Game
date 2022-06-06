using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    //Load next scene (which would be play-scene)
    public void playGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    //Shut down application
    public void quitGame(){
        Debug.Log("QUIT");
        Application.Quit();
    }

    //Load main menu scene, which is sceneBuildIndex 0
    public void mainMenu(){
        SceneManager.LoadScene(0);
    }
}
