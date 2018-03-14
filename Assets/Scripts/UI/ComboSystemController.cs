using System.Collections;
using System.Collections.Generic;
using ElanVital.UI;
using UnityEngine;

public class ComboSystemController : MonoBehaviour
{
    [SerializeField]
    private CombatWheel wheel;
    [SerializeField]
    private UILineRenderer lineRenderer;

    public Stack<int> traveledSections = new Stack<int>();
    //depth
    [SerializeField]
    private List<Color> ComboColors = new List<Color>();
    private int lastSection = -1;

    void Update () {
		//get point from inputmanager
	    Vector3 tempInput = Vector3.zero;
	    Vector2 currentPoint = new Vector2(tempInput.x, tempInput.y);
        if (lineRenderer)
        {
            lineRenderer.Points.Add(currentPoint);
        }

        if (wheel)
        {
            int currentSection = wheel.SectionContains(currentPoint);
            if (currentSection != -1)
            {
                if (lastSection != currentSection)
                {
                    traveledSections.Push(currentSection);

                }
            }
            lastSection = currentSection;
        } 


	}
}
