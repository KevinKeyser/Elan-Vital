using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey("up"))
        {
            transform.position += Vector3.forward;
        }
        else if (Input.GetKey("down"))
        {
            transform.position += Vector3.back;
        }
        if(Input.GetKey("left"))
        {
            transform.position += Vector3.left;
        }
        else if (Input.GetKey("right"))
        {
            transform.position += Vector3.right;
        }
	}
}
