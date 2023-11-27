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
        displayText.text = $"You lost {lostYears} years in your life. You should be more careful.";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
