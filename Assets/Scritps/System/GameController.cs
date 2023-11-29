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

    public event EventHandler<int> PlayerCaught;
    public event EventHandler WonGame;
    public event EventHandler LostGame;
    public event EventHandler PlayerGotOlder;

    public float GoalPoint => goalPoint;

    private void Awake()
    {
        if (player == null)
            throw new System.Exception($"{name}: {nameof(player)} can't be null.");

        if (startPoint == null)
            throw new System.Exception($"{name}: {nameof(startPoint)} can't be null.");

        if (gameMusic == null)
            throw new System.Exception($"{name}: {nameof(gameMusic)} can't be null.");

        player.transform.position = startPoint.position;
        player.Caught += CatchedPlayer;
        player.ChangedPoints += ChangedPoint;
        player.Died += (s,e) => LostGame?.Invoke(this, EventArgs.Empty);
        player.GotOlder += (s, e) => PlayerGotOlder?.Invoke(this, EventArgs.Empty);

        gameMusic.volume *= SavingManager.Instance.GetMasterVolume(1) * SavingManager.Instance.GetMusicVolume(1);
    }

    private void CatchedPlayer(object sender, int e)
    {
        player.transform.position = startPoint.position;
        PlayerCaught?.Invoke(this, e);
    }

    private void ChangedPoint(object sender, int points)
    {
        if(points >= goalPoint)
        {
            WonGame?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TeleportPlayer(Transform transform)
    {
        player.transform.position = transform.position;
    }
}
