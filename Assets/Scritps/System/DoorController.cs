using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    private AudioSource audioSource;

    private void Awake()
    {
        if (key == null)
            throw new Exception(name + ": key is missing.");
        if (openSprite == null)
            throw new Exception(name + ": openSprite is missing.");

        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider2D = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
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
                audioSource.Play();
            }
        }
    }
}
