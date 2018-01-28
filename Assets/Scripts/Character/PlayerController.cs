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

	public static float SPEED_REGULAR = 12.0f;
	public static float SPEED_WITH_GRAB = 6.0f;

	public float speed;
	public float mouseSensitivity;

	public bool isGrabbing;

	private Rigidbody rb;


	void Start ()
	{
		speed = SPEED_REGULAR;
		mouseSensitivity = 30.0f;
		isGrabbing = false;
		rb = GetComponent<Rigidbody>();
	}


	//TODO Grab function
	private void Grab()
	{
		
	}

	//TODO Release function
	private void Release()
	{
		
	}

	void FixedUpdate ()
	{
		if (Input.GetKey (KeyCode.E)) 
		{
			if (isGrabbing)
			{
				Release ();
			}
			else
			{
				Grab ();
			}

			isGrabbing = !isGrabbing;
		}
		
		AnimateCharacter ();
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
			speed = SPEED_WITH_GRAB;
		} 
		else
		{
			speed = SPEED_REGULAR;
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


	private void AnimateCharacter()
	{
		// when nothing happens, idle
		if (!Input.anyKeyDown) 
		{
			GetComponent<Animator> ().SetTrigger ("Idle");
		}
		// when E input pressed
		if(Input.GetKey(KeyCode.E)) 
		{
			if (isGrabbing) {
				// TODO Release animation play
			} else {
				//TODO GRAB animation play
			}
		} 

		// When moving key input pressed
		if(Input.GetKey(KeyCode.W)
			|| Input.GetKey(KeyCode.UpArrow)
			|| Input.GetKey(KeyCode.A)
			|| Input.GetKey(KeyCode.LeftArrow)
			|| Input.GetKey(KeyCode.S)
			|| Input.GetKey(KeyCode.DownArrow)
			|| Input.GetKey(KeyCode.D)
			|| Input.GetKey(KeyCode.RightArrow))
		{
			if (isGrabbing)
			{
				if (Input.GetKey (KeyCode.W)
					|| Input.GetKey(KeyCode.UpArrow))
				{
					GetComponent<Animator> ().SetTrigger ("Push");	
				}
				//TODO pulling
				else if (Input.GetKey (KeyCode.S)
					|| Input.GetKey(KeyCode.DownArrow))
				{
//					GetComponent<Animator> ().SetTrigger ("Pull");
				}
			} 
			else
			{
				GetComponent<Animator> ().SetTrigger ("Walk");
			}
		}

	}

	
}