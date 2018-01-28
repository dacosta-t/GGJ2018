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

        print("Completed level!");
        if (nextLevel != "") {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
