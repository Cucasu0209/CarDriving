using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveableObject : MonoBehaviour
{
    #region Variables
    protected Rigidbody ObjectBody;
    protected TraceData Trace;
    protected List<Vector3> Points;
    protected float TargetSpeed;
    protected float TargetAngle;
    protected bool IsControllingVelocity = true;
    private bool isPositiveDir = true;
    #endregion

    #region Unity Behabiours
    protected virtual void Start()
    {
        ObjectBody = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        if (Trace != null && IsControllingVelocity) Run();
    }
    protected virtual void OnDestroy()
    {

    }
    #endregion

    public virtual void SetupTrace(TraceData data)
    {
        Trace = data;
        Points = Trace.GetBezierTrace();
        transform.position = Trace.GetStartPoint();
    }
    public virtual void Run()
    {

        // Set Velocity
        Vector3 normalizedDirection = (Trace.GetNextPoint(transform.position, isPositiveDir) - transform.position);
        normalizedDirection = new Vector3(normalizedDirection.x, 0, normalizedDirection.z);
        if (normalizedDirection.magnitude < 0.001f) normalizedDirection = normalizedDirection.normalized;
        ObjectBody.velocity = new Vector3(Mathf.Lerp(ObjectBody.velocity.x, TargetSpeed * normalizedDirection.x, 0.5f),
            ObjectBody.velocity.y,
            Mathf.Lerp(ObjectBody.velocity.z, TargetSpeed * normalizedDirection.z, 0.5f));

        LineDebug();

        //Set Angle
        //ObjectBody.angularVelocity = Vector3.up * 20;
        if (normalizedDirection.magnitude > 0.001f)
        {
            TargetAngle = Vector2.SignedAngle(new Vector2(normalizedDirection.x, normalizedDirection.z), Vector2.up);
            float currentAngle = (transform.rotation.eulerAngles.y + 360 + 180) % 360 - 180;
            if (Mathf.Abs(TargetAngle - currentAngle) > 180) currentAngle = currentAngle + (TargetAngle > currentAngle ? 360 : -360);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
              Mathf.Lerp(currentAngle, TargetAngle, 0.1f),
            transform.rotation.z);
        }
        else if (Trace.LoopType == TraceData.TraceLoopType.Restart)
        {
            transform.position = Trace.GetStartPoint();
        }
        else if (Trace.LoopType == TraceData.TraceLoopType.Yoyo)
        {
            isPositiveDir = !isPositiveDir;
        }


    }

    protected void LineDebug()
    {
        Debug.DrawRay(transform.position, (Trace.GetNextPoint(transform.position, isPositiveDir) - transform.position).normalized * 10, Color.blue);

        List<Vector3> points = Trace.GetIntersectionList();
        for (int i = 0; i < points.Count - 1; i++)
        {
            Debug.DrawLine(points[i], points[i + 1]);
        }
    }
    public virtual void Stop()
    {
        TargetSpeed = 0;
    }
    public virtual void StopInstantly()
    {
        TargetSpeed = 0;
        ObjectBody.velocity = Vector3.zero;
        ObjectBody.angularVelocity = Vector3.zero;
        IsControllingVelocity = false;
    }
    public virtual void OnHit(Vector3 dir)
    {
        ObjectBody.AddForce(dir.normalized);
    }
}
