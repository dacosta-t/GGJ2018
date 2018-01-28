using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private Text timerText;
    private float runTime = 0.0f;
    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        runTime += Time.deltaTime;
        if ((int)(runTime % 60) < 10)
        {
            timerText.text = "" + (int)(runTime / 60) + ":0" + (int)(runTime % 60);
        }
        else
        {
            timerText.text = "" + (int)(runTime / 60) + ":" + (int)(runTime % 60);
        }
    }
}
