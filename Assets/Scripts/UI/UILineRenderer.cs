#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace ElanVital.UI
{
    public class UILineRenderer : Graphic
    {
        [Range(1, 250)]
        [SerializeField] public int MaxPoints = 10;

        public float LineThickness = 1;
        [SerializeField]
        private List<Vector2> points = new List<Vector2>();
        private Vector2[] verts = new Vector2[0];


        public void AddPoint(Vector2 point)
        {
            points.Add(point);
            if (points.Count > MaxPoints)
            {
                points.RemoveAt(0);
            } 
            SetVerticesDirty();
        }

        protected override void OnValidate()
        {
            LineThickness = Mathf.Max(.01f, LineThickness);
            base.OnValidate();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (points.Count < 2)
                return;

            verts = new Vector2[points.Count * 2];

            UIVertex vert = UIVertex.simpleVert;
            vert.color = color;

            Vector2 lastNormal = Vector2.zero;
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 v1 = points[i];
                Vector2 direction = lastNormal;

                if (i != points.Count - 1) //Find normal average with next point in line
                {
                    Vector2 v2 = points[i + 1];
                    Vector2 vDif = v2 - v1;

                    direction = (vDif.normalized + lastNormal) / 2;
                    direction.Normalize();
                    lastNormal = vDif.normalized;
                }

                Vector2 leftNormal = new Vector2(-direction.y, direction.x);
                Vector2 rightNormal = -leftNormal;

                Vector2 v1Left = v1 + leftNormal * LineThickness / 2;
                Vector2 v1Right = v1 + rightNormal * LineThickness / 2;

                verts[i * 2] = v1Left;
                verts[i * 2 + 1] = v1Right;

                vert.position = v1Left;
                vh.AddVert(vert);

                vert.position = v1Right;
                vh.AddVert(vert);
            }

            for (int i = 0; i < points.Count - 1; i++)
            {
                int index = i * 2;
                vh.AddTriangle(index, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            for (int i = 0; i < verts.Length; i++)
            {
                Gizmos.DrawSphere(verts[i], .5f);
            }
        }
    }
}