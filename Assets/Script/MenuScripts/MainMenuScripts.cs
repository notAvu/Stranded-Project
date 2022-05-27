using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScripts : MonoBehaviour
{
    [SerializeField]
    private GameObject loadGameButton;
    void Start()
    {
        if (SaveSystem.FileExists)
            loadGameButton.SetActive(true);
    }
    /// <summary>
    /// Loads a scene given its name
    /// </summary>
    /// <param name="scene">The name of the scene you want to load</param>
    public void LoadScene(string scene)
    {
        Debug.Log("Loading scene: " + scene);
        SceneManager.LoadScene(scene);
    }
    /// <summary>
    /// Does this actually need an explaination?
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
    /// <summary>
    /// Calls the save system's method to delete the save file
    /// And then loads a given scene
    /// </summary>
    /// <param name="scene">The name of the scene you want to load</param>
    public void DeleteData(string scene)
    {
        Debug.Log("Deleting data");
        SaveSystem.DeleteFile();
        SceneManager.LoadScene(scene);
    }
}
