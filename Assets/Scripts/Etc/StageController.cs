using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Stage Controller
/// Author: Shawn Kim
/// </summary>
public class StageController : MonoBehaviour {

	/// <summary>
	/// Goes to this scene.
	/// </summary>
	/// <param name="scene">Destination scene to be sent</param>
	public void goToThisScene(string scene) {
		SceneManager.LoadScene(scene);	// move to the scene
	}

}
