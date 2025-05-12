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

    protected override void Start()
    {
        base.Start();
        UserInput.Instance.OnUserMouse += SetTargetSpeed;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UserInput.Instance.OnUserMouse -= SetTargetSpeed;

    }
    public override void SetupTrace(TraceData data)
    {
        base.SetupTrace(data);
        Interactable = true;
        IsEndTrace = false;
        PickedCustomerUp = false;
        GameManager.Instance.OnUpdatePickupPoint?.Invoke(Trace.GetIndexByPosition(LevelManager.Instance.CurrentLevelData.PickupPoint) * 1f / Points.Count);
        GameManager.Instance.OnUpdateProgress?.Invoke(0);
        LoadModel();
    }
    public override void Run()
    {
        base.Run();
        UpdateProgress();
        CheckCanPickCustomerUp();
        CheckEndTrace();
    }
    private void SetTargetSpeed(bool isUserDragging)
    {
        if (Interactable)
        {
            if (isUserDragging)
            {
                TargetSpeed = Mathf.Min(TargetSpeed + Config.MaxSpeed * Time.deltaTime / Config.Throttle, Config.MaxSpeed);
                if (GameManager.Instance.IsGameRunning == false)
                    GameManager.Instance.StartGame();
            }
            else
                TargetSpeed = Mathf.Max(TargetSpeed - Config.MaxSpeed * Time.deltaTime / Config.Drag, 0);
        }
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
            && Vector3.Distance(transform.position, LevelManager.Instance.CurrentLevelData.PickupPoint) < 2f)
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
        DOVirtual.DelayedCall(5f, () =>
        {
            GameManager.Instance.OnShowEndgamePopup?.Invoke(true);
        });
    }
    private void LoadModel()
    {
        GameObject Model = Resources.Load<GameObject>(GameConfig.SHOWROOM_MODEL_LINK + ShowroomManager.Instance.GetCarModel(PlayerData.Instance.CurrentSkinId).name);
        if (Model != null)
        {
            Model = Instantiate(Model, transform);
            Model.transform.localPosition = Vector3.zero;
        }
    }

}
