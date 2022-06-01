using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject[] endPoints;
    private int currentTargetIndex = 0;
    [SerializeField] 
    private float speed = 2f;
    void Update()
    {
        if (Vector2.Distance(endPoints[currentTargetIndex].transform.position, transform.position) < 0.1f)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= endPoints.Length)
            {
                currentTargetIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, endPoints[currentTargetIndex].transform.position, Time.deltaTime * speed);
    }
}
