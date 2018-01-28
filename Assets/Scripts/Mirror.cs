using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    // Degree of rotation applied for each button press
    public float rotationSpeed = 15.0f;

    // Reference to Light prefab
    public GameObject lightParticlePrefab;

    // Mirror starting grab state
    bool grabbed = false;

    bool mirrorUpRightOrientation = false;

    // Instance of light particle for each new rotation
    GameObject reflectedLightParticle;

    // Current origin of light before reflection
    Vector3 currentOrigin;

    // Current point of contact of light on mirror
    Vector3 currentHitPoint;

    RaycastHit currentHit;

    Quaternion currentRotation;

    Color currentColor;

    PhysicalLight sourceLight = null;

    Ray shootRay = new Ray();

    RaycastHit shootHit;

    static LinkedList<Mirror> mirrors = new LinkedList<Mirror>();

    void Awake()
    {
        currentOrigin = Vector3.zero;
        currentHitPoint = Vector3.zero;
    }

    // Use this for initialization
    void Start()
    {
        if (transform.rotation == Quaternion.Euler(0, 135, 0) || transform.rotation == Quaternion.Euler(0, 315, 0))
        {
            mirrorUpRightOrientation = true;
        }
        else if (transform.rotation == Quaternion.Euler(0, 45, 0) || transform.rotation == Quaternion.Euler(0, 225, 0))
        {
            mirrorUpRightOrientation = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
            mirrorUpRightOrientation = false;
        }
        currentRotation = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.rotation == Quaternion.Euler(0, 135, 0) || transform.rotation == Quaternion.Euler(0, 315, 0))
        {
            mirrorUpRightOrientation = true;
        }
        else if (transform.rotation == Quaternion.Euler(0, 45, 0) || transform.rotation == Quaternion.Euler(0, 225, 0))
        {
            mirrorUpRightOrientation = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 45, 0);
            mirrorUpRightOrientation = false;
        }
        if (currentRotation != transform.rotation)
        {
            cleanList();
            currentRotation = transform.rotation;
        }
    }

    // Rotate mirror 
    public void Rotate()
    {
        if (grabbed)
        {
            transform.Rotate(0, rotationSpeed, 0);
            cleanList();
        }
    }

    // Reflect light off the surface of the mirror
    //      Instantiates new light when light origin and hit point changes
    public void Reflect(PhysicalLight source, Vector3 origin, RaycastHit hit, Color color)
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
            sourceLight = source;
            lightMain.startColor = color;
            currentHitPoint = hit.point;
            currentHit = hit;
            currentOrigin = origin;
            currentColor = color;
            mirrors.AddLast(this);
            shootRay = new Ray();
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
                    mirror.Reflect(source, hit.point, shootHit, color);
                }
            }
        }
    }

    public void Setup()
    {
        reflectedLightParticle = null;

        currentOrigin = Vector3.zero;

        currentHitPoint = Vector3.zero;


        currentRotation = Quaternion.identity;


        sourceLight = null;
    }

    private void Reflect()
    {
        Reflect(sourceLight, currentOrigin, currentHit, currentColor);
    }

    public void cleanList()
    {
        LinkedListNode<Mirror> last = mirrors.Find(this);
        LinkedListNode<Mirror> next = last.Next;
        while (next.Next != null)
        {
            next.Value.sourceLight = null;
            Destroy(next.Value.reflectedLightParticle);
            LinkedListNode<Mirror> temp = next.Next;
            mirrors.Remove(next);
            next = temp;
        }
    }

    public void RemoveReflectedLight()
    {
        Destroy(reflectedLightParticle);
    }

    public void RemoveSourceLight()
    {
        sourceLight = null;
    }
}
