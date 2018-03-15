using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel : MonoBehaviour
{
    public Transform TravelPoints;
    public float Speed;
    private List<Transform> points;
    private int target = 0;

    // Use this for initialization
    void Start()
    {
        points = new List<Transform>();
        foreach (Transform child in TravelPoints.transform)
        {
            points.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step = Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, points[target].position, step);

        if (Vector3.Distance(transform.position, points[target].position) < 1)
        {
            target++;
            if (target >= points.Count) target = 0;
        }
    }
}
