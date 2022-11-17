using System;
using TMPro;
using UnityEngine;

public class QuinceaneraQueue : MinigameMB
{
    const int MINIMUM_DISTANCE_FROM_CHAMBELAN = 2;

    [SerializeField] private Sprite[] queueSprites;
    [SerializeField] private GameObject queue;
    [SerializeField] private TextMeshProUGUI timerUI;

    private SwipeListener swipeListener;
    private SpriteRenderer queueSpriteRenderer;
    private TimerBehaviour timer;
    private int queueLength;
    private int queuePosition;

    private void Awake()
    {
        // Get components.
        swipeListener = GetComponent<SwipeListener>();
        queueSpriteRenderer = queue.GetComponent<SpriteRenderer>();
        timer = GetComponent<TimerBehaviour>();

        queueLength = queueSprites.Length;
        // Pick a random position in the queue, and change the queue sprite accordingly.
        queuePosition = UnityEngine.Random.Range(0, queueLength - MINIMUM_DISTANCE_FROM_CHAMBELAN - 1);
        UpdateQueuePosition(queuePosition);
    }

    private void OnEnable()
    {
        swipeListener.OnSwipe += SwipeListenerOnSwipe;
    }

    private void OnDisable()
    {
        swipeListener.OnSwipe -= SwipeListenerOnSwipe;
    }

    private void SwipeListenerOnSwipe(SwipeListener.SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeListener.SwipeDirection.Left:
                ChooseQueue(queuePosition);
                break;
            case SwipeListener.SwipeDirection.Right:
                UpdateQueuePosition(++queuePosition);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        timerUI.text = (Mathf.CeilToInt(timer.Timer.RemainingSeconds)).ToString();
    }

    private void UpdateQueuePosition(int newPosition)
    {
        if (newPosition >= queueLength || newPosition < 0) return;
        queueSpriteRenderer.sprite = queueSprites[newPosition];
    }

    private void ChooseQueue(int position)
    {
        /*
        if (position == queueLength - 1)
            return;
        else
            return;
        */
        InvokeOnEnd(position == queueLength - 1, timer.Timer.RemainingSeconds);
    }

    public override void BeginMinigame(float remainingSeconds)
    {
        timer.BeginTimer();
    }
}
