﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Character switcher.
///  - Switches(Deactivate/Activate) character to another by F1, F2, F3,
///  - Higlihts a current character,
///  - Attach main camera to the new current character
/// Author: Shawn(Dongwon) Kim
/// </summary>
public class CharacterSwitcher : MonoBehaviour {

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
		currentCharacterIndex = 0;
		DeactivateCharacter (GetIndexOfTheCharacter ("Character02"));
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

	}


	private void SwitchCharacter(int index)
	{   		
		if (index < characters.Length)
		{

			if (index != currentCharacterIndex) {
				// Set camera to the new character 
				SetCameraTargetTo (index);
				// Deactivate current character
				DeactivateCharacter (currentCharacterIndex);
				// Activate the new character
				ActivateCharacter (index);
				// Update current character index to be new character
				UpdateCurrentCharacterIndex (index);
                // Toggle grayscale
                ToggleGrayscale(index);
			}
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
			// turn off the spotlight(higliht) itself
			characters [index].transform.GetChild (0).gameObject.SetActive (false);
		}
	}


	private void ActivateCharacter(int index)
	{
		if (index < characters.Length)
		{
			// PlayerController
			characters [index].GetComponent<PlayerController>().enabled = true;
			// make it is kineatic to  moveable
			characters [index].GetComponent<Rigidbody>().isKinematic = false;
			// turn on the spotlight(higliht) itself
			characters [index].transform.GetChild (0).gameObject.SetActive (true);
		}
	}


	private void UpdateCurrentCharacterIndex(int index)
	{
		if (index < characters.Length)
		{
			this.currentCharacterIndex = index;
		}
	}


	private void SetCameraTargetTo(int index)
	{
		if (index != currentCharacterIndex) 
		{
			// reference of camera transform
			Transform camera = GameObject.Find("MainCamera").transform;

			// reference of a switched transform
			Transform newParentTransform = characters [index].transform;

			// attach camera to switched character
			// camera.SetParent (newParentTransform);
			camera.parent = newParentTransform;

			// get the switched character location
			Vector3 newParentLocation = characters [index].transform.position;
			Quaternion newParentRotation = characters [index].transform.rotation;

			// roates the camera to be new parent camera
			camera.rotation = Quaternion.Euler (newParentRotation.eulerAngles) * Quaternion.Euler (new Vector3 (45, 0, 0));
			camera.localScale = Vector3.one;

			// update relative location of the camera related to the parent
			camera.localPosition  = new Vector3 (0, 10, -10);
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
		return -1;
	}

    private void ToggleGrayscale(int index) {
        if (index == GetIndexOfTheCharacter("Character01")) {
            GameObject.Find("MainCamera").GetComponent<Grayscale>().enabled = true;
        } else if (index == GetIndexOfTheCharacter("Character02")) {
            GameObject.Find("MainCamera").GetComponent<Grayscale>().enabled = false;
        }
    }

//	private void BlinkingCharacterColor(int index){
//
//		if (index == currentCharacterIndex)
//		{
//			// Change color of current object's mesh
//			rend = characters [currentCharacterIndex].GetComponent<Renderer> ();
//
//			if (rend != null)
//			{
//				float lerp = Mathf.PingPong (Time.time, duration) / duration;
//				rend.material.color = Color.Lerp (colorStart, colorEnd, lerp);
//			}
//		}
//		else
//		{
//			Debug.Log ("BlinkingCharacterColor(): invalid index");
//		}
//	}
}
