using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindColumn : MonoBehaviour
{
    [SerializeField] private float verticalForce;
    [SerializeField] private float horizontalForce;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //apply a force to the player equal to the wind's vertical force
        other.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalForce, verticalForce));
        //other.attachedRigidbody.AddForce(new Vector2(horizontalForce, verticalForce));
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        
    }
}