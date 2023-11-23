using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float smoothTime = 0.5f;
    [Header("Boundary")]
    [SerializeField]
    private bool useBoundary = false;
    [SerializeField]
    private Vector2 minBoundary = Vector2.zero;
    [SerializeField]
    private Vector2 maxBoundary = Vector2.zero;

    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }
        var targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z + offset.z);
        
        var newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime * Time.fixedDeltaTime);
        if(useBoundary)
        {
            if(newPosition.x < minBoundary.x)
                newPosition.x = minBoundary.x;
            else if(newPosition.x > maxBoundary.x)
                newPosition.x = maxBoundary.x;

            if (newPosition.y < minBoundary.y)
                newPosition.y = minBoundary.y;
            else if (newPosition.y > maxBoundary.y)
                newPosition.y = maxBoundary.y;
        }
        transform.position = newPosition;
    }
}
