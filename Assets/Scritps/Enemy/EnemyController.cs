using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField]
    private Animator animator;

    private int targetIndex;
    private bool canMove;
    private int modifier;

    public int PrisonTime => prisonTime;

    private const string SpeedId = "Speed";

    private void Awake()
    {
        if (points.Count < 2)
        {
            throw new Exception("Points list must have at least 2 elements");
        }

        if (animator == null)
            throw new Exception($"{name}: the {nameof(animator)} can't be null.");

        transform.position = points[0].Transform.position;
        targetIndex = 1;
        canMove = true;
        modifier = 1;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            return;
        }

        var targetVector = points[targetIndex].Transform.position;
        targetVector.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, targetVector, speed * Time.fixedDeltaTime);
        animator.SetFloat(SpeedId, 1f);

        var targetPosition = points[targetIndex].Transform.position;
        if (Mathf.Abs(transform.position.x - targetPosition.x) < closeToTarget.x
            && Mathf.Abs(transform.position.y - targetPosition.y) < closeToTarget.y)
        {
            animator.SetFloat(SpeedId, 0f);
            StartCoroutine(WaitForTime(points[targetIndex].StopTime));
        }
    }

    private void LateUpdate()
    {
        FixAnimation();
    }

    private void FixAnimation()
    {
        var targetVector = points[targetIndex].Transform.position;
        if (
            //The enemy watch left side, but it move to right side.
            (transform.localScale.x < 0 && transform.position.x < targetVector.x)
            //The enemy watch right side, but it move to left side.
            || (transform.localScale.x > 0 && transform.position.x > targetVector.x))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
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
}
