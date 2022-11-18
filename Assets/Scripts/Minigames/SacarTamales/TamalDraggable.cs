using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

/// <summary>
/// An implementation for a draggable object behaviour.
/// </summary>
/// <remarks>
/// Allows a GameObject to be dragged by touch input, with certain limitations.
/// The draggable's area is a circle, and will anchor to the touch's position, regardless from where the contact was made.
/// Emits an event for when the dragging ends, and provides it's last position as an argument.
/// </remarks>
public class TamalDraggable : MonoBehaviour
{
    /// <summary>
    /// Defines the circular radius of the grabbing area.
    /// </summary>
    [SerializeField] private float grabRadius = 1f;
    /// <summary>
    /// The scene's camera, to convert from camera position to world position.
    /// </summary>
    [SerializeField] private Camera mainCamera;

    private bool isDragging = false;

    public delegate void DragEnd(Vector3 worldPosition);
    public event DragEnd OnDragEnd;

    public bool placed = false;

    private bool enlarging = false;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += TouchOnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += TouchOnFingerUp;
    }

    private void Update()
    {
        if (enlarging && transform.localScale.x < .5)
        {
            transform.localScale = new Vector3((float)(transform.localScale.x + .005F), (float)(transform.localScale.y + .005F), 1.0F);
        }
        else if (!enlarging && transform.localScale.x > .4)
        {
            transform.localScale = new Vector3((float)(transform.localScale.x - .005F), (float)(transform.localScale.y - .005F), 1.0F);
        }
    }

    private void Enlarge()
    {
        enlarging = true;
    }

    private void Reduce()
    {
        enlarging = false;
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
    	if (placed)
    		return;

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);

        if (Vector3.Distance(touchPosition, transform.position) < grabRadius)
        {
            isDragging = true;
            Enlarge();
        }
    }

    private void TouchOnFingerMove(Finger obj)
    {
        if (!isDragging) return;

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        // Sets object position to touch position.
        transform.position = touchPosition;
    }

    private void TouchOnFingerUp(Finger obj)
    {
        if (!isDragging) return;
        isDragging = false;
        Reduce();

        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        OnDragEnd?.Invoke(touchPosition);
    }
}
