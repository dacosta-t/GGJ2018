using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Mirror starting grab state
    public bool grabbed = false;

    // Degree of rotation applied for each button press
    public float rotationSpeed = 15.0f;

    // Reference to Light prefab
    public GameObject lightParticlePrefab;

    // Instance of light particle for each new rotation
    GameObject reflectedLightParticle;

    // Current origin of light before reflection
    Vector3 currentOrigin;

    // Current point of contact of light on mirror
    Vector3 currentHitPoint;

    Ray shootRay = new Ray();

    RaycastHit shootHit;
    void Awake()
    {
        currentOrigin = Vector3.zero;
        currentHitPoint = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Rotate mirror to the right
    public void RotateRight()
    {
        if (grabbed)
        {
            transform.Rotate(0, rotationSpeed, 0);
        }
    }

    // Rotate mirror to the left
    public void RotateLeft()
    {
        if (grabbed)
        {
            transform.Rotate(0, -rotationSpeed, 0);
        }
    }

    // Reflect light off the surface of the mirror
    //      Instantiates new light when light origin and hit point changes
    public void Reflect(Vector3 origin, RaycastHit hit)
    {
        if (!currentHitPoint.Equals(hit.point) || !currentOrigin.Equals(origin))
        {
            Vector3 incomingVec = hit.point - origin;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Vector3 relativePos = reflectVec - hit.point;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            Destroy(reflectedLightParticle);
            reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point, rotation);
            currentHitPoint = hit.point;
            currentOrigin = origin;
            shootRay.origin = hit.point;
            shootRay.direction = relativePos;
            if (Physics.Raycast(shootRay, out shootHit))
            {
                Mirror mirror = shootHit.collider.GetComponent<Mirror>();
                //Debug.Log("hello", shootHit.collider);
                mirror.Reflect(transform.position, shootHit);
            }
            else
            {
                Destroy(reflectedLightParticle);
            }
        }
        else
        {
            Destroy(reflectedLightParticle);
        }
    }
}
