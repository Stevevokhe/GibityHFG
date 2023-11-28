using System;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    public event EventHandler Step;

    public void CallStepEvent()
    {
        Step?.Invoke(this, EventArgs.Empty);
    }
}
