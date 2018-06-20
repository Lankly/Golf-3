using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour {
    private int rotateSpeed = 45;
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, rotateSpeed) * Time.deltaTime);
	}
}
