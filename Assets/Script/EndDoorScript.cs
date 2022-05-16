using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndDoorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //change scene to main menu 
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
