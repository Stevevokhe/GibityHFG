using System;
using System.Collections;
using UnityEngine;

public class ArtifactController : Item
{
    [SerializeField]
    private int point = 1;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private GameObject lightObject;

    private Collider2D artifactCollider;

    private void Awake()
    {
        if (audioSource == null)
            throw new Exception($"{name}: the {nameof(audioSource)} can't be null.");
        if (spriteRenderer == null)
            throw new Exception($"{name}: the {nameof(spriteRenderer)} can't be null.");
        if (lightObject == null)
            throw new Exception($"{name}: the {nameof(lightObject)} can't be null.");

        artifactCollider = GetComponent<Collider2D>();

        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
    }

    protected override void TouchPlayer(PlayerController player)
    {
        player.Points += point;
        spriteRenderer.enabled = false;
        artifactCollider.enabled = false;
        lightObject.SetActive(false);
        audioSource.Play();
        StartCoroutine(WaitForDestroy(audioSource.clip.length));
    }

    private IEnumerator WaitForDestroy(float waitingTimeForDestroy)
    {
        yield return new WaitForSeconds(waitingTimeForDestroy);
        GameObject.Destroy(gameObject);
    }
}
