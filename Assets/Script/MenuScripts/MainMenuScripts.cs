using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
    //method that loads a game scene given its name
    public void LoadScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);
        SceneManager.LoadScene(scene);
    }
    //method that quits the game 
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
