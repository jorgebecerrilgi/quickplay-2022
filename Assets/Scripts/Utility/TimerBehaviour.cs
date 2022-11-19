using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A MonoBehaviour Component implementation of a timer.
/// </summary>
/// <remarks>
/// Provides a timer that emits a UnityEvent after it ends.
/// Also provides alerts (that is, a time within the timer's duration) that also emit a UnityEvent.
/// </remarks>
public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool destroyOnEnd = true;
    [SerializeField] private bool autostart = true;
    [SerializeField] private float[] alarms = new float[0];
    [SerializeField] private UnityEvent onTimerEnd = null;
    [SerializeField] private UnityEvent onAlert = null;

    private bool isRunning = false;
    public Timer Timer { get; private set; }

    private void Start()
    {
        Timer = new Timer(duration, alarms, autostart);
        isRunning = autostart;

        Timer.OnTimerEnd += TimerOnTimerEnd;
        Timer.OnAlert += TimerOnAlert;
    }

    private void TimerOnAlert()
    {
        onAlert?.Invoke();
    }

    private void TimerOnTimerEnd()
    {
        onTimerEnd?.Invoke();

        if (destroyOnEnd) Destroy(this);
    }

    void Update()
    {
        if (!isRunning) return;
        Timer.Tick(Time.deltaTime);
    }

    public void BeginTimer()
    {
        Timer.Start();
        isRunning = true;
    }

    public void BeginTimer(float newDuration)
    {
        Timer.Start(newDuration);
        isRunning = true;
    }

    public void RestartTimer()
    {
        Timer.Start(duration);
        isRunning = true;
    }

    public void Stop()
    {
        isRunning = false;
    }
}
