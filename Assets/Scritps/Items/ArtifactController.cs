using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class ArtifactController : Item
{
    [SerializeField]
    private int point = 1;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D artifactCollider;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        artifactCollider = GetComponent<Collider2D>();

        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
    }

    protected override void TouchPlayer(PlayerController player)
    {
        player.Points += point;
        spriteRenderer.enabled = false;
        artifactCollider.enabled = false;
        audioSource.Play();
        StartCoroutine(WaitForDestroy(audioSource.clip.length));
    }

    private IEnumerator WaitForDestroy(float waitingTimeForDestroy)
    {
        yield return new WaitForSeconds(waitingTimeForDestroy);
        GameObject.Destroy(gameObject.transform.parent.gameObject);
    }
}
