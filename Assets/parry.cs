using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy the object if tag is enemy or enemybullet
        if (collision.gameObject.tag == "Enemies")
        {
            Destroy(collision.gameObject);
        }
    }
}
