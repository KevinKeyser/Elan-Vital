using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{

    float hAxis;
    float vAxis;
    float htAxis;
    float vtAxis;

    float ltaxis;
    float rtaxis;
    float dhaxis;
    float dvaxis;

    bool xbox_a;
    bool xbox_b;
    bool xbox_x;
    bool xbox_y;
    bool xbox_lb;
    bool xbox_rb;
    bool xbox_ls;
    bool xbox_rs;
    bool xbox_view;
    bool xbox_menu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControllerInput();
    }

    void ControllerInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        htAxis = Input.GetAxis("HorizontalTurn");
        vtAxis = Input.GetAxis("VerticalTurn");

        ltaxis = Input.GetAxis("XboxLeftTrigger");
        rtaxis = Input.GetAxis("XboxRightTrigger");
        dhaxis = Input.GetAxis("XboxDpadHorizontal");
        dvaxis = Input.GetAxis("XboxDpadVertical");

        xbox_a = Input.GetButton("XboxA");
        xbox_b = Input.GetButton("XboxB");
        xbox_x = Input.GetButton("XboxX");
        xbox_y = Input.GetButton("XboxY");
        xbox_lb = Input.GetButton("XboxLB");
        xbox_rb = Input.GetButton("XboxRB");
        xbox_ls = Input.GetButton("XboxLS");
        xbox_rs = Input.GetButton("XboxRS");
        xbox_view = Input.GetButton("XboxView");
        xbox_menu = Input.GetButton("XboxMenu");
    }

    void ViveInput()
    {

    }

    public void OnGUI()
    {
        string text =
            string.Format(
                "Horizontal: {14:0.000} Vertical: {15:0.000}\n" +
                "HorizontalTurn: {16:0.000} VerticalTurn: {17:0.000}\n" +
                "LTrigger: {0:0.000} RTrigger: {1:0.000}\n" +
                "A: {2} B: {3} X: {4} Y:{5}\n" +
                "LB: {6} RB: {7} LS: {8} RS:{9}\n" +
                "View: {10} Menu: {11}\n" +
                "Dpad-H: {12:0.000} Dpad-V: {13:0.000}\n",
                ltaxis, rtaxis,
                xbox_a, xbox_b, xbox_x, xbox_y,
                xbox_lb, xbox_rb, xbox_ls, xbox_rs,
                xbox_view, xbox_menu,
                dhaxis, dvaxis,
                hAxis, vAxis,
                htAxis, vtAxis);

        GUI.Label(new Rect(0, 0, 400, 300), text);
    }
}
