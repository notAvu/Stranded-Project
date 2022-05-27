using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField]
    private GrappleGun grapple;

    [SerializeField]
    private int damage = 5;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealBaseDamage(collision);//Maybe it would be better to have the Controller script execute this logic. TODO Change grappling gun system to have the controller script use this
            grapple.Disbable();

        }
    }
    /// <summary>
    /// Deals damage to the player and disables the player's active rope
    /// </summary>
    /// <param name="target"></param>
    private void DealBaseDamage(Collider2D target)
    {
        target.gameObject.GetComponent<Controller>().GetHurt(damage);
    }

}
