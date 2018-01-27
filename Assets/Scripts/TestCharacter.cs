using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{

    public float speed = 6f;
    public GameObject mirrorObject;

    Vector3 movement;
    Rigidbody playerRigidbody;
    Mirror mirror;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        mirror = mirrorObject.GetComponent<Mirror>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mirror.RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            mirror.RotateRight();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(-h, -v);
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            mirror.Rotate(playerRigidbody.position);
        }
        */
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }
}
