using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private Text timerText;
    private float runTime = 0.0f;
    private bool running = true;
    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            runTime += Time.deltaTime;
            timerText.text = GetFormattedTime();
        }
    }

    public string GetFormattedTime()
    {
        if ((int)(runTime % 60) < 10)
        {
            return (int)(runTime / 60) + ":0" + (int)(runTime % 60);
        }
        else
        {
            return (int)(runTime / 60) + ":" + (int)(runTime % 60);
        }
    }

    public void SetIsRunning(bool running)
    {
        this.running = running;
    }
}
