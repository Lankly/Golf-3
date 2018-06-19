using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Camera main;
    private float speed = 0.0f;
    //This is the used to time how long the power meter is held
    private float fireTime = 0.0f;
    //This is how long it takes to get to max power
    private float maxPowerTime = 5.0f;
    //This is what the power is multiplied by
    private float powerMult = 100.0f;

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
        if (Input.GetMouseButton(0) && (fireTime < maxPowerTime))
        {
            //This is where speed is increased
            fireTime = fireTime + Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0) && (fireTime < maxPowerTime))
        {
            //This is where the ball is fired
            speed = fireTime * powerMult;
            Vector3 camera_direction = main.transform.forward;
            camera_direction.y = 0;

            rb.AddForce(camera_direction * speed);
            //INSERT STROKE CODE
        }
        else if (Input.GetMouseButtonUp(0) && fireTime >= maxPowerTime)
        {
            //Overran the power meter
            fireTime = 0.0f;
        }
        else
        {
            fireTime = 0.0f;
        }

       
    }

    // Update, but for physics calculations
    private void FixedUpdate()
    {
    }
}
