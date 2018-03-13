#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace ElanVital.UI
{
    public class CombatWheel : Graphic
    {
        [Range(-360, 360)] public float DegreeOffset = 0;

        [Range(1, 100)] public int Radius = 10;
        [Range(1, 50)] public int Thickness = 10;
        [Range(1, 10)] public int SectionCount = 1;
        [Range(0, 10)] public int Padding = 0;

        [Range(10, 100)] public int Detail = 10;

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            float perSectionDegree = (360f - Padding * SectionCount) / SectionCount;

            //float degreeIncrease = 360f / Detail;
            float currentDegree = -perSectionDegree / 2;
            UIVertex vert = UIVertex.simpleVert;
            vert.color = Color.black;
            for (int sectionNumber = 0; sectionNumber < SectionCount; sectionNumber++)
            {
                float sectionDegreeIncrease = perSectionDegree / (Detail - 1);
                float currentSectionDegree = 0;
                float x;
                float y;
                for (int i = 0; i < Detail - 1; i++)
                {
                    x = Mathf.Cos((DegreeOffset + currentSectionDegree + currentDegree) * Mathf.Deg2Rad);
                    y = Mathf.Sin((DegreeOffset + currentSectionDegree + currentDegree) * Mathf.Deg2Rad);
                    vert.position = new Vector2(x, y) * Radius;
                    //vert.color = Random.ColorHSV();
                    vh.AddVert(vert);
                    vert.position = new Vector2(x, y) * (Radius + Thickness);
                    //vert.color = Random.ColorHSV();
                    vh.AddVert(vert);
                    currentSectionDegree += sectionDegreeIncrease;
                }

                //Force Create Last Detail point at end of arc piece
                x = Mathf.Cos((DegreeOffset + currentDegree + perSectionDegree) * Mathf.Deg2Rad);
                y = Mathf.Sin((DegreeOffset + currentDegree + perSectionDegree) * Mathf.Deg2Rad);
                vert.position = new Vector2(x, y) * Radius;
                //vert.color = Random.ColorHSV();
                vh.AddVert(vert);
                vert.position = new Vector2(x, y) * (Radius + Thickness);
                //vert.color = Random.ColorHSV();
                vh.AddVert(vert);

                for (int i = sectionNumber * Detail; i < (sectionNumber + 1) * Detail - 1; i++)
                {
                    var index = i * 2;
                    vh.AddTriangle(index, index + 1, index + 2);
                    vh.AddTriangle(index + 2, index + 1, index + 3);
                }

                currentDegree += perSectionDegree + Padding;
            }
        }

        public override void Rebuild(CanvasUpdate update)
        {
            if(update == CanvasUpdate.PreRender)
            {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (Radius + Thickness) * 2);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (Radius + Thickness) * 2);
            }
            base.Rebuild(update);
        }
    }
}