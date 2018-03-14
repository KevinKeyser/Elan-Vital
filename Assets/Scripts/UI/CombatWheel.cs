#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace ElanVital.UI
{
    public class CombatWheel : MaskableGraphic
    {
        [Range(-360, 360)] public float DegreeOffset = 0;
        [Range(1, 100)] public int Radius = 10;
        [Range(1, 50)] public int Thickness = 10;
        [Range(0, 50)] public int Padding = 0;
        [Range(0, 50)] public int InnerPadding = 0;
        [Range(10, 100)] public int Detail = 10;

        [SerializeField]
        [Range(1, 10)]
        private int SectionCount = 1;
        [SerializeField]
        private Color[] SectionColors;

        public void SetSectionColor(int sectionNumber, Color newColor)
        {
            SectionColors[sectionNumber] = newColor;
        }
            
        protected override void Awake()
        {
            Canvas.willRenderCanvases += () => {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (Radius + Thickness + InnerPadding) * 2);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (Radius + Thickness + InnerPadding) * 2);
            };
            SectionColors = new Color[SectionCount + 1];
            for (int i = 0; i < SectionColors.Length; i++)
            {
                SectionColors[i] = this.color;
            }
            base.Awake();
        }
       

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            //SectionColors = new Color[SectionCount];
            float perSectionDegree = (360f - Padding * SectionCount) / SectionCount;

            //float degreeIncrease = 360f / Detail;
            float currentDegree = -perSectionDegree / 2;
            UIVertex vert = UIVertex.simpleVert;
            //vert.color = Color.black;
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
                    vert.position = new Vector2(x, y) * (Radius + InnerPadding);
                    vert.color = SectionColors[sectionNumber + 1];
                    vh.AddVert(vert);
                    vert.position = new Vector2(x, y) * (Radius + InnerPadding + Thickness);
                    vert.color = SectionColors[sectionNumber + 1];
                    vh.AddVert(vert);
                    currentSectionDegree += sectionDegreeIncrease;
                }

                //Force Create Last Detail point at end of each arc piece
                x = Mathf.Cos((DegreeOffset + currentDegree + perSectionDegree) * Mathf.Deg2Rad);
                y = Mathf.Sin((DegreeOffset + currentDegree + perSectionDegree) * Mathf.Deg2Rad);
                vert.position = new Vector2(x, y) * (Radius + InnerPadding);
                vert.color = SectionColors[sectionNumber + 1];
                vh.AddVert(vert);
                vert.position = new Vector2(x, y) * (Radius + InnerPadding + Thickness);
                vert.color = SectionColors[sectionNumber + 1];
                vh.AddVert(vert);

                for (int i = sectionNumber * Detail; i < (sectionNumber + 1) * Detail - 1; i++)
                {
                    var index = i * 2;
                    vh.AddTriangle(index, index + 1, index + 2);
                    vh.AddTriangle(index + 2, index + 1, index + 3);
                }

                currentDegree += perSectionDegree + Padding;
            }

            int outerSectionVertEnd = vh.currentVertCount;
            vert.color = SectionColors[0];
            vert.position = Vector2.zero;
            vh.AddVert(vert);
            currentDegree = 0f;
            perSectionDegree = 360f / Detail;
            for (int i = 0; i < Detail; i++)
            {
                float x = Mathf.Cos(currentDegree * Mathf.Deg2Rad) * Radius;
                float y = Mathf.Sin(currentDegree * Mathf.Deg2Rad) * Radius;
                vert.position = new Vector2(x, y);
                vh.AddVert(vert);
                currentDegree += perSectionDegree;
            }

            for (int i = 0; i < Detail - 1; i++)
            {
                int index = outerSectionVertEnd + 1 + i;
                vh.AddTriangle(index, outerSectionVertEnd, index + 1);
            }
            vh.AddTriangle(vh.currentVertCount - 1, outerSectionVertEnd, outerSectionVertEnd + 1);
        }
    }
}