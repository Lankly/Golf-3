using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Camera main;

    // Use this for initialization
    void Start()
    {
        main = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Clicking anywhere moves ball. No holding to power up shot yet
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 camera_direction = main.transform.forward;
            camera_direction.y = 0;

            rb.AddForce(camera_direction * 1000);
        }
    }

    // Update, but for physics calculations
    private void FixedUpdate()
    {
    }
}
