using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour {

	GameObject[] characters;
	private Vector3 offset;        

	public int currentCharacterIndex;


	void Awake()
	{
		characters = GameObject.FindGameObjectsWithTag ("Player");
		SetCurrentCharacterIndex (GetIndexOfTheCharacter ("Character01"));
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
	}


	private void SwitchCharacter(int index)
	{
		if (index < characters.Length
		    && index != currentCharacterIndex)
		{

			ActivateCharacter (index);

			SetCameraTargetTo (index);

			DeactivateCharacter (currentCharacterIndex);

			// Update current character index
			SetCurrentCharacterIndex (index);
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


	private void SetCurrentCharacterIndex(int index)
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
			// reference of a switched transform
			Transform newParentTransform = characters [index].transform;

			// reference of camera transform
			Transform camera = characters [currentCharacterIndex].transform.GetChild (0);

			// attach camera to switched character
			camera.SetParent (newParentTransform);

			// get the switched character location
			Vector3 newParentLocation = characters [index].transform.position;
			Quaternion newParentRotation = characters [index].transform.rotation;

			camera.transform.position = new Vector3 (0, 0, 0);

			// update new location of the camera related to the parent
			camera.transform.position = new Vector3 (0, 10, -10) + newParentRotation.eulerAngles;

			// rotation
			camera.rotation = Quaternion.Euler (newParentRotation.eulerAngles) * Quaternion.Euler (new Vector3 (45, 0, 0));

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

}
