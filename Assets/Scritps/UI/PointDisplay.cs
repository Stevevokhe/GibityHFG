using System;
using TMPro;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private TextMeshProUGUI displayText;

    private const string pointText = "Points";

    private void Awake()
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }
        if (gameController == null)
        {
            throw new ArgumentNullException(nameof(gameController));
        }
        player.ChangedPoints += UpdatePoints;
    }

    private void Start()
    {
        UpdatePoints(this, 0);
    }

    private void UpdatePoints(object sender, int newPoints)
    {
        displayText.text = $"{pointText}: {newPoints}/{gameController.GoalPoint}";
    }
}
