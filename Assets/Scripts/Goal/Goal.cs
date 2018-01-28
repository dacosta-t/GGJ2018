using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

    private GoalController goalController;
    public bool isComplete;

    public Color goalRequirement;

    void Start() {
        goalController = GameObject.Find("GoalController").GetComponent<GoalController>();
        goalController.addGoal(this);
        isComplete = false;
    }

    public void CheckGoal(Color colour) {
        SetGoal(colour);
        if (isComplete) {
            goalController.CheckGoals(colour);
        }
    }

    public void SetGoal(Color lightColour) {
        isComplete = lightColour.Equals(goalRequirement);
        print(isComplete);
    }
}
