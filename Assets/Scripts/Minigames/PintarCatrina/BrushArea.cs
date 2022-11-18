using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class BrushArea : MonoBehaviour
{

	public GameObject maskPrefab;

    [SerializeField] private float grabRadius = 1f;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject eraser;

    private bool isDragging = false;

    public delegate void DragEnd(Vector3 worldPosition);
    public event DragEnd OnDragEnd;

    private bool allowMasking = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += TouchOnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += TouchOnFingerUp;
    }

    private void FixedUpdate()
    {
    	allowMasking = true;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, transform.position.z));
        worldPosition.z = transform.position.z;
        return worldPosition;
    }

    private void TouchOnFingerDown(Finger obj)
    {
        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);

        if (Vector3.Distance(touchPosition, transform.position) < grabRadius)
            isDragging = true;
    }

    private void TouchOnFingerMove(Finger obj)
    {
        if (!isDragging)
        {
        	return;
        }

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        // Sets object position to touch position.
        if (allowMasking)
        {
        	allowMasking = false;
        	var mask = Instantiate(maskPrefab, touchPosition, Quaternion.identity);
        	mask.transform.position = touchPosition;
        	mask.transform.parent = transform;

            eraser.transform.position = touchPosition;
        }
    }

    private void TouchOnFingerUp(Finger obj)
    {
        if (!isDragging) return;
        isDragging = false;

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        OnDragEnd?.Invoke(touchPosition);
    }
}
