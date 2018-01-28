using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalLight : MonoBehaviour {

    private float distance = 0;
    private ParticleSystem pSys;
    private ParticleSystem.MainModule pMain;
    private ParticleSystem.EmissionModule pEmission;
    private GameObject box;

	// Use this for initialization
	void Start () {
        pSys = GetComponent<ParticleSystem>();
        pMain = pSys.main;
        pEmission = pSys.emission;
	}

    void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) {
            // Draw light beam
            distance = hit.distance;
            float factor = Mathf.Sqrt(distance);
            pMain.startLifetime = factor;
            pMain.startSpeed = factor;
            pEmission.rateOverTime = factor * 10;

            // Trigger hit events
            if (hit.transform.tag == "Box" && hit.transform.gameObject != box) {
                box = hit.transform.gameObject;
                box.GetComponent<Box>().OnHit(this, pMain.startColor.color, transform.eulerAngles);
            } else if (hit.transform.tag != "Box" && box != null) {
                box.GetComponent<Box>().OnMiss(transform.eulerAngles);
                box = null;
            }
        }
    }
}
