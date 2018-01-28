using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLight : MonoBehaviour
{
    Ray shootRay = new Ray();
    RaycastHit shootHit;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;
        if (Physics.Raycast(shootRay, out shootHit))
        {
            Mirror mirror = shootHit.collider.GetComponent<Mirror>();
            //Debug.Log("hello", shootHit.collider);
            //mirror.Reflect(transform.position, shootHit, Color.white);
        }
    }
}
