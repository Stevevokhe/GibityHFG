using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorTeleportController : MonoBehaviour
{
    [SerializeField]
    private DoorTeleportController targetDoor;

    private void Awake()
    {
        if (targetDoor == null)
            throw new Exception($"{name}: {nameof(targetDoor)} can't be null");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(Tag.Player))
        {
            return;
        }
        GameInterfaceController.Instance.ShowTeleportButton(targetDoor.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(Tag.Player) || GameInterfaceController.IsTeleporting)
        {
            return;
        }
        GameInterfaceController.Instance.HideTeleportButton();
    }
}
