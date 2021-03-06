using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to store the necesasary data of a player object for later loading of its state.
/// </summary>
[System.Serializable]
public class PlayerData 
{
    //public string CurrentScene { get; set; }
    public float[] PlayerPosition { get; set; }
    public int PlayerLives { get; set; }
    public string SceneName { get; set; }

    public PlayerData(Controller player)
    {
        PlayerPosition = new float[3];
        PlayerPosition[0] = player.transform.position.x;
        PlayerPosition[1] = player.transform.position.y;
        PlayerPosition[2] = player.transform.position.z;
        PlayerLives = player.Lives;
        SceneName = SceneManager.GetActiveScene().name;
    }
}
