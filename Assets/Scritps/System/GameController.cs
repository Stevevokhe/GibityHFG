using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Transform startPoint;

    private void Awake()
    {
        if (player == null)
            throw new System.Exception($"{name}: player can't be null.");

        if (startPoint == null)
            throw new System.Exception($"{name}: startPoint can't be null.");

        player.transform.position = startPoint.position;
        player.Caught += TeleportPlayerToStartPoint;
    }

    private void TeleportPlayerToStartPoint(object sender, EventArgs e)
    {
        player.transform.position = startPoint.position;
    }
}
