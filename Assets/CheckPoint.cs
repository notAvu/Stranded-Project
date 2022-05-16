using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SavePlayer();
            Debug.Log("Save Triggered");
        }
    }
}
