using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {

    public Color goalRequirement;
    public string nextLevel;

	public void CheckGoal(Color lightColour)
    {
        if (lightColour.Equals(goalRequirement))
        {
            // Load different stage
            print("Goal met");
            if (nextLevel != "")
            {
                SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
