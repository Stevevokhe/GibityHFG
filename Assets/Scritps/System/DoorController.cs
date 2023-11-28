using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorController : MonoBehaviour
{
    [SerializeField]
    private Key key;
    [SerializeField]
    private Sprite openSprite;


    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider2D;

    private void Awake()
    {
        if (key == null)
            throw new System.Exception(name + ": key is missing.");
        if (openSprite == null)
            throw new System.Exception(name + ": openSprite is missing.");

        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider2D = GetComponent<Collider2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag.Player) &&
            collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if(player.HasKey(key))
            {
                spriteRenderer.sprite = openSprite;
                doorCollider2D.enabled = false;
            }
        }
    }
}
