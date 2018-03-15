using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Travel : MonoBehaviour
{
    public Transform TravelPoints;
    public float Speed;
    private List<Transform> points;
    private int target = 0;
    private Rigidbody body;

    // Use this for initialization
    void Start()
    {
        points = new List<Transform>();
        foreach (Transform child in TravelPoints.transform)
        {
            points.Add(child);
        }
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (body)
        {
            float step = Speed * Time.deltaTime;
            body.position = Vector3.MoveTowards(transform.position, points[target].position, step);

            if (Vector3.Distance(transform.position, points[target].position) < 1)
            {
                target++;
                if (target >= points.Count) target = 0;
            }
        }
    }
}
