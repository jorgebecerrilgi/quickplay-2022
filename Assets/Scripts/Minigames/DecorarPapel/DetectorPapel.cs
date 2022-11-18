using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorPapel : MonoBehaviour
{

    void FixedUpdate()
    {
        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        Collider2D[] otherColliders = Physics2D.OverlapAreaAll(myCollider.bounds.min, myCollider.bounds.max);
 
        foreach (var otherCollider in otherColliders)
        {
        	PointPapel point = otherCollider.gameObject.GetComponent<PointPapel>();
			if (point != null && !point.detected)
            {
            	point.detected = true;
            	Debug.Log(otherCollider.name);
            }
        }
    }
}
