using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class PapelPicado : MonoBehaviour
{
    public Camera mainCamera;

    private bool isDragging = false;

    public delegate void DragEnd(Vector3 worldPosition);
    public event DragEnd OnDragEnd;

    [SerializeField] private GameObject detector;

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

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, transform.position.z));
        worldPosition.z = transform.position.z;
        return worldPosition;
    }

    private void TouchOnFingerDown(Finger obj)
    {
        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        isDragging = true;
        Debug.Log("f down");
    }

    private void TouchOnFingerMove(Finger obj)
    {
        if (!isDragging) return;

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        // Sets object position to touch position.
        detector.transform.position = touchPosition;
    }

    private void TouchOnFingerUp(Finger obj)
    {
        if (!isDragging) return;
        isDragging = false;

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        OnDragEnd?.Invoke(touchPosition);
    }
}
