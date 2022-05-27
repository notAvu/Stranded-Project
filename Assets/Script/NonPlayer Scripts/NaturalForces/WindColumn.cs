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
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalForce, verticalForce));

    }
}