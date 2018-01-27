using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public static float SPEED = 12.0f;
	public static float SPEED_WITH_GRAB = 6.0f;

	public float speed = 12.0f;
	public float mouseSensitivity = 60.0f;

	public bool isGrabbing = false;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		// TODO Grabbing checing and changing state
		
		Move();
		Rotate ();
	}


	private void Move()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// Future, when its grabed apply the speed with grabing
		if (isGrabbing) {
			speed = SPEED;
		} else {
			speed = SPEED_WITH_GRAB;
		}

		// moving
		Vector3 moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed;
		rb.velocity = moveDirection;

		 
	}

	private void Rotate()
	{
		float mouseX = Input.GetAxis("Mouse X");

		// rotation
		transform.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, mouseSensitivity * mouseX, 0f));
		rb.rotation.SetLookRotation (rb.velocity);
	}

}