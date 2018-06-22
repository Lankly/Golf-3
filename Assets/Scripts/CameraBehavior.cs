using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //The four players
    public GameObject target;
    public GameObject target2;
    public GameObject target3;
    public GameObject target4;

    //The number of players
    public int playerCount = 2;
    public int currentPlayer = 1;

    public float distance = 15f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = 0f;
    public float yMaxLimit = 80f;

    public float distanceMin = 5f;
    public float distanceMax = 25f;

    float x = 0.0f;
    float y = 0.0f;

    // Use this for initialization
    void Start()
    {
        ChangeTarget(target);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) { SwitchNextPlayer(); }
    }

    // Update, but for physics calculations
    private void FixedUpdate()
    {
    }

    /**
     * Moves camera around target (ball).
     * 
     * Stolen from:
     * http://wiki.unity3d.com/index.php?title=MouseOrbitImproved#Code_C.23
     */
    void LateUpdate()
    {
        // Do nothing if camera has no target
        if (target == null) { return; }

        Transform tt = target.transform;

        // Camera moves to mouse input, so get mouse input
        //We dont want the player moving the camera while power is being used
        if (!(Input.GetMouseButton(0)))
        {
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }
        // Limit y axis (up & down) rotation
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // Covert mouse input to a rotation
        Quaternion rotation = Quaternion.Euler(y, x, 0);

        // Limit distance from target
        distance = Mathf.Clamp(
            distance - Input.GetAxis("Mouse ScrollWheel") * 5
            , distanceMin
            , distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(tt.position, transform.position, out hit))
        {
            distance -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position = rotation * negDistance + tt.position;

        // Set calculated values
        transform.rotation = rotation;
        transform.position = position;
    }

    /*********** Public Methods ***********/

    public bool ChangeTarget(GameObject newTarget)
    {
        if (newTarget == null)
        {
            Debug.Log("Camera didn't get new target reference!");
            return false;
        }

        target = newTarget;
        return true;
    }


    /*********** Private Methods ***********/

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        else if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }

    void SwitchNextPlayer()
    {
        Debug.Log("test");
        int nextPlayer = (currentPlayer + 1) % playerCount;
        if(nextPlayer == 1) { ChangeTarget(target); }
        else if(nextPlayer == 2) { ChangeTarget(target2); }
        else if(nextPlayer == 3) { ChangeTarget(target3); }
        else if(nextPlayer == 4) { ChangeTarget(target4); }
        currentPlayer = nextPlayer;
    }
}
