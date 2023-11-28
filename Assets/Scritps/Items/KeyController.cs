using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

[RequireComponent(typeof(AudioSource))]
public class KeyController : Item
{
    [SerializeField]
    private Key key;

    private AudioSource audioSource;

    private void Awake()
    {
        if (key == null)
            throw new System.Exception(name + ": key is missing.");

        audioSource = GetComponent<AudioSource>();
        audioSource.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetSFXVolume(1);
    }

    protected override void TouchPlayer(PlayerController player)
    {
        player.AddKey(key);
        audioSource.Play();
        StartCoroutine(WaitForSound());
    }

    IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        GameObject.Destroy(gameObject);
    }
}
