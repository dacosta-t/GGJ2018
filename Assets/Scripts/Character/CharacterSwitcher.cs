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
		SetCurrentCharacterIndex (characters.Length - 1);
	}

	void Start()
	{	
		int index = 0;
		foreach (GameObject character in characters)
		{
			Debug.Log (character.name.ToString () + " id: " + index++);

			if (!character.name.ToString ().Equals ("Character01")) {
				character.SetActive (false);
			}
		}

		SetCameraTargetTo (characters.Length - 1);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.F1))
		{
			SwitchCharacter (characters.Length - 1);
		}

		if(Input.GetKeyDown(KeyCode.F2))
		{
			SwitchCharacter (characters.Length - 2);
		}

		// TODO : if we are going to have a third character
		if(Input.GetKeyDown(KeyCode.F3))
		{
		}	
	}

	private void SwitchCharacter(int index)
	{
		if (index < characters.Length) {
			if (index != currentCharacterIndex) {
				ActivateCharacter (index);

				SetCameraTargetTo (index);

				DeactivateCharacter (currentCharacterIndex);
			}

			// Update current character index
			SetCurrentCharacterIndex(index);
		}

	}

	private void DeactivateCharacter(int index)
	{
		// Switch Character
		if(index < characters.Length)
			characters [index].SetActive (false);
		else
			Debug.Log ("DisableCharacter(): Index Out of bound");
	}

	private void ActivateCharacter(int index)
	{
		if(index != currentCharacterIndex)
			if (index < characters.Length)
				characters [index].SetActive (true);
			else
				Debug.Log ("ActivateCharacter(): Index Out of bound");
	}

	private void SetCurrentCharacterIndex(int index)
	{
		if(index != currentCharacterIndex)
			if (index < characters.Length)
				this.currentCharacterIndex = index;
			else
				Debug.Log ("SetCurrentCharacterIndex(): Index Out of bound");
	}

	private void SetCameraTargetTo(int index)
	{
		if (index != currentCharacterIndex) {
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
			camera.transform.position = newParentLocation + new Vector3 (0, 10, -10);

			// rotation
			camera.rotation = Quaternion.Euler (newParentRotation.eulerAngles) * Quaternion.Euler (new Vector3 (45, 0, 0));
		}
		else {
			Debug.Log ("SetCameraTargetTo(): Index is equal");
		}
	}

}
