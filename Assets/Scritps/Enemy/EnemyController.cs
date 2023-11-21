using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private List<EnemyPoint> points;
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private Vector2 closeToTarget = new(0.1f, 2f);
    [SerializeField]
    private int prisonTime = 5; 

    private int targetIndex;
    private bool canMove;
    private int modifier;

    public int PrisonTime => prisonTime;

    private void Awake()
    {
        if (points.Count < 2)
        {
            throw new Exception("Points list must have at least 2 elements");
        }

        transform.position = points[0].Transform.position;
        targetIndex = 1;
        canMove = true;
        modifier = 1;
    }

    private IEnumerator WaitForTime(float stopTime)
    {
        canMove = false;

        yield return new WaitForSeconds(stopTime);

        canMove = true;

        if (targetIndex == points.Count - 1)
        {
            modifier = -1;
        }
        else if (targetIndex == 0)
        {
            modifier = 1;
        }

        targetIndex += modifier;
    }

    private void Update()
    {
        if (!canMove)
        {
            return;
        }

        var targetVector = points[targetIndex].Transform.position;
        targetVector.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetVector, speed * Time.deltaTime);

        var targetPosition = points[targetIndex].Transform.position;
        if (Mathf.Abs(transform.position.x - targetPosition.x) < closeToTarget.x
            && Mathf.Abs(transform.position.y - targetPosition.y) < closeToTarget.y)
        {
            StartCoroutine(WaitForTime(points[targetIndex].StopTime));
        }
    }
}
