using System.Collections;
using System.Collections.Generic;
using ElanVital.UI;
using UnityEngine;

public class ComboSystemController : MonoBehaviour
{
    [Range(1, 100)] [SerializeField] private int MaxTraveledSections = 100;

    [SerializeField] private CombatWheel wheel;
    [SerializeField] private UILineRenderer lineRenderer;
    [SerializeField] private InputManager inputManager;

    public List<int> traveledSections = new List<int>();

    //depth
    [SerializeField] private List<Color> ComboColors = new List<Color>();
    private int lastSection = -1;

    void Update () {
	    Vector3 tempInput = inputManager.RightPosition * (wheel.Radius + wheel.InnerPadding + wheel.Thickness - 1);
	    Vector2 currentPoint = new Vector2(tempInput.x, tempInput.y);
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
                    traveledSections.Add(currentSection);

                }
            }
            lastSection = currentSection;
        }


        while (traveledSections.Count > MaxTraveledSections)
        {
            traveledSections.RemoveAt(0);
        }
	}
}
