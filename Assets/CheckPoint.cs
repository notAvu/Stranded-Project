using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject player;
    void Save()
    {
        //save the player position and store it in a file 
        PlayerPrefs.SetFloat("x", player.transform.position.x);
        PlayerPrefs.SetFloat("y", player.transform.position.y);
        PlayerPrefs.SetFloat("z", player.transform.position.z);

    }

    void Load()
    {
        player.transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));
    }
    //call save function on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Save();
        }
    }
}
