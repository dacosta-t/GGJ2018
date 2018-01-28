using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour {

    private List<Goal> goals;
    public string nextLevel;

    private void Start() {
        goals = new List<Goal>();
    }

    public void addGoal(Goal goal) {
        goals.Add(goal);
        print(goals.Count);
    }

    public void CheckGoals(Color colour) {
        for (int i = 0; i < goals.Count; i++) {
            print(goals[i].isComplete);
            if (!goals[i].isComplete) {
                print("Not all goals complete");
                return;
            }
        }

		// Stage Cleared Panel
		GameObject canvas = GameObject.Find ("Canvas");
		Transform tStageClearedPanel = canvas.transform.GetChild (2);
		if (tStageClearedPanel != null) 
		{
			//activate the panel
			tStageClearedPanel.gameObject.SetActive (true);
			tStageClearedPanel.GetComponent<GlowImageOutline> ().enabled = true;

			// references to character
			GameObject[] characters = GameObject.FindGameObjectsWithTag ("Player");

			//set animation
			foreach (GameObject character in characters) 
			{
				character.GetComponent<Animator> ().SetTrigger ("Push");
				character.GetComponent<Animator> ().SetTrigger ("Pull");
				character.GetComponent<Animator> ().SetTrigger ("Push");
				character.GetComponent<Animator> ().SetTrigger ("Pull");
			}
		}

		// pausing tihngwith coroutine
		StartCoroutine ("WaitForSecondToChangeScene");
    }

	IEnumerator WaitForSecondToChangeScene() {
		yield return new WaitForSeconds(4);
		SendToNextScene();
	}

	public void SendToNextScene(){
		print("Completed level!");
		if (nextLevel != "") {
			SceneManager.LoadScene(nextLevel);
		}
	}
}
