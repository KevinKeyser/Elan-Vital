using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class MoveByController : MonoBehaviour
{
    private Rigidbody body;
    public InputManager Manager;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body && Manager)
        {
            //var x = Manager.LeftHorizontal * Time.deltaTime * 
        }
    }
}
