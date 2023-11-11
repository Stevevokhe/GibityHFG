using System;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TextMeshProUGUI displayText;

    private const string pointText = "Points: ";

    private void Awake()
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }
        player.ChangedPoints += UpdatePoints;
    }

    private void Start()
    {
        displayText.text = pointText + player.Points;
    }

    private void UpdatePoints(object sender, int newPoints)
    {
        displayText.text = pointText + newPoints;
    }
}
