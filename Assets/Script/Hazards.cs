using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField]
    private GrappleGun grapple;

    [SerializeField]
    int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealBaseDamage(collision);
            grapple.Disbable();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealBaseDamage(collision.collider);
        }
    }
    private void DealBaseDamage(Collider2D target)
    {
        target.gameObject.GetComponent<Controller>().GetHurt(damage);
    }    

}
