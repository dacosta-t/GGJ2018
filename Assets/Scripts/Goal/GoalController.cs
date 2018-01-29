using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalController : MonoBehaviour {

    private List<Goal> goals = new List<Goal>();
    public string nextLevel;

    public void addGoal(Goal goal) {
        goals.Add(goal);
    }

    public void CheckGoals(Color colour) {
        for (int i = 0; i < goals.Count; i++) {
            print(goals[i].isComplete);
            if (!goals[i].isComplete) {
                return;
            }
        }

		// Stage Cleared Panel
		GameObject canvas = GameObject.Find ("Canvas");
		if (canvas != null)
		{
            Timer timer = GameObject.Find("Timer").GetComponent<Timer>();
			Transform tStageClearedPanel = canvas.transform.GetChild (2);
            Text tStageClearedText = tStageClearedPanel.GetChild(0).GetComponent<Text>();

            timer.SetIsRunning(false);
            //tStageClearedText.text = timer.GetFormattedTime();
            //string temp = "STAGE\nCLEARED\n" + timer.GetFormattedTime();
            tStageClearedText.text = "STAGE\nCLEARED\n\n" + timer.GetFormattedTime();

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
		if (nextLevel != "") {
			SceneManager.LoadScene(nextLevel);
		}
	}
}
