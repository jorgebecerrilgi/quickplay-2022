using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Holdable : MonoBehaviour
{
    [SerializeField] private float grabRadius = 10f;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private RellenarPan gameHandler;

    private bool isHolding = false;

    public delegate void HoldEnd(Vector3 worldPosition);
    public event HoldEnd OnHoldEnd;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += TouchOnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += TouchOnFingerUp;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private void FixedUpdate()
    {
    	if (isHolding)
    	{
    		gameHandler.Grow(Time.fixedDeltaTime);
    	}
    }

    private void TouchOnFingerDown(Finger obj)
    {
    	if (isHolding || gameHandler.MinigameEnded) return;

    	isHolding = true;
        Debug.Log("down");
    }

    private void TouchOnFingerMove(Finger obj)
    {
        return;
    }

    private void TouchOnFingerUp(Finger obj)
    {
        if (!isHolding) return;

        isHolding = false;
        
        Debug.Log("up");
    }
}
