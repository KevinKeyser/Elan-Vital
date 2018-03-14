using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{

    //Get Controller locations from object
    // LeftLocation
    // RightLocation


    public bool Debug = false;

    //What Isaac needs
    public Vector3[] LeftTrail { get; private set; }
    public Vector3[] RightTrail { get; private set; }
    public bool HasXbox { get; private set; }
    public bool HasVive { get; private set; }

    public float LeftTrigger { get; private set; }
    public float RightTrigger { get; private set; }
    public float LeftHorizontal { get; private set; }
    public float RightHorizontal { get; private set; }
    public float LeftVertical { get; private set; }
    public float RightVertical { get; private set; }

    //Xbox Input
    private float hAxis;
    private float vAxis;
    private float htAxis;
    private float vtAxis;
    private float ltaxis;
    private float rtaxis;
    private float dhaxis;
    private float dvaxis;
    private bool xbox_a;
    private bool xbox_b;
    private bool xbox_x;
    private bool xbox_y;
    private bool xbox_lb;
    private bool xbox_rb;
    private bool xbox_ls;
    private bool xbox_rs;
    private bool xbox_view;
    private bool xbox_menu;

    //Vive Input
    private float vive_leftHorz;
    private float vive_leftVert;
    private float vive_rightHorz;
    private float vive_rightVert;
    private float vive_leftTrigger;
    private float vive_rightTrigger;
    private float vive_leftGrip;
    private float vive_rightGrip;

    private void Start()
    {
        CheckControllers();
    }

    void Update()
    {
        CheckControllers();
        if (HasVive) ReadViveInput();
        if (HasXbox) ReadXboxInput();
    }

    private void CheckControllers()
    {
        HasXbox = false;
        HasVive = false;
        foreach (var name in Input.GetJoystickNames())
        {
            if (name.Contains("Xbox One"))
            {
                HasXbox = true;
                continue;
            }
            if (name.Contains("Vive"))
            {
                HasVive = true;
            }
        }
    }

    private void ReadXboxInput()
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

    private void ReadViveInput()
    {
        vive_leftHorz = Input.GetAxis("HTC_VIU_LeftTrackpadHorizontal");
        vive_leftVert = Input.GetAxis("HTC_VIU_LeftTrackpadVertical");
        vive_rightHorz = Input.GetAxis("HTC_VIU_RightTrackpadHorizontal");
        vive_rightVert = Input.GetAxis("HTC_VIU_RightTrackpadVertical");
        vive_leftTrigger = Input.GetAxis("HTC_VIU_LeftTrigger");
        vive_rightTrigger = Input.GetAxis("HTC_VIU_RightTrigger");
        vive_leftGrip = Input.GetAxis("HTC_VIU_LeftGrip");
        vive_rightGrip = Input.GetAxis("HTC_VIU_RightGrip");
    }

    public void OnGUI()
    {
        if (!Debug) return;

        string textXbox =
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

        string textVive =
            string.Format(
                "\n\n" +
                "Left_Horz: {0:0.000}   Right_Horz: {2:0.000}\n" +
                "Left_Vert: {1:0.000}   Right_Vert: {3:0.000}\n" +
                "LTrigger: {4:0.000}    RTrigger: {5:0.000}\n" +
                "Left_Grip: {6:0.000}   Right_Grip: {7:0.000}\n",
                vive_leftHorz, vive_leftVert, vive_rightHorz,
                vive_rightVert, vive_leftTrigger, vive_rightTrigger,
                vive_leftGrip, vive_rightGrip
                );

        string joyNames = "\n";
        foreach (var name in Input.GetJoystickNames())
        {
            joyNames += name + "\n";
        }

        string boolChecks = "\n";
        if (HasVive) boolChecks += "HasVive\n";
        if (HasXbox) boolChecks += "HasXbox\n";

        GUI.Label(new Rect(0, 0, 500, 500), textXbox + textVive + joyNames + boolChecks);
    }
}
