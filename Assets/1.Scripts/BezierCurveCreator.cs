using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveCreator : MonoBehaviour
{
    [SerializeField] private Transform CustomerStartPos;
    [SerializeField] public Transform CustomerEndPos;
    [SerializeField] private Transform PickupPoint;
    [Header("Line Drawing")]
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private float PointGap = 0.3f;//Khoảng cách lớn nhất giữa 2 điểm trên line

    [Header("Point Data Setup")]
    [SerializeField] private List<Transform> Points;
    [SerializeField] private float CornerGap = 2;
    private List<Vector3> LinePoints;
    private List<Vector3> Knots;
    private List<bool> KnotAnchorMarks;//Is Anchor Knot
    private int StartPointCount = 0;
    float minGap;
    Vector3 pP, cP, nP;// preveous Point, current Point, next Point

    void Awake()
    {
        UpdateKnots();
        UpdateLine();
        RedrawLine();
    }

    /// <summary>
    /// Từ các điểm (điểm đầu, cuối, rẽ) tạo ra các knot để vẽ đường bezier
    /// </summary>
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

    /// <summary>
    /// Vẽ đường Bezier từ các knot
    /// </summary>
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

    }

    /// <summary>
    /// Vẽ lại đường sao cho các điểm chỉ cách nhau PointGap (cho việc tính lộ trình Hiển thị đường)
    /// </summary>
    private void RedrawLine()
    {
        float currentAbundance = 0;
        int pNumber = 0;
        float distance = 0;
        List<Vector3> Lines = new List<Vector3>();
        Lines.Add(LinePoints[0]);
        for (int i = 1; i < LinePoints.Count; i++)
        {
            distance = Vector3.Distance(LinePoints[i - 1], LinePoints[i]);
            pNumber = Mathf.FloorToInt((distance + currentAbundance) / PointGap);
            for (int j = 1; j <= pNumber; j++)
            {
                Lines.Add((LinePoints[i] - LinePoints[i - 1]) / distance * (j * PointGap - currentAbundance) + LinePoints[i - 1]);
            }
            currentAbundance = distance + currentAbundance - pNumber * PointGap;
        }
        Lines.Add(LinePoints[LinePoints.Count - 1]);

        LinePoints = Lines;
        StartPointCount = LinePoints.Count;
        LineRenderer.positionCount = LinePoints.Count;
        LineRenderer.SetPositions(LinePoints.ToArray());





    }
    private IEnumerator Start()
    {
        yield return null;
        int NearistPickupPointIndex = 0;
        float NearistPickupPointDistance = 9999;
        for (int i = 0; i < LinePoints.Count; i++)
        {
            if (Vector3.Distance(LinePoints[i], PickupPoint.position) < NearistPickupPointDistance)
            {
                NearistPickupPointDistance = Vector3.Distance(LinePoints[i], PickupPoint.position);
                NearistPickupPointIndex = i;
            }
        }
        GameManager.Instance.OnUpdatePickupPoint?.Invoke((NearistPickupPointIndex + 1) * 1f / StartPointCount);
    }
    public Vector3 GetNextPoint(Vector3 position)
    {
        float MinDis = 9999;
        int resultIndex = 0;
        for (int i = 0; i < LinePoints.Count; i++)
        {
            if (Vector3.Distance(position, LinePoints[i]) < MinDis)
            {
                MinDis = Vector3.Distance(position, LinePoints[i]);
                resultIndex = i;
            }
        }
        for (int i = 0; i < resultIndex; i++)
        {
            LinePoints.RemoveAt(0);
        }
        GameManager.Instance.OnUpdateProgress(1 - LinePoints.Count * 1f / StartPointCount);
        LineRenderer.positionCount = LinePoints.Count;
        LineRenderer.SetPositions(LinePoints.ToArray());
        if (LinePoints.Count < 2) return position;
        return LinePoints[1];
    }
    public Vector3 GetStartPoint()
    {
        return LinePoints[0];
    }
    public bool HitPickupPoint(Vector3 position)
    {
        return Vector3.Distance(PickupPoint.position, position) < 2f;
    }
    public bool CheckHitFinishPoint(Vector3 position)
    {
        return Vector3.Distance(Points[Points.Count - 1].position, position) < 1f;
    }
}