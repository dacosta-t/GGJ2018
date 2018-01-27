using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalLight : MonoBehaviour {

    private float distance = 0;
    private ParticleSystem pSys;
    private ParticleSystem.MainModule pMain;
    private ParticleSystem.EmissionModule pEmission;

	// Use this for initialization
	void Start () {
        pSys = GetComponent<ParticleSystem>();
        pMain = pSys.main;
        pEmission = pSys.emission;
	}

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            distance = hit.distance;
            float factor = Mathf.Sqrt(distance);
            pMain.startLifetime = factor;
            pMain.startSpeed = factor;
            pEmission.rateOverDistance = 10;
        }
    }
}
