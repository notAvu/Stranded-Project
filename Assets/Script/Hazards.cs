using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    private GrappleGun grapple;

    [SerializeField]
    private int damage = 5;
    void Start()
    {
        grapple = FindObjectOfType<GrappleGun>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HurtPlayer(collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        HurtPlayer(collision);
    }

    private void HurtPlayer(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Controller>().RecieveDamage(damage);
            grapple.Disbable();
        }
    }

    /// <summary>
    /// Deals damage to the player and disables the player's active rope
    /// </summary>
    /// <param name="target"></param>
    private void DealBaseDamage(Collider2D target)
    {
        
    }

}
