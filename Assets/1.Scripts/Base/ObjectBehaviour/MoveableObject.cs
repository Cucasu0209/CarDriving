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
    protected Vector3 VelocDirection;
    protected float TargetSpeed;
    protected float TargetAngle;
    protected bool IsControllingVelocity = true;
    protected bool IsPositiveDir = true;
    [SerializeField] private float VelocDamper = 0.01f;
    [SerializeField] private float AngleDamper = 0.2f;
    #endregion

    #region Unity Behabiours
    protected virtual void Start()
    {
        ObjectBody = GetComponent<Rigidbody>();
    }
    protected virtual void FixedUpdate()
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
        VelocDirection = (Trace.GetPointAtIndex(Trace.GetIndexByPosition(transform.position) + (IsPositiveDir ? 4 : -4)) - transform.position);
        VelocDirection = new Vector3(VelocDirection.x, 0, VelocDirection.z);
        if (VelocDirection.magnitude > 0.001f) VelocDirection = VelocDirection.normalized;
        ObjectBody.velocity = (new Vector3(Mathf.Lerp(transform.forward.x, VelocDirection.x, 0.5f),
            0,
            Mathf.Lerp(transform.forward.z, VelocDirection.z, VelocDamper))).normalized * TargetSpeed;

   
        LineDebug(VelocDirection);

        //Set Angle
        //ObjectBody.angularVelocity = Vector3.up * 20;
        if (Vector3.Distance(Trace.GetPointAtIndex(Trace.GetIndexByPosition(transform.position) + (IsPositiveDir ? 4 : -4)), transform.position) > 2)
        {
            TargetAngle = Vector2.SignedAngle(new Vector2(VelocDirection.x, VelocDirection.z), Vector2.up);
            float currentAngle = (transform.rotation.eulerAngles.y + 360 + 180) % 360 - 180;
            if (Mathf.Abs(TargetAngle - currentAngle) > 180) currentAngle = currentAngle + (TargetAngle > currentAngle ? 360 : -360);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
              Mathf.Lerp(currentAngle, TargetAngle, AngleDamper),
            transform.rotation.z);
        }
        else if (Trace.LoopType == TraceData.TraceLoopType.Restart)
        {
            transform.position = Trace.GetStartPoint();
        }
        else if (Trace.LoopType == TraceData.TraceLoopType.Yoyo)
        {
            IsPositiveDir = !IsPositiveDir;
        }


    }

    protected void LineDebug(Vector3 Direction)
    {
        Debug.DrawRay(transform.position, Direction.normalized * 10, Color.blue);
        Debug.DrawRay(transform.position, ObjectBody.velocity.normalized * 10, Color.red);

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
