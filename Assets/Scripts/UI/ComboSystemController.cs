using System.Collections;
using System.Collections.Generic;
using ElanVital.UI;
using UnityEngine;

public class ComboSystemController : MonoBehaviour
{
    public InputLocation InputLocation = InputLocation.Right;
    [Range(1, 100)] [SerializeField] private int MaxTraveledSections = 100;

    [SerializeField] private CombatWheel wheel;
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private InputManager inputManager;

    public delegate void ComboEvent(ComboSystemController comboController, int sectionNumber, int comboCount);
    public event ComboEvent ComboIncreased;

    //traveled sections private and count of it (public get/no set)
    private List<int> traveledSections = new List<int>();

    public int ComboCount
    {
        get { return traveledSections.Count; }
    }

    
    //depth
    [SerializeField] private List<Color> ComboColors = new List<Color>();
    //Identify Approved Section Passthroughs For Things Like Colors
    private int[] sectionHitCount;

    private int lastSection = -1;


    void Start()
    {
        sectionHitCount = new int[wheel.SectionCount + 1];
    }

    void Update()
    {
        Vector3 inputPosition = Vector3.zero;
        if (InputLocation == InputLocation.Right)
            inputPosition = inputManager.RightPosition;
        else
            inputPosition = inputManager.LeftPosition;
        Vector2 currentPoint = new Vector2(inputPosition.x, inputPosition.y) * (wheel.Radius + wheel.InnerPadding + wheel.Thickness);
        if (lineRenderer)
        {
            lineRenderer.AddPoint(currentPoint);
        }

        if (wheel)
        {
            int currentSection = wheel.SectionContains(currentPoint);
            if (currentSection != -1)
            {
                if (lastSection != currentSection)
                {
                    if (lastSection == 0 || currentSection == 0)
                    {
                        traveledSections.Add(currentSection);
                        sectionHitCount[currentSection]++;
                        //Debug.Log($"Current section:{currentSection} : Hit count: {sectionHitCount[currentSection]}");
                        wheel.SetSectionColor(currentSection,
                            ComboColors[Mathf.Min(sectionHitCount[currentSection], ComboColors.Count - 1)]);
                        ComboIncreased?.Invoke(this, currentSection, traveledSections.Count);
                    }
                }

                lastSection = currentSection;
            }
        }


        while (traveledSections.Count > MaxTraveledSections)
        {
            traveledSections.RemoveAt(0);
        }
    }
}
