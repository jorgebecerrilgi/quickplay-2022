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
    [SerializeField] private float[] alarms = new float[0];
    [SerializeField] private UnityEvent onTimerEnd = null;
    [SerializeField] private UnityEvent onAlert = null;

    public Timer Timer { get; private set; }

    void Start()
    {
        Timer = new Timer(duration, alarms);

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
        Destroy(this);
    }

    void Update()
    {
        Timer.Tick(Time.deltaTime);
    }
}
