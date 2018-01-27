using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{

    //RaycastHit lightHit;
    public float rotationSpeed = 15.0f;
    LineRenderer reflectedLight;
    bool grabbed = false;

    void Awake()
    {
        reflectedLight = GetComponent<LineRenderer>();

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate();
    }

    public void RotateRight()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    public void RotateLeft()
    {
        transform.Rotate(0, -rotationSpeed, 0);
    }

    public void Reflect(Vector3 origin, RaycastHit hit)
    {
        Vector3 incomingVec = hit.point - origin;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
        reflectedLight.SetPosition(0, hit.point);
        reflectedLight.SetPosition(1, reflectVec);
        reflectedLight.enabled = true;
    }
}
