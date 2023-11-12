using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Key key;

    private void Awake()
    {
        if (key == null)
            throw new System.Exception(name + ": key is missing.");

        GetComponent<SpriteRenderer>().color = key.ColorValue;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Player) &&
            collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if(player.HasKey(key))
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
