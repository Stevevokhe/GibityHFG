using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorTeleportFadeAnimationEventController : MonoBehaviour
{
    public event EventHandler FadeInEnded;
    public event EventHandler FadeOutEnded;

    public void FadeInEndedAnimationEvent()
    {
        FadeInEnded?.Invoke(this, EventArgs.Empty);
    }

    public void FadeOutEndedAnimationEvent()
    {
        FadeOutEnded?.Invoke(this, EventArgs.Empty);
    }
}