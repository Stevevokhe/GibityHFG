using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Item : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tag.Player) &&
            collision.TryGetComponent<PlayerController>(out var player))
        {
            TouchPlayer(player);
        }

    }

    protected abstract void TouchPlayer(PlayerController player);
}
