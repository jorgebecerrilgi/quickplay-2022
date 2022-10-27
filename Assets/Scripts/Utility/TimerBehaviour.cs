using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float[] alarms = new float[0];
    [SerializeField] private UnityEvent onTimerEnd = null;
    [SerializeField] private UnityEvent onAlert = null;

    public Timer Timer { get; private set; }

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        Timer.Tick(Time.deltaTime);
    }
}
