using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;

public class Player : MoveableObject
{
    [SerializeField] private PlayerConfig Config;
    [SerializeField] private LineRenderer RoadLine;
    [SerializeField] private Transform Door;
    private bool PickedCustomerUp = false;
    private bool IsEndTrace = false;
    private bool Interactable = true;

    public override void SetupTrace(TraceData data)
    {
        base.SetupTrace(data);
        GameManager.Instance.OnUpdatePickupPoint?.Invoke(Trace.GetIndexByPosition(LevelManager.Instace.CurrentLevelData.PickupPoint) * 1f / Points.Count);
    }
    public override void Run()
    {
        if (Interactable)
        {
            if (Input.GetMouseButtonDown(0) && GameManager.Instance.IsGameRunning == false) GameManager.Instance.StartGame();

            if (Input.GetMouseButton(0))
                TargetSpeed = Mathf.Min(TargetSpeed + Config.MaxSpeed * Time.deltaTime / Config.Throttle, Config.MaxSpeed);
            else
                TargetSpeed = Mathf.Max(TargetSpeed - Config.MaxSpeed * Time.deltaTime / Config.Drag, 0);
        }
        base.Run();
        UpdateProgress();
        CheckCanPickCustomerUp();
        CheckEndTrace();
    }
    private void UpdateProgress()
    {
        //Draw Line renerder
        int NextPoint = Trace.GetIndexByPosition(transform.position);
        RoadLine.positionCount = Points.Count - NextPoint;
        for (int i = NextPoint; i < Points.Count; i++)
        {
            RoadLine.SetPosition(i - NextPoint, Points[i]);
        }

        //Update UI

        GameManager.Instance.OnUpdateProgress?.Invoke((NextPoint) * 1f / Points.Count);

    }


    private void CheckCanPickCustomerUp()
    {
        if (PickedCustomerUp == false
            && Vector3.Distance(transform.position, LevelManager.Instace.CurrentLevelData.PickupPoint) < 2f)
        {
            PickedCustomerUp = true;
            PickCustomerUp();
        }
    }
    private void PickCustomerUp()
    {
        GameManager.Instance.OnPickCustomer?.Invoke(Door);
        Interactable = false;
        Stop();
        DOVirtual.DelayedCall(1.5f, () =>
        {
            Interactable = true;
        });
    }
    private void CheckEndTrace()
    {
        if (IsEndTrace == false
            && Trace.CheckHitFinishPoint(transform.position))
        {
            IsEndTrace = true;
            FinishJourney();
        }
    }
    private void FinishJourney()
    {
        GameManager.Instance.OnFinishTrace?.Invoke();
        GameManager.Instance.OnUpdateProgress?.Invoke(1);
        Interactable = false;
        Stop();
    }
}
