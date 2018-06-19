using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Camera main;
    public Text countText;

    private int stroke = 0;


    private bool moving = false;
    private float minBallSpeed = 2.0f;
    
    
    //Speeds
    private float speed = 0.0f;
    //This is the used to time how long the power meter is held
    private float fireTime = 0.0f;
    //This is how long it takes to get to max power
    private float maxPowerTime = 5.0f;
    //This is what the power is multiplied by
    private float powerMult = 50000.0f;

    // Use this for initialization
    void updateCountText()
    {
        countText.text = "Stroke: " + stroke.ToString();
    }
    void Start()
    {
        main = Camera.main;
        rb = GetComponent<Rigidbody>();
        stroke = 0;
        updateCountText();
    }

    // Update is called once per frame
    void Update()
    {
        // Do a check for ball movement before progressing to next turn
        // Clicking anywhere moves ball. No holding to power up shot yet
        Vector3 ballVelocity = rb.velocity;
        if(ballVelocity.magnitude < minBallSpeed)
        {
            moving = false;
            rb.velocity = new Vector3(0, 0, 0);
        }
        else
        {
            moving = true;
        }
        if (moving == false)
        {
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
                //This is used to ensure the force is the same no matter where the camera is pointed at
                float xDir = camera_direction.x;
                float zDir = camera_direction.z;
                float magDir = (Mathf.Pow(xDir, 2) + Mathf.Pow(zDir, 2));
                magDir = Mathf.Sqrt(magDir);
                camera_direction.x = (xDir / magDir);
                camera_direction.z = (zDir / magDir);

                rb.AddForce(camera_direction * speed);
                //INSERT STROKE CODE
                stroke = stroke + 1;
                updateCountText();
            }
            else if (Input.GetMouseButtonUp(0) && fireTime >= maxPowerTime)
            {
                //Overran the power meter
                fireTime = 0.0f;
            }
            else
            {
                fireTime = 0.0f;
                speed = 0.0f;
            }
        }

       
    }

    // Update, but for physics calculations
    private void FixedUpdate()
    {
    }
}
