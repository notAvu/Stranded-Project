using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/// <summary>
/// this class it's just used to load the next scene when the player reaches the door
/// </summary>
public class EndDoorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = Vector3.zero;
            collision.gameObject.GetComponent<Controller>().SaveState();
            if (SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            else
            { 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            }
        }
    }
}
