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

    public bool mirrorUpRightOrientation = false;

    // Instance of light particle for each new rotation
    GameObject reflectedLightParticle;

    // Current origin of light before reflection
    Vector3 currentOrigin;

    // Current point of contact of light on mirror
    Vector3 currentHitPoint;

    Color currentColor;

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
        if (mirrorUpRightOrientation)
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    // Rotate mirror to the right
    public void RotateRight()
    {
        if (grabbed)
        {
            transform.Rotate(0, rotationSpeed, 0);
            mirrorUpRightOrientation = !mirrorUpRightOrientation;

        }
    }

    // Rotate mirror to the left
    public void RotateLeft()
    {
        if (grabbed)
        {
            transform.Rotate(0, -rotationSpeed, 0);
            mirrorUpRightOrientation = !mirrorUpRightOrientation;
        }
    }

    // Reflect light off the surface of the mirror
    //      Instantiates new light when light origin and hit point changes
    public void Reflect(Vector3 origin, RaycastHit hit, Color color)
    {
        float deltaZ = hit.point.z - origin.z;
        float deltaX = hit.point.x - origin.x;
        if (!currentHitPoint.Equals(hit.point) || !currentOrigin.Equals(origin) || !currentColor.Equals(color))
        {
            Destroy(reflectedLightParticle);
            if (Mathf.Abs(deltaZ) >= Mathf.Abs(deltaX))
            {
                if (deltaZ >= 0)
                {
                    reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                        mirrorUpRightOrientation ? Quaternion.LookRotation(Vector3.right) : Quaternion.LookRotation(Vector3.left));
                }
                else
                {
                    reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                        mirrorUpRightOrientation ? Quaternion.LookRotation(Vector3.left) : Quaternion.LookRotation(Vector3.right));
                }
            }
            else
            {
                if (deltaX >= 0)
                {
                    reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                        mirrorUpRightOrientation ? Quaternion.LookRotation(Vector3.forward) : Quaternion.LookRotation(Vector3.back));
                }
                else
                {
                    reflectedLightParticle = Instantiate(lightParticlePrefab, hit.point,
                        mirrorUpRightOrientation ? Quaternion.LookRotation(Vector3.back) : Quaternion.LookRotation(Vector3.forward));
                }
            }
            ParticleSystem light = reflectedLightParticle.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule lightMain = light.main;
            lightMain.startColor = color;
            currentHitPoint = hit.point;
            currentOrigin = origin;
            currentColor = color;
            shootRay.origin = transform.position;
            shootRay.direction = Vector3.right;
            if (Mathf.Abs(deltaZ) >= Mathf.Abs(deltaX))
            {
                if (deltaZ >= 0)
                {
                    if (mirrorUpRightOrientation) shootRay.direction = Vector3.right; else shootRay.direction = Vector3.left;
                }
                else
                {
                    if (mirrorUpRightOrientation) shootRay.direction = Vector3.left; else shootRay.direction = Vector3.right;
                }
            }
            else
            {
                if (deltaX >= 0)
                {
                    if (mirrorUpRightOrientation) shootRay.direction = Vector3.forward; else shootRay.direction = Vector3.back;
                }
                else
                {
                    if (mirrorUpRightOrientation) shootRay.direction = Vector3.back; else shootRay.direction = Vector3.forward;
                }
            }
            if (Physics.Raycast(shootRay, out shootHit))
            {
                Mirror mirror = shootHit.collider.GetComponent<Mirror>();
                if (mirror != null)
                {
                    mirror.Reflect(hit.point, shootHit, color);
                }
            }
        }
    }
}
