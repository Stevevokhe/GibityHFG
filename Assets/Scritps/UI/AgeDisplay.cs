using UnityEngine;
using System;
using TMPro;

public class AgeDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerAgeController playerAgeController;
    [SerializeField]
    private TextMeshProUGUI displayText;

    private void Awake()
    {
        if(playerAgeController == null)
        {
            throw new ArgumentNullException(nameof(playerAgeController));
        }
        playerAgeController.AgeChanged += AgeChanged;
    }

    private void Start() => 
        displayText.text = $"Age: {playerAgeController.MinAge}";

    private void AgeChanged(object sender, int e) => 
        displayText.text = $"Age: {e}";
}
