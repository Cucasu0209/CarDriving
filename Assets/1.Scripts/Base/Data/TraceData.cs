using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TraceData", menuName = "Data/TraceData", order = 0)]
public class TraceData : ScriptableObject
{
    public enum TraceLoopType
    {
        Yoyo,
        Restart,
        None
    }
    /// <summary>
    /// Đường đi các điểm trong map
    /// </summary>
    [SerializeField] private List<Vector3> TracePoints;
    [SerializeField] public TraceLoopType LoopType;


    #region Draw Bezier line Algorithm
    private List<Vector3> LinePoints;
    public List<Vector3> GetBezierTrace()
    {
        float CornerGap = 2;
        float PointGap = 1;//Khoảng cách lớn nhất giữa 2 điểm trên line
        Vector3 pP, cP, nP;// preveous Point, current Point, next Point
        float minGap;

        List<Vector3> Points = new List<Vector3>();

        for (int i = 0; i < TracePoints.Count; i++)
        {
            Points.Add(TracePoints[i]);
        }

        List<Vector3> Knots = new List<Vector3>();
        List<bool> KnotAnchorMarks = new List<bool>();

        //Từ các điểm(điểm đầu, cuối, rẽ) tạo ra các knot để vẽ đường bezier
        // Xử lý thành Knot
        for (int i = 0; i < Points.Count; i++)
        {
            if (i == 0 || i == Points.Count - 1)
            {
                Knots.Add(Points[i]);
                KnotAnchorMarks.Add(false);
            }
            else
            {
                pP = Points[i - 1];
                cP = Points[i];
                nP = Points[i + 1];
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

        // Vẽ đường Bezier từ các knot
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

        // Vẽ lại đường sao cho các điểm chỉ cách nhau PointGap (cho việc tính lộ trình Hiển thị đường)
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
        return LinePoints;
    }
    #endregion

    #region Handle From outside Positions
    public int GetIndexByPosition(Vector3 position)
    {
        float MinDis = 9999;
        int resultIndex = 0;
        position = new Vector3(position.x, LinePoints[0].y, position.z);
        for (int i = 0; i < LinePoints.Count; i++)
        {
            if (Vector3.Distance(position, LinePoints[i]) < MinDis)
            {
                MinDis = Vector3.Distance(position, LinePoints[i]);
                resultIndex = i;
            }
        }
        return resultIndex;
    }
    public Vector3 GetNextPoint(Vector3 position, bool isPositiveDir = true)
    {
        int resultIndex = GetIndexByPosition(position);
        position = new Vector3(position.x, LinePoints[0].y, position.z);
        if (LoopType == TraceLoopType.Yoyo && isPositiveDir == false)
        {
            if (Vector3.Distance(position, GetStartPoint()) < 1) return position;
            return resultIndex - 1 < 0 ? position : LinePoints[resultIndex - 1];
        }
        else
        {
            if (Vector3.Distance(position, GetLastPoint()) < 1) return position;
            return resultIndex + 1 > LinePoints.Count - 1 ? position : LinePoints[resultIndex + 1];
        }
    }
    public Vector3 GetStartPoint()
    {
        return LinePoints[0];
    }
    public Vector3 GetLastPoint()
    {
        return LinePoints[LinePoints.Count - 1];
    }
    public List<Vector3> GetIntersectionList() => TracePoints;
    public bool CheckHitFinishPoint(Vector3 position)
    {
        return Vector3.Distance(position, GetLastPoint()) < 1;
    }
    #endregion
}
