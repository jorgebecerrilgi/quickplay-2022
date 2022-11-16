using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class SwipeListener : MonoBehaviour
{
    private enum SwipeType
    {
        FastSwipe,
        SlowSwipe,
    }

    public enum SwipeDirection
    {
        Left,
        Right,
        Up,
        Down,
    }

    private const float SWIPE_DISTANCE_THRESHOLD = 25f;

    [SerializeField] private SwipeType type = SwipeType.FastSwipe;

    private float maxTime;
    private Vector2 startPosition;
    private float timer = 0f;

    public event Action<SwipeDirection> OnSwipe;

    private void Awake()
    {
        switch (type)
        {
            case SwipeType.FastSwipe: maxTime = 0.2f; break;
            case SwipeType.SlowSwipe: maxTime = 0.5f; break;
            default: maxTime = 0.2f; break;
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += TouchOnFingerUp;
    }

    private void OnDisable()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp -= TouchOnFingerUp;
        EnhancedTouchSupport.Disable();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void TouchOnFingerDown(Finger obj)
    {
        startPosition = obj.screenPosition;
        // Restarts the timer.
        timer = 0f;
    }

    private void TouchOnFingerUp(Finger obj)
    {
        // Returns if swipe took too long.
        if (timer > maxTime) return;
        // Returns if swipe barely moved.
        if (Vector2.Distance(startPosition, obj.screenPosition) < SWIPE_DISTANCE_THRESHOLD) return;
        
        Vector2 displacement = obj.screenPosition - startPosition;

        // Invokes a OnSwipe event with its direction.
        if (Math.Abs(displacement.x) > Math.Abs(displacement.y))
        {
            Debug.Log(displacement.x < 0 ? "Left" : "Right");
            OnSwipe?.Invoke(displacement.x < 0 ? SwipeDirection.Left : SwipeDirection.Right);
        }
        else
        {
            Debug.Log(displacement.y < 0 ? "Down" : "Up");
            OnSwipe?.Invoke(displacement.y < 0 ? SwipeDirection.Down : SwipeDirection.Up);
        }
    }
}
