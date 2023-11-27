using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private float goalPoint = 4;
    [SerializeField]
    private Transform startPoint;
    [SerializeField]
    private AudioSource gameMusic;

    public event EventHandler PlayerCaught;

    private void Awake()
    {
        if (player == null)
            throw new System.Exception($"{name}: {nameof(player)} can't be null.");

        if (startPoint == null)
            throw new System.Exception($"{name}: {nameof(startPoint)} can't be null.");

        if (gameMusic == null)
            throw new System.Exception($"{name}: {nameof(gameMusic)} can't be null.");

        player.transform.position = startPoint.position;
        player.Caught += TeleportPlayerToStartPoint;

        gameMusic.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetMusicVolume(1);
    }

    private void TeleportPlayerToStartPoint(object sender, EventArgs e)
    {
        player.transform.position = startPoint.position;
        PlayerCaught?.Invoke(this, EventArgs.Empty);
    }
}
