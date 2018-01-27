using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public bool grabbed = false;
    public float rotationSpeed = 15.0f;
    public GameObject lightParticlePrefab;

    GameObject reflectedLightParticle;

    void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void RotateRight()
    {
        if (grabbed)
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
    }

    public void RotateLeft()
    {
        if (grabbed)
        {
            transform.Rotate(0, -rotationSpeed, 0);
        }
    }

    public void Reflect(Vector3 origin, RaycastHit hit)
    {

        Vector3 incomingVec = hit.point - origin;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
        Vector3 relativePos = reflectVec - hit.point;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Destroy(reflectedLightParticle);
        reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point, rotation);
    }
}
