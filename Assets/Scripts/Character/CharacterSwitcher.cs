using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character switcher.
///  - Switches(Deactivate/Activate) character to another by F1, F2, F3,
///  - Higlihts a current character,
///  - Attach main camera to the new current character
/// Author: Shawn(Dongwon) Kim
/// </summary>
public class CharacterSwitcher : MonoBehaviour {

	// for higlihting current character
	public Color colorStart = Color.red;
	public Color colorEnd = Color.green;
	public float duration = 1.0F;
	public Renderer rend;

	// references of characters
	GameObject[] characters;

	// current character index
	public int currentCharacterIndex;


	void Awake()
	{
		characters = GameObject.FindGameObjectsWithTag ("Player");
		UpdateCurrentCharacterIndex (GetIndexOfTheCharacter ("Character01"));
	}


	void Start()
	{	
		DeactivateCharacter (GetIndexOfTheCharacter ("Character02"));
		DeactivateCharacter (GetIndexOfTheCharacter ("Character03"));
		SwitchCharacter (GetIndexOfTheCharacter ("Character01"));
	}


	void Update(){
		if(Input.GetKeyDown(KeyCode.F1))
		{
			SwitchCharacter (GetIndexOfTheCharacter ("Character01"));
		}

		if(Input.GetKeyDown(KeyCode.F2))
		{
			SwitchCharacter (GetIndexOfTheCharacter ("Character02"));
		}

		if(Input.GetKeyDown(KeyCode.F3))
		{
			SwitchCharacter (GetIndexOfTheCharacter ("Character03"));
		}

		BlinkingCharacterColor (currentCharacterIndex);
	}


	private void SwitchCharacter(int index)
	{   		
		if (index < characters.Length
		    && index != currentCharacterIndex)
		{
			// Deactivate current character
			DeactivateCharacter (currentCharacterIndex);
			// Set camera to the new character 
			SetCameraTargetTo (index);
			// Activate the new character
			ActivateCharacter (index);
			// Update current character index to be new character
			UpdateCurrentCharacterIndex (index);
		} 
		else
		{
			Debug.Log ("SwitchCharacter(): Index is not valid");
		}

	}


	private void DeactivateCharacter(int index)
	{
		// Switch Character
		if (index < characters.Length) 
		{
//			characters [index].SetActive (false);
			// disable PlayerController
			characters [index].GetComponent<PlayerController>().enabled = false;
			// make it is kineatic to not moveable
			characters [index].GetComponent<Rigidbody>().isKinematic = true;
			// set the character mesh render color to be white back
			characters [index].GetComponent<Renderer> ().material.color = Color.white;
		} 
		else
		{
			Debug.Log ("DeactivateCharacter(): Index is not valid");
		}
	}


	private void ActivateCharacter(int index)
	{
		if (index < characters.Length)
		{
//				characters [index].SetActive (true);
			// PlayerController
			characters [index].GetComponent<PlayerController>().enabled = true;
			// make it is kineatic to  moveable
			characters [index].GetComponent<Rigidbody>().isKinematic = false;

		}
		else
		{
			Debug.Log ("ActivateCharacter(): Index is not valid");
		}
	}


	private void UpdateCurrentCharacterIndex(int index)
	{
		if (index != currentCharacterIndex
		   && index < characters.Length)
		{
			this.currentCharacterIndex = index;
		}
		else
		{
			Debug.Log ("SetCurrentCharacterIndex(): Index Out of bound");
		}
	}

	// TODO bug fix camera positioning when swithching
	private void SetCameraTargetTo(int index)
	{
		if (index != currentCharacterIndex) 
		{
			// reference of camera transform
			Transform camera = characters [currentCharacterIndex].transform.GetChild (0);

			// reference of a switched transform
			Transform newParentTransform = characters [index].transform;

			// attach camera to switched character
			camera.SetParent (newParentTransform);

			// get the switched character location
			Vector3 newParentLocation = characters [index].transform.position;
			Quaternion newParentRotation = characters [index].transform.rotation;


			// roates the camera to be new parent camera
			camera.rotation = Quaternion.Euler (newParentRotation.eulerAngles) * Quaternion.Euler (new Vector3 (45, 0, 0));
			camera.localScale = Vector3.one;

			// update relative location of the camera related to the parent
			camera.localPosition  = new Vector3 (0, 10, -10);
		}
		else 
		{
			Debug.Log ("SetCameraTargetTo(): Index is equal");
		}
	}


	private int GetIndexOfTheCharacter(string name)
	{

		for (int index = 0; index < characters.Length; index++)
		{
			// if the character name equals to what we are looking for
			if (name.Equals(characters[index].name.ToString ()))
			{
				// return it
				return index;
			}
		}

		Debug.Log ("GetIndexOfTheCharacter(): nothing found");

		return -1;
	}

	private void BlinkingCharacterColor(int index){

		if (index == currentCharacterIndex)
		{
			// Change color of current object's mesh
			rend = characters [currentCharacterIndex].GetComponent<Renderer> ();
			float lerp = Mathf.PingPong (Time.time, duration) / duration;
			rend.material.color = Color.Lerp (colorStart, colorEnd, lerp);
		}
		else
		{
			Debug.Log ("BlinkingCharacterColor(): invalid index");
		}
	}
}
