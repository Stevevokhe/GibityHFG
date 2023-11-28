using System;
using TMPro;
using UnityEngine;

public class PrisonPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI displayText;

    private void Awake()
    {
        if (displayText == null)
        {
            throw new ArgumentNullException(nameof(displayText));
        }
    }

    public void Show(int lostYears)
    {
        displayText.text = $"You lost {lostYears} years of your life. You don't want to fail in front of the scale, do you?";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
