using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalLight : MonoBehaviour
{

    private float distance = 0;
    private ParticleSystem pSys;
    private ParticleSystem.MainModule pMain;
    private ParticleSystem.EmissionModule pEmission;
    private GameObject mirror;

    [HideInInspector]
    public GameObject box;
    // Use this for initialization
    void Start()
    {
        pSys = GetComponent<ParticleSystem>();
        pMain = pSys.main;
        pEmission = pSys.emission;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // Draw light beam
            distance = hit.distance;
            float factor = Mathf.Sqrt(distance);
            pMain.startLifetime = factor;
            pMain.startSpeed = factor;
            pEmission.rateOverTime = factor * 10;

            // Trigger hit events
            if (hit.transform.tag == "Goal")
            {
                hit.transform.GetComponent<Goal>().CheckGoal(pMain.startColor.color);
            }

            if (hit.transform.tag == "Box" && hit.transform.gameObject != box)
            {
                if (box != null)
                {
                    box.GetComponent<Box>().OnMissChain(transform.eulerAngles);
                }
                box = hit.transform.gameObject;
                box.GetComponent<Box>().OnHit(this, pMain.startColor.color, transform.eulerAngles);
            }
            else if (hit.transform.tag != "Box" && box != null)
            {
                box.GetComponent<Box>().OnMissChain(transform.eulerAngles);
                box = null;
            }
            else if (hit.transform.tag == "Mirror" && hit.transform.gameObject != mirror)
            {
                mirror = hit.transform.gameObject;
                mirror.GetComponent<Mirror>().Reflect(this, transform.position, hit, pMain.startColor.color);
            }
            else if (hit.transform.tag != "Mirror" && mirror != null)
            {
                mirror.GetComponent<Mirror>().RemoveSourceLight();
                mirror.GetComponent<Mirror>().cleanList();
                mirror.GetComponent<Mirror>().Setup();
                mirror = null;
                Debug.Log("clean");
            }
        }
    }
}
