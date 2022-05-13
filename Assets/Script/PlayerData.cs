using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float[] playerPosition;
    public float playerLives;

    public PlayerData(Controller player)
    {
        // fill player data
        playerPosition = new float[3];
        playerPosition[0] = player.transform.position.x;
        playerPosition[1] = player.transform.position.y;
        playerPosition[2] = player.transform.position.z;
        playerLives = player.Lives;
    }
}
