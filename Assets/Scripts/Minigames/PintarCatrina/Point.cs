using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Point : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;

    [SerializeField] private PintarCatrina gameHandler;

	public bool detected = false;

	Vector3 touchPosWorld;

	//Change me to change the touch phase used.
	TouchPhase touchPhase = TouchPhase.Ended;

     void Update()
     {

     }
}
