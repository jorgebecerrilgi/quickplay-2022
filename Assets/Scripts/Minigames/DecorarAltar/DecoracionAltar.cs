using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class DecoracionAltar : MonoBehaviour
{
    [SerializeField] private float grabRadius = 1f;
    public Camera mainCamera;

    private bool isDragging = false;

    public delegate void DragEnd(Vector3 worldPosition);
    public event DragEnd OnDragEnd;

    private DecoracionPoint currPoint = null;

    private bool done = false;

    public Vector3 loc;

    private bool enlarging = false;

    [SerializeField] private AudioSource grabSFX;
    [SerializeField] private AudioSource dropSFX;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += TouchOnFingerDown;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerMove += TouchOnFingerMove;
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerUp += TouchOnFingerUp;
    }

    public void Locate(Vector3 _loc)
    {
    	loc = _loc;
    	transform.position = loc;
    }

    private void Update()
    {
        if (enlarging && transform.localScale.x < .3)
        {
            transform.localScale = new Vector3((float)(transform.localScale.x + .005F), (float)(transform.localScale.y + .005F), 1.0F);
        }
        else if (!enlarging && transform.localScale.x > .2)
        {
            transform.localScale = new Vector3((float)(transform.localScale.x - .005F), (float)(transform.localScale.y - .005F), 1.0F);
        }
    }

    private void Enlarge()
    {
        enlarging = true;
        grabSFX.Play();
    }

    private void Reduce()
    {
        enlarging = false;
        dropSFX.Play();
    }

    private void FixedUpdate()
    {
    	if (done)
    		return;

        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        Collider2D[] otherColliders = Physics2D.OverlapAreaAll(myCollider.bounds.min, myCollider.bounds.max);
 
        foreach (var otherCollider in otherColliders)
        {
        	DecoracionPoint point = otherCollider.gameObject.GetComponent<DecoracionPoint>();
			if (point != null)
            {
            	currPoint = point;
            	break;
            }
            else
            {
            	currPoint = null;
            }
        }
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
    	if (done)
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

        if (currPoint == null || !currPoint.activated)
        {
        	transform.position = loc;
        	return;
        }

        transform.position = currPoint.transform.position;

        done = true;
        Vector3 touchPosition = GetWorldPosition(obj.screenPosition);
        OnDragEnd?.Invoke(touchPosition);
    }
}
