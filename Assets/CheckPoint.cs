using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    public Controller player;
    void SavePlayer()
    {
        player.SaveState();
    }

    void LoadPlayer()
    {
        player.LoadState();
    }
    public void LoadScene()
    {
        SavePlayer();
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            SavePlayer();
            Debug.Log("Save Triggered");
        }
    }
}
