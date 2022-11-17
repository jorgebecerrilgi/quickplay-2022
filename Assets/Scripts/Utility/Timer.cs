using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A C# implementation of a timer.
/// </summary>
/// <remarks>
/// Provides a timer that emits an event after it ends.
/// Also provides alerts (that is, a time within the timer's duration) that also emit an event.
/// </remarks>
public class Timer
{
    public float RemainingSeconds { get; private set; }
    private float[] alarms;
    private int alarmsAmount;
    private int currentAlarm = 0;
    private bool isPaused = true;
    public event Action OnTimerEnd;
    public event Action OnAlert;

    /// <summary>
    /// Constructor for a timer of specific duration, with any alarms.
    /// </summary>
    /// <param name="duration">
    /// Duration of the timer in seconds.
    /// </param>
    /// <param name="alarms">
    /// An array of alarms, in seconds, that will each emit an event when they're reached.
    /// Alerts must lie within the timer's duration, and be in ascending order.
    /// </param>
    public Timer(float duration, float[] alarms, bool autostart)
    {
        RemainingSeconds = duration;
        this.alarms = alarms;
        alarmsAmount = alarms.Length;
        isPaused = !autostart;
    }

    public void Start()
    {
        isPaused = false;
    }

    public void Start(float newDuration)
    {
        RemainingSeconds = newDuration;
        Start();
    }

    public void Tick(float delta)
    {
        if (isPaused || RemainingSeconds <= 0f) return;
        RemainingSeconds -= delta;

        CheckForTimerEnd();
    }

    private void CheckForTimerEnd()
    {
        if (currentAlarm < alarmsAmount && RemainingSeconds < alarms[currentAlarm])
        {
            currentAlarm++;
            OnAlert?.Invoke();
        }
        if (RemainingSeconds > 0f) return;
        RemainingSeconds = 0f;
        OnTimerEnd?.Invoke();
    }
}
