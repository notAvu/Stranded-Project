using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : MonoBehaviour
{
    [SerializeField]
    private GrappleGun grapple;

    [SerializeField]
    public int Damage { get; set; } = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.gameObject.GetComponent<Controller>().GetHurt(Damage);
            grapple.Disbable();
        }
    }
}
