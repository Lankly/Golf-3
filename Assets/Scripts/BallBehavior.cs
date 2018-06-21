using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallBehavior : MonoBehaviour
{
    private Rigidbody rb;
    private Camera main;
    public Text countText;
    
    public int numberOfPlayers = 2;
    public int activePlayer;

    public int par = 3;
    public Text holeText;
    private int stroke = 0;

    private int framesStopped = 0; // Frames since the ball has stopped moving quickly


    private bool moving = true;
    private float minBallSpeed = 2.0f;
    
    
    //Speeds
    private float speed = 0.0f;
    
    //This is the used to time how long the power meter is held
    private float fireTime = 0.0f;
    
    //This is how long it takes to get to max power
    private float maxPowerTime = 5.0f;

    //This is what the power is multiplied by
    private float powerMult = 50000.0f;

    //These are for handling wanting to reline up the shot
    //This is how long it takes to reset the power meter after max power has been held for a while
    private float waitTime = 0.0f;
    private float maxPowerWaitToReset = 2.0f;
    private bool mouseClicked = false;
    
    //This is the text for power
    private int powerPercent = 0;
    public Text powerText;

    //This updates the stroke count
    void updateCountText()
    {
        countText.text = "Stroke: " + stroke.ToString();
    }

    //This updates the power meter
    void updatePowerText()
    {
        powerPercent = (int)((fireTime*100.0f) / maxPowerTime);
        powerText.text = "Power: " + powerPercent.ToString();
    }

    // Use this for initialization
    void Start()
    {
        main = Camera.main;
        rb = GetComponent<Rigidbody>();
        stroke = 0;
        updateCountText();
        updatePowerText();
        holeText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Do a check for ball movement before progressing to next turn
        // Clicking anywhere moves ball. No holding to power up shot yet
        //BALL MOVEMENT CODE
        Vector3 ballVelocity = rb.velocity;
        if(ballVelocity.magnitude < minBallSpeed)
        {
            framesStopped += 1;
            if (framesStopped > 30) // If the ball hasn't moved much for 30 frames, then assume it's stopped moving forever.
            {
                rb.velocity = new Vector3(0, 0, 0);
                // TODO: project ball down to terrain (it shouldn't stop in midair)
                moving = false;
                updatePowerText();
            }
        }
        else
        {
            framesStopped = 0;
            moving = true;
        }

        //We want to only take a stroke when the mouse was clicked and the ball was not moving
        if ((moving == false) && (mouseClicked == true))
        {
            if (Input.GetMouseButton(0) && (fireTime < maxPowerTime))
            {
                //This is where speed is increased
                fireTime = fireTime + Time.deltaTime;
                updatePowerText();
            }
            //Player releases mouse, power is less than max, and we have not exceeded the wait time
            else if (Input.GetMouseButtonUp(0) && (fireTime <= maxPowerTime) && (waitTime < maxPowerWaitToReset))
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
            else if (waitTime < maxPowerWaitToReset)
            {
                waitTime = waitTime + Time.deltaTime;
            }
            //The player wants to redo the lining up of shot
            else if ((waitTime >= maxPowerWaitToReset))
            {
                //Overran the power meter
                fireTime = 0.0f;
                waitTime = 0.0f;
                mouseClicked = false;
                updatePowerText();
            }
            else
            {
                fireTime = 0.0f;
                speed = 0.0f;
                updatePowerText();
            }
        }
        //This signals that the mouse has been clicked
        else if (Input.GetMouseButtonDown(0) && mouseClicked == false)
        {
            mouseClicked = true;
        }

      
    }
    private void OnTriggerEnter(Collider other)
    {
        //Power Up and Hole Logic
        if (other.gameObject.CompareTag("Power Up"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Hole"))
        {
            rb.gameObject.SetActive(false);
            if(stroke == 1)               { holeText.text = "Hole In One"; }
            else if (stroke == (par - 4)) { holeText.text = "Dodo?"; }
            else if (stroke == (par - 3)) { holeText.text = "Double Eagle"; }
            else if (stroke == (par - 2)) { holeText.text = "Eagle"; }
            else if (stroke == (par - 1)) { holeText.text = "Birdie"; }
            else if (stroke == (par))     { holeText.text = "Par"; }
            else if (stroke == (par + 1)) { holeText.text = "Bogie"; }
            else if (stroke == (par + 2)) { holeText.text = "Double Bogie"; }
            else if (stroke == (par + 3)) { holeText.text = "Triple Bogie"; }
            else {
                int overPar = stroke - par;
                holeText.text = "+" + overPar.ToString();
            }
        }
    }
    // Update, but for physics calculations
    private void FixedUpdate()
    {
    }
}
