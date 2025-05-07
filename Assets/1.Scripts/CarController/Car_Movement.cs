using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car_Movement : MonoBehaviour
{
    private Rigidbody CarBody;
    private float TargetSpeed;
    private float TargetAngle;
    private bool PickedCustomerUp = false;
    private bool IsEndTrace = false;
    private bool Interactable = true;
    [Header("Figures")]
    [SerializeField] private float MaxSpeed = 15;
    [SerializeField] private float Drag = 0.3f;
    [SerializeField] private float Throtlte = 0.3f;
    [SerializeField] private float RotateDamper = 0.2f;
    [Header("Car Road")]
    [SerializeField] private BezierCurveCreator CarTrace;
    [Header("Car")]
    [SerializeField] private Transform Door;
    private void Start()
    {
        CarBody = GetComponent<Rigidbody>();
        TargetSpeed = 0;
        transform.position = CarTrace.GetStartPoint();
    }
    private void Update()
    {
        if (Interactable)
        {
            if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameRunning == false) GameManager.Instance.StartGame();

            if (Input.GetMouseButton(0))
                TargetSpeed = Mathf.Min(TargetSpeed + MaxSpeed * Time.deltaTime / Throtlte, MaxSpeed);
            else
                TargetSpeed = Mathf.Max(TargetSpeed - MaxSpeed * Time.deltaTime / Drag, 0);
        }


        Vector3 normalizedDirection = (CarTrace.GetNextPoint(transform.position) - transform.position);
        normalizedDirection = new Vector3(normalizedDirection.x, 0, normalizedDirection.z);
        if (normalizedDirection.magnitude != 0) normalizedDirection = normalizedDirection.normalized;
        Move(normalizedDirection);
        RotateCarWhileMoving(normalizedDirection);
        CheckCanPickCustomerUp();
        CheckEndTrace();
    }
    /// <summary>
    ///Car chạy theo đường
    /// </summary>
    /// <param name="dir"></param>
    private void Move(Vector3 dir)
    {
        CarBody.velocity = Vector3.Lerp(CarBody.velocity, TargetSpeed * dir, RotateDamper);
        Debug.DrawRay(transform.position, dir * 10, Color.red);
    }

    /// <summary>
    /// Car xoay khi chạy tren đường (chỉ xoay chiều y)
    /// </summary>
    private void RotateCarWhileMoving(Vector3 dir)
    {
        if (dir.magnitude == 0) return;
        TargetAngle = Vector2.SignedAngle(new Vector2(dir.x, dir.z), Vector2.up);
        float currentAngle = (transform.rotation.eulerAngles.y + 360 + 180) % 360 - 180;
        if (Mathf.Abs(TargetAngle - currentAngle) > 180) currentAngle = currentAngle + (TargetAngle > currentAngle ? 360 : -360);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            Mathf.Lerp(currentAngle, TargetAngle, 0.1f),
            transform.rotation.z);

    }


    private void Break()
    {
        TargetSpeed = 0;
    }
    private void CheckCanPickCustomerUp()
    {
        if (PickedCustomerUp == false
            && CarTrace.HitPickupPoint(transform.position))
        {
            PickedCustomerUp = true;
            PickCustomerUp();
        }
    }
    private void PickCustomerUp()
    {
        GameManager.Instance.OnPickCustomer?.Invoke(Door);
        Interactable = false;
        Break();
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Interactable = true;
        });
    }

    private void CheckEndTrace()
    {
        if (IsEndTrace == false
            && CarTrace.CheckHitFinishPoint(transform.position))
        {
            IsEndTrace = true;
            FinishJourney();
        }
    }
    private void FinishJourney()
    {
        GameManager.Instance.OnFinishTrace?.Invoke(CarTrace.CustomerEndPos);
        Interactable = false;
        Break();
    }
}
