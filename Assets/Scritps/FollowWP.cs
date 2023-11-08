using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    private Vector3 targetPosition;
    private bool movingToB = true;

    void Start()
    {
        targetPosition = pointB.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            if (movingToB)
            {
                targetPosition = pointA.position;
                movingToB = false;
            }
            else
            {
                targetPosition = pointB.position;
                movingToB = true;
            }
        }
    }
}