using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Timer : MonoBehaviour
{
    [Header("Temporary")]
    [SerializeField]
    string timerId;
    [SerializeField]
    [Tooltip("Second(s)")]
    float tickTime;
    [SerializeField]
    bool repeat;
    [SerializeField]
    bool isInited;
    [SerializeField]
    bool isRunning;
    [SerializeField]
    TimerMode timerMode;

    /// <summary>
    /// When the timer is over, it will call this event.
    /// </summary>
    public event EventHandler Tick;

    /// <summary>
    /// When the timer is started, this will set the timeScale which used when the timer is running.
    /// </summary>
    public enum TimerMode
    {
        GameTime,
        RealTime
    };

    public float TickTime
    {
        get => tickTime;
        set => tickTime = 
            value < 0 
                ? throw new Exception("TickTime can't be smaller than 0.") 
                : value;
    }

    public bool IsRunning => isRunning;

    const string containerName = "Timers";

    /// <summary>
    /// </summary>
    /// <param name="gameObject">The timer will be added to this gameObject</param>
    /// <param name="timerId"></param>
    /// <param name="tickTime">Seconds</param>
    /// <param name="repeat"></param>
    /// <param name="timerMode"></param>
    /// <returns></returns>
    public static Timer Create(GameObject gameObject, string timerId, float tickTime = 0, bool repeat = false, TimerMode timerMode = TimerMode.GameTime)
    {
        GameObject timerObject = null;
#if UNITY_EDITOR
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == containerName)
            {
                timerObject = child.gameObject;
                break;
            }
        }
        if (timerObject == null)
        {
            var t = new GameObject
            {
                name = containerName
            };
            t.transform.parent = gameObject.transform;
            timerObject = t;
        }
#else
        timerObject = gameObject;
#endif

        Timer timer = timerObject.AddComponent<Timer>();
        timer.Init(timerId, tickTime, repeat, timerMode);

        return timer;
    }

    private void Init(string timerId, float tickTime, bool repeat = false, TimerMode timerMode = TimerMode.GameTime)
    {
        this.timerId = timerId;
        this.tickTime = tickTime;
        this.repeat = repeat;
        this.timerMode = timerMode;

        isInited = true;
    }

    public void StartRunning()
    {
        if (!isInited)
        {
            throw new Exception("You have to init the timer.");
        }

        if (isRunning)
        {
            return;
        }

        isRunning = true;
        StartCoroutine(nameof(Run));
    }

    IEnumerator Run()
    {
        do
        {
            yield return timerMode == TimerMode.RealTime 
                ? new WaitForSecondsRealtime(tickTime) 
                : new WaitForSeconds(tickTime);

            Tick?.Invoke(this, EventArgs.Empty);
        } while (repeat);

        isRunning = false;
    }

    public void StopRunning()
    {
        if (!isInited)
        {
            throw new Exception("You have to init the timer.");
        }

        if (!isRunning)
        {
            return;
        }

        isRunning = false;

        StopCoroutine(nameof(Run));
    }

    private void OnDestroy() => 
        StopRunning();
}
