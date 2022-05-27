using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/// <summary>
/// this class it's just used to load the next scene when the player reaches the door
/// </summary>
public class EndDoorScript : MonoBehaviour
{
    [SerializeField]
    private string nextScene = "MainMenu";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }
}
