using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeleportTextMeshProUGUI : TextMeshProUGUI
{
    private Transform currentTarget;

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }
}