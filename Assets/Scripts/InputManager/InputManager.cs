using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private enum XboxMode
    {
        Pad,
        Track,
        Raw
    }

    public bool Debug = false;
    public Text DebugText;
    private const int scale = 250;

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

    #region Objects
    [SerializeField]
    private Transform leftController;
    [SerializeField]
    private Transform rightController;
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
    #endregion

    public Transform Player;

    public Vector3 LeftPosition
    {
        get
        {
            if (HasVive)
            {
                Vector3 local;
                if (Player)
                {
                    local = -leftController.InverseTransformPoint(Player.position);
                    local.Normalize();
                    return local;
                }

                local = -leftController.InverseTransformPoint(mainCamera.transform.position);
                local.Normalize();
                return local;
            }
            else if (HasXbox && left_mode == XboxMode.Track)
            {
                return new Vector3(xbox_leftHorz, xbox_leftVert, 0);
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
                Vector3 local;
                if (Player)
                {
                    local = -rightController.InverseTransformPoint(Player.position);
                    local.Normalize();
                    return local;
                }

                local = -rightController.InverseTransformPoint(mainCamera.transform.position);
                local.Normalize();
                return local;
            }
            else if (HasXbox && right_mode == XboxMode.Track)
            {
                return new Vector3(xbox_rightHorz, xbox_rightVert, 0);
            }
            return Vector3.zero;
        }
    }

    public Transform LeftTransform => leftController.transform;
    public Transform RightTransform => rightController.transform;

    public Vector3 PreviousLeftPosition { get; private set; }
    public Vector3 PreviousRightPosition { get; private set; }

    public bool HasXbox { get; private set; }
    public bool HasVive { get; private set; }

    #region Common_Properties
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
            else if (HasXbox && left_mode == XboxMode.Pad)
            {
                return xbox_leftHorz;
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
            else if (HasXbox && left_mode == XboxMode.Pad)
            {
                return xbox_leftVert;
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
            else if (HasXbox && right_mode == XboxMode.Pad)
            {
                return xbox_rightHorz;
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
            else if (HasXbox && right_mode == XboxMode.Pad)
            {
                return xbox_rightVert;
            }
            return 0;
        }
    }

    public Vector2 LeftJoy
    {
        get
        {
            if (HasXbox && left_mode == XboxMode.Raw)
            {
                return new Vector2(xbox_leftHorz, xbox_leftVert);
            }
            return Vector2.zero;
        }
    }
    public Vector2 RightJoy
    {
        get
        {
            if (HasXbox && right_mode == XboxMode.Raw)
            {
                return new Vector2(xbox_rightHorz, xbox_rightVert);
            }
            return Vector2.zero;
        }
    }
    #endregion

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
        PreviousLeftPosition = LeftPosition;
        PreviousRightPosition = RightPosition;

        bool pre_xbox_leftStick = xbox_leftStick;
        bool pre_xbox_rightStick = xbox_rightStick;

        CheckControllers();
        if (HasVive) ReadViveInput();
        if (HasXbox)
        {
            ReadXboxInput();

            if (xbox_rightStick && !pre_xbox_rightStick)
            {
                switch (right_mode)
                {
                    case XboxMode.Pad:
                        right_mode = XboxMode.Track;
                        break;
                    case XboxMode.Track:
                        right_mode = XboxMode.Raw;
                        break;
                    case XboxMode.Raw:
                        right_mode = XboxMode.Pad;
                        break;
                    default:
                        right_mode = XboxMode.Pad;
                        break;
                }
            }

            if (xbox_leftStick && !pre_xbox_leftStick)
            {
                switch (left_mode)
                {
                    case XboxMode.Pad:
                        left_mode = XboxMode.Track;
                        break;
                    case XboxMode.Track:
                        left_mode = XboxMode.Raw;
                        break;
                    case XboxMode.Raw:
                        left_mode = XboxMode.Pad;
                        break;
                    default:
                        left_mode = XboxMode.Pad;
                        break;
                }
            }

            if (left_mode == XboxMode.Track)
            {
                simulatorLeft.localPosition = new Vector3(xbox_leftHorz, xbox_leftVert, 0) * scale;
            }
            if (right_mode == XboxMode.Track)
            {
                simulatorRight.localPosition = new Vector3(xbox_rightHorz, xbox_rightVert, 0) * scale;
            }
        }

        if (DebugText)
        {
            DebugText.text = DebugString();
        }
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
        xbox_leftVert = -Input.GetAxis("Vertical"); // hotfix: inverted?
        xbox_rightHorz = Input.GetAxis("HorizontalTurn");
        xbox_rightVert = -Input.GetAxis("VerticalTurn"); // hotfix: inverted?

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

    private string DebugString()
    {
        string joyNames = "";
        foreach (var name in Input.GetJoystickNames())
        {
            joyNames += name + "\n";
        }

        string boolChecks = "";
        if (HasVive) boolChecks += "HasVive\n";

        string mode = "";
        if (HasXbox)
        {
            boolChecks += "HasXbox\n";
            mode += $"LeftMode: {left_mode}, RightMode: {right_mode}\n";
            if (left_mode == XboxMode.Raw)
            {
                mode += $" LeftJoy: {LeftJoy}";
            }
            if (right_mode == XboxMode.Raw)
            {
                mode += $" RightJoy: {RightJoy}";
            }
            mode += "\n";
        }

        string genericInputs =
            $"\nLeftPos: {LeftPosition}, RightPos: {RightPosition}" +
            $"\nLeftTrig: {LeftTrigger}, RightTrig: {RightTrigger}" +
            $"\nLeftHorz: {LeftHorizontal}, RightHorz: {RightHorizontal}" +
            $"\nLeftVert: {LeftVertical}, RightVert: {RightVertical}";

        return joyNames + boolChecks + mode + genericInputs;
    }

    private float Map(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        float oldRange = (oldMax - oldMin);
        float newRange = (newMax - newMin);
        float newValue = (((value - oldMin) * newRange) / oldRange) + newMin;

        return (newValue);
    }

}
