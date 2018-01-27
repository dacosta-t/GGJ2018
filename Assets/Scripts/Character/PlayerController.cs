using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player controller.
///  - Controls characters by Rigidbody
///  - Inputs: Mouse horizontal moves(rotates the character),
/// 			WASD(or arrows) for forward, sides, backward 
/// Author: Shawn(Dongwon) Kim
/// </summary>
public class PlayerController : MonoBehaviour {

	public static float SPEED = 12.0f;
	public static float SPEED_WITH_GRAB = 6.0f;

	public float speed = 12.0f;
	public float mouseSensitivity = 30.0f;

	public bool isGrabbing;

	private Rigidbody rb;

	void Start ()
	{
		isGrabbing = false;
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		if(Input.GetKey(KeyCode.W)
		|| Input.GetKey(KeyCode.UpArrow))
		{
			GetComponent<Animator> ().SetTrigger ("Walk");
		}

		if(Input.GetKey(KeyCode.S)
			|| Input.GetKey(KeyCode.DownArrow))
		{
			GetComponent<Animator> ().SetTrigger ("Walk");
		}

		if(Input.GetKey(KeyCode.A)
			|| Input.GetKey(KeyCode.LeftArrow))
		{
			GetComponent<Animator> ().SetTrigger ("Walk");
		}

		if(Input.GetKey(KeyCode.D)
			|| Input.GetKey(KeyCode.RightArrow))
		{
			GetComponent<Animator> ().SetTrigger ("Walk");
		}

		if(Input.GetKey(KeyCode.None))
		{
			GetComponent<Animator> ().SetTrigger ("Idle");
		}

//		if (GetComponent<Rigidbody> ().velocity == Vector3.zero) {
//			Debug.Log ("is idle: Animation needs to be idle");
//			GetComponent<Animator> ().SetTrigger ("Idle");
//		} else {
//			if (isGrabbing) {
//				
//			} else {
//				GetComponent<Animator> ().SetTrigger ("Walk");
//			}
//		}
			

		Move();
		Rotate ();
	}


	private void Move()
	{	
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		// Future, when its grabed apply the speed with grabing
		if (isGrabbing) 
		{
			speed = SPEED;
		} 
		else
		{
			speed = SPEED_WITH_GRAB;
		}

		// moving
		Vector3 moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed;

		rb.velocity = moveDirection;
	}

	private void Rotate()
	{
		float mouseX = Input.GetAxis("Mouse X");

		Vector3 direction = new Vector3 (0f, mouseSensitivity * mouseX, 0f);

		if( direction != Vector3.zero)
		{
			// rotation while moving
			transform.rotation = Quaternion.Euler(rb.rotation.eulerAngles + direction);
		}

		if (rb.velocity != Vector3.zero)
		{
			// rotation while moving mouse only
			rb.rotation.SetLookRotation (rb.velocity);
		}
	}

}