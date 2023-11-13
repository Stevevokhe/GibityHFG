using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgeController : MonoBehaviour
{
    [Header("Age")]
    [SerializeField]
    [Min(10)]
    private int minAge = 20;
    [SerializeField]
    [Min(11)]
    private int maxAge = 70;
    [SerializeField]
    [Min(0)]
    private float secondsPerAge = 5f;
    [SerializeField]
    private List<int> modelChangeAgeLimits = new();

    public event EventHandler AgeTimerStarted;
    public event EventHandler AgeTimerEnded; 
    public event EventHandler<int> AgeChanged;
    public event EventHandler<int> RaiseModelChangeAtAge;
    public event EventHandler MaxAgeLimitReached;

    public int MinAge => minAge;

    private int currentAge;
    private Timer timer;

    private void Awake()
    {
        if(minAge >= maxAge)
        {
            throw new Exception("MinAge can't be greater (or equal) with MaxAge");
        }
        timer = Timer.Create(gameObject, nameof(PlayerAgeController), secondsPerAge, true);
        timer.Tick += TimerTick;
        currentAge = minAge;
    }

    private void TimerTick(object sender, EventArgs e)
    {
        currentAge++;
        AgeChanged?.Invoke(this, currentAge);
        if (modelChangeAgeLimits.Contains(currentAge))
        {
            RaiseModelChangeAtAge?.Invoke(this, currentAge);
        }
        if (currentAge == maxAge)
        {
            MaxAgeLimitReached?.Invoke(this, EventArgs.Empty);
            StopTimer();
        }
    }

    public void StartTimer()
    {
        timer.StartRunning();
        AgeTimerStarted?.Invoke(this, EventArgs.Empty);
    }

    public void StopTimer()
    {
        timer.StopRunning();
        AgeTimerEnded?.Invoke(this, EventArgs.Empty);
    }
}
