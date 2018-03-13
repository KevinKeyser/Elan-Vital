using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(RectTransform))]
public class CircleSplitter : MonoBehaviour
{
    public GameObject PrefabSlice;
    public float radius;
    RectTransform rectTransform;
    public Image Image;
    public int splitAmount;
    private GameObject[] children = new GameObject[0];

    void OnValidate()
    {
        radius = Mathf.Max(.01f, radius);
    }

    void Start()
    {
        CreateChildren();
    }

    private void CreateChildren()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius * 2);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius * 2);
        float currentTriangleDeg = 0;
        float currentSplitDegrees = 360f / splitAmount;
        children = new GameObject[splitAmount];
        for (int i = 0; i < splitAmount; i++)
        {
            float x = Mathf.Cos(currentTriangleDeg * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(currentTriangleDeg * Mathf.Deg2Rad) * radius;
            Vector2 direction = new Vector2(x, y);
            children[i] = Instantiate(PrefabSlice, transform.position + new Vector3(direction.x, direction.y), Quaternion.Euler(0, 0, currentTriangleDeg), this.transform);
            currentTriangleDeg += currentSplitDegrees;
        }
    }

   
}
