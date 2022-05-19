using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
    [SerializeField]
    private GameObject loadGameButton;
    //make button visible when the scene is loaded
    void Start()
    {
        if (SaveSystem.FileExists())
            loadGameButton.SetActive(true);
    }
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
    //method that deletes saved data and loads a given scene
    public void DeleteData(string scene)
    {
        Debug.Log("Deleting data");
        SaveSystem.DeleteFile();
        SceneManager.LoadScene(scene);
    }
}
