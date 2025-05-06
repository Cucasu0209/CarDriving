using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BezierCurveCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> Points;
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private float CornerGap = 2;
    private List<BezierCurveKnot> Knots;
    void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        LineRenderer.positionCount = Points.Count;
        LineRenderer.SetPositions(Points.Select(_ => _.position).ToArray());
    }

}
public class BezierCurveKnot
{
    public Vector3 Position;
    public Vector3 Rotation;
    public float TangentLength;
}