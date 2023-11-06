using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float smoothTime = 0.5f;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        if (player == null)
        {
            return;
        }
        var targetPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z + offset.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
