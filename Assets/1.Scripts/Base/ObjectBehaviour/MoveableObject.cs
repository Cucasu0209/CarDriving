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
    #endregion

    #region Unity Behabiours
    protected virtual void Start()
    {
        ObjectBody = GetComponent<Rigidbody>();
    }
    protected virtual void Update()
    {
        if (Trace != null) Run();
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
        Vector3 normalizedDirection = (Trace.GetNextPoint(transform.position) - transform.position);
        normalizedDirection = new Vector3(normalizedDirection.x, 0, normalizedDirection.z);
        if (normalizedDirection.magnitude != 0) normalizedDirection = normalizedDirection.normalized;
        ObjectBody.velocity = new Vector3(Mathf.Lerp(ObjectBody.velocity.x, TargetSpeed * normalizedDirection.x, 0.5f),
            ObjectBody.velocity.y,
            Mathf.Lerp(ObjectBody.velocity.z, TargetSpeed * normalizedDirection.z, 0.5f));
        Debug.DrawRay(transform.position, normalizedDirection * 10, Color.red);

        //Set Angle
        //ObjectBody.angularVelocity = Vector3.up * 20;
        if (normalizedDirection.magnitude > 0)
        {
            TargetAngle = Vector2.SignedAngle(new Vector2(normalizedDirection.x, normalizedDirection.z), Vector2.up);
            float currentAngle = (transform.rotation.eulerAngles.y + 360 + 180) % 360 - 180;
            if (Mathf.Abs(TargetAngle - currentAngle) > 180) currentAngle = currentAngle + (TargetAngle > currentAngle ? 360 : -360);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
              Mathf.Lerp(currentAngle, TargetAngle, 0.1f),
            transform.rotation.z);
        }

    }
    public virtual void Stop()
    {
        TargetSpeed = 0;
    }
}
