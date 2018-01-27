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

    public bool mirrorUpLeftOrientation = false;

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
        float deltaZ = hit.point.z - origin.z;
        float deltaX = hit.point.x - origin.x;

        Destroy(reflectedLightParticle);
        if (Mathf.Abs(deltaZ) > Mathf.Abs(deltaX))
        {
            if (deltaZ > 0)
            {
                reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                    mirrorUpLeftOrientation ? Quaternion.LookRotation(Vector3.left) : Quaternion.LookRotation(Vector3.right));
            }
            else
            {
                reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                    mirrorUpLeftOrientation ? Quaternion.LookRotation(Vector3.right) : Quaternion.LookRotation(Vector3.left));
            }
        }
        else
        {
            if (deltaX > 0)
            {
                reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                    mirrorUpLeftOrientation ? Quaternion.LookRotation(Vector3.forward) : Quaternion.LookRotation(Vector3.back));
            }
            else
            {
                reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                    mirrorUpLeftOrientation ? Quaternion.LookRotation(Vector3.back) : Quaternion.LookRotation(Vector3.forward));
            }
        }
        shootRay.origin = transform.position;
        shootRay.direction = hit.point - origin;
        if (Physics.Raycast(shootRay, out shootHit))
        {
            Mirror mirror = shootHit.collider.GetComponent<Mirror>();
            //Debug.Log("hello", shootHit.collider);
            mirror.Reflect(hit.point, shootHit);
        }

        /*
    if (!currentHitPoint.Equals(hit.point) || !currentOrigin.Equals(origin))
    {
        Vector3 incomingVec = hit.point - origin;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
        Vector3 relativePos = reflectVec - hit.point;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Destroy(reflectedLightParticle);
        reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point, rotation);
        shootRay = new Ray();
        shootRay.origin = hit.point;
        shootRay.direction = relativePos.z > 0 ? Vector3.forward : Vector3.back;
        currentHitPoint = hit.point;
        currentOrigin = origin;
        if (Physics.Raycast(shootRay, out shootHit))
        {
            Mirror mirror = shootHit.collider.GetComponent<Mirror>();
            //Debug.Log("hello", shootHit.collider);
            mirror.Reflect(transform.position, shootHit);
        }
    }
    else
    {
        Destroy(reflectedLightParticle);
    }
    */
    }
}
