using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierCurveCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> Points;
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private float CornerGap = 2;
    private List<Vector3> LinePoints;
    private List<Vector3> Knots;
    private List<bool> KnotAnchorMarks;//Is Anchor Knot



    void Update()
    {
        UpdateKnots();
        UpdateLine();
    }

    private void UpdateLine()
    {
        LinePoints = new List<Vector3>();
        for (int i = 0; i < Knots.Count - 1; i++)
        {

            if (KnotAnchorMarks[i + 1] == false)
            {
                //Chỉ nối vì nó k phải đường bezier
                LinePoints.Add(Knots[i]);
                LinePoints.Add(Knots[i + 1]);
            }
            else
            {
                //Tạo đường bezier với Index i,i+1, i+2
                pP = Knots[i];
                cP = Knots[i + 1];
                nP = Knots[i + 2];
                for (float t = 0.1f; t < 1; t += 0.1f)
                {
                    LinePoints.Add((1 - t) * ((1 - t) * pP + t * cP) + t * ((1 - t) * cP + t * nP));
                }
                i++;
            }
        }
        //
        LineRenderer.positionCount = LinePoints.Count;
        LineRenderer.SetPositions(LinePoints.ToArray());
    }

    float minGap;
    Vector3 pP, cP, nP;// preveous Point, current Point, next Point
    private void UpdateKnots()
    {
        //Loại các Point thẳng hàng

        // Xử lý thành Knot
        Knots = new List<Vector3>();
        KnotAnchorMarks = new List<bool>();
        for (int i = 0; i < Points.Count; i++)
        {
            if (i == 0 || i == Points.Count - 1)
            {
                Knots.Add(Points[i].position);
                KnotAnchorMarks.Add(false);
            }
            else
            {
                pP = Points[i - 1].position;
                cP = Points[i].position;
                nP = Points[i + 1].position;
                minGap = Mathf.Min(Vector3.Distance(pP, cP), Vector3.Distance(cP, nP)) / 2;
                minGap = Mathf.Min(minGap, CornerGap);

                Knots.Add(cP + (pP - cP) * minGap / Vector3.Distance(pP, cP));
                Knots.Add(cP);
                Knots.Add(cP + (nP - cP) * minGap / Vector3.Distance(nP, cP));

                KnotAnchorMarks.Add(false);
                KnotAnchorMarks.Add(true);
                KnotAnchorMarks.Add(false);
            }

        }
    }
}