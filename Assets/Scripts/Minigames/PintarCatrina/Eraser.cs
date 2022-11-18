using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{

    [SerializeField] private PintarCatrina gameHandler;

    void FixedUpdate()
    {
        CapsuleCollider2D myCollider = GetComponent<CapsuleCollider2D>();
        Collider2D[] otherColliders = Physics2D.OverlapAreaAll(myCollider.bounds.min, myCollider.bounds.max);
 
        foreach (var otherCollider in otherColliders)
        {
        	Point point = otherCollider.gameObject.GetComponent<Point>();
			if (point != null && !point.detected)
            {
            	point.detected = true;
                gameHandler.TryPassed();
            }
        }
    }
}
