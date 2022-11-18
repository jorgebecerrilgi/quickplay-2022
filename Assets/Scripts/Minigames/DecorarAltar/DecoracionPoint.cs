using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoracionPoint : MonoBehaviour
{

	public bool activated = false;

    public void Activate()
    {
    	activated = true;
    	GetComponent<SpriteRenderer>().enabled = true;
    }
}
