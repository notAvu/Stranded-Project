using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : MonoBehaviour
{
    [SerializeField]
    private GrappleGun grapple;

    [SerializeField]
    private int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealBaseDamage(collision);
            grapple.Disbable();
        }
    }
    
    private void DealBaseDamage(Collider2D target)
    {
        target.gameObject.GetComponent<Controller>().GetHurt(Damage);
    }

}
