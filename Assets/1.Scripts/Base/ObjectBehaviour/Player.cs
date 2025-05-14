using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MoveableObject
{
    [SerializeField] private PlayerConfig Config;
    [SerializeField] private LineRenderer RoadLine;
    [SerializeField] private Transform Door;
    [SerializeField] private GameObject ExploderFX;
    private bool PickedCustomerUp = false;
    private bool IsEndTrace = false;
    private bool Interactable = true;

    protected override void Start()
    {
        base.Start();
        UserInput.Instance.OnUserMouse += SetTargetSpeed;
        LevelManager.Instance.OnLoadLevelComplete += OnLevelComplete;

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        UserInput.Instance.OnUserMouse -= SetTargetSpeed;
        LevelManager.Instance.OnLoadLevelComplete -= OnLevelComplete;
    }
    private void OnLevelComplete()
    {
        SetupTrace(LevelManager.Instance.CurrentLevelData.PlayerTrace);
    }
    public override void SetupTrace(TraceData data)
    {
        base.SetupTrace(data);
        Interactable = true;
        IsEndTrace = false;
        PickedCustomerUp = false;
        IsControllingVelocity = true;
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
        GameManager.Instance.OnEndGame?.Invoke(true);
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


    private void OnTriggerEnter(Collider other)
    {
        if (other is null || Interactable == false) return;
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<MoveableObject>() != null)
        {
            MoveableObject obstacle = other.gameObject.GetComponent<MoveableObject>();
            obstacle.StopInstantly();
            obstacle.OnHit((obstacle.transform.position - transform.position));
            StopInstantly();
            OnHit(transform.position - obstacle.transform.position);
            GameObject Exploder = Instantiate(ExploderFX, (other.transform.position + transform.position) / 2, Quaternion.identity);
            Interactable = false;
            GameManager.Instance.OnEndGame?.Invoke(false);
            DOVirtual.DelayedCall(3, () =>
            {
                Destroy(Exploder);
            });
        }
    }

    public override void OnHit(Vector3 dir)
    {
        ObjectBody.AddForce(dir.normalized * 700);
    }
}
