using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    [SerializeField] 
    [Header("Main Menu Scene")]
    public string MAIN_MENU_NAME = "MainMenu";
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    /// <summary>
    /// This method closes the pause menu and resumes the game
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    /// <summary>
    /// This method pauses the game and opens the pause menu
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    /// <summary>
    /// This method leads the player to the main menu scene
    /// </summary>
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(MAIN_MENU_NAME);
    }
    /// <summary>
    /// This method quits the game. Did this actually need an explaination?
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
