using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{
    private enum XboxMode
    {
        Pad,
        Track,
        Raw
    }

    public bool Debug = false;

    #region Xbox_Fields
    private float xbox_leftHorz;
    private float xbox_leftVert;
    private float xbox_rightHorz;
    private float xbox_rightVert;
    private float xbox_leftTrigger;
    private float xbox_rightTrigger;
    private float xbox_dpadHorz;
    private float xbox_dpadVert;
    private bool xbox_a;
    private bool xbox_b;
    private bool xbox_x;
    private bool xbox_y;
    private bool xbox_leftBumper;
    private bool xbox_rightBumper;
    private bool xbox_leftStick;
    private bool xbox_rightStick;
    private bool xbox_view;
    private bool xbox_menu;
    #endregion

    private XboxMode left_mode;
    private XboxMode right_mode;

    #region Vive_Fields
    private float vive_leftHorz;
    private float vive_leftVert;
    private float vive_rightHorz;
    private float vive_rightVert;
    private float vive_leftTrigger;
    private float vive_rightTrigger;
    private float vive_leftGrip;
    private float vive_rightGrip;
    #endregion

    [SerializeField]
    public Transform leftController;
    [SerializeField]
    public Transform rightController;
    [SerializeField]
    private Transform simulatorLeft;
    [SerializeField]
    private Transform simulatorRight;
    [SerializeField]
    private GameObject viveTrackers;
    [SerializeField]
    private GameObject viveControllers;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameObject backupCamera;

    public Vector3 LeftPosition
    {
        get
        {
            if (HasVive)
            {
                return leftController.position;
            }
            else if (HasXbox && left_mode == XboxMode.Track)
            {
                return simulatorLeft.position;
            }
            return Vector3.zero;
        }
    }
    public Vector3 RightPosition
    {
        get
        {
            if (HasVive)
            {
                return rightController.position;
            }
            else if (HasXbox && right_mode == XboxMode.Track)
            {
                return simulatorRight.position;
            }
            return Vector3.zero;
        }
    }

    public Vector3 PreviousLeftPosition { get; private set; }
    public Vector3 PreviousRightPosition { get; private set; }

    public bool HasXbox { get; private set; }
    public bool HasVive { get; private set; }

    public float LeftTrigger
    {
        get
        {
            if (HasVive)
            {
                return vive_leftTrigger;
            }
            else if (HasXbox)
            {
                return xbox_leftTrigger;
            }
            return 0;
        }
    }
    public float RightTrigger
    {
        get
        {
            if (HasVive)
            {
                return vive_rightTrigger;
            }
            else if (HasXbox)
            {
                return xbox_rightTrigger;
            }
            return 0;
        }
    }
    public float LeftHorizontal
    {
        get
        {
            if (HasVive)
            {
                return vive_leftHorz;
            }
            else if (HasXbox)
            {
                return xbox_leftHorz;
            }
            return 0;
        }
    }
    public float RightHorizontal
    {
        get
        {
            if (HasVive)
            {
                return vive_rightHorz;
            }
            else if (HasXbox)
            {
                return xbox_rightHorz;
            }
            return 0;
        }
    }
    public float LeftVertical
    {
        get
        {
            if (HasVive)
            {
                return vive_leftVert;
            }
            else if (HasXbox)
            {
                return xbox_leftVert;
            }
            return 0;
        }
    }
    public float RightVertical
    {
        get
        {
            if (HasVive)
            {
                return vive_rightHorz;
            }
            else if (HasXbox)
            {
                return xbox_rightVert;
            }
            return 0;
        }
    }

    private void Start()
    {
        left_mode = XboxMode.Pad;
        right_mode = XboxMode.Pad;
        CheckControllers();
        if (!HasVive)
        {
            viveTrackers.SetActive(false);
            viveControllers.SetActive(false);
            mainCamera.SetActive(false);
            backupCamera.SetActive(true);
            Debug = true;
        }
    }

    void Update()
    {
        CheckControllers();
        if (HasVive) ReadViveInput();
        if (HasXbox)
        {
            ReadXboxInput();
            //test for range of movement with vive controllers
            //click to change xbox mode
            //move & show points if mode in track
        }
    }

    private void LateUpdate()
    {
        PreviousLeftPosition = LeftPosition;
        PreviousRightPosition = RightPosition;
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
        xbox_leftHorz = Input.GetAxis("Horizontal");
        xbox_leftVert = Input.GetAxis("Vertical");
        xbox_rightHorz = Input.GetAxis("HorizontalTurn");
        xbox_rightVert = Input.GetAxis("VerticalTurn");

        xbox_leftTrigger = Input.GetAxis("XboxLeftTrigger");
        xbox_rightTrigger = Input.GetAxis("XboxRightTrigger");
        xbox_dpadHorz = Input.GetAxis("XboxDpadHorizontal");
        xbox_dpadVert = Input.GetAxis("XboxDpadVertical");

        xbox_a = Input.GetButton("XboxA");
        xbox_b = Input.GetButton("XboxB");
        xbox_x = Input.GetButton("XboxX");
        xbox_y = Input.GetButton("XboxY");
        xbox_leftBumper = Input.GetButton("XboxLB");
        xbox_rightBumper = Input.GetButton("XboxRB");
        xbox_leftStick = Input.GetButton("XboxLS");
        xbox_rightStick = Input.GetButton("XboxRS");
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
                xbox_leftTrigger, xbox_rightTrigger,
                xbox_a, xbox_b, xbox_x, xbox_y,
                xbox_leftBumper, xbox_rightBumper, xbox_leftStick, xbox_rightStick,
                xbox_view, xbox_menu,
                xbox_dpadHorz, xbox_dpadVert,
                xbox_leftHorz, xbox_leftVert,
                xbox_rightHorz, xbox_rightVert);

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

        string controllerPositions = $"\nLeft: {LeftPosition}, Right: {RightPosition}";

        GUI.Label(new Rect(0, 0, 500, 500), textXbox + textVive + joyNames + boolChecks + controllerPositions);
    }
}
