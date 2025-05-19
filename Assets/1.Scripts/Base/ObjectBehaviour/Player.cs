using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Player : MoveableObject
{
    #region Variables

    //referances
    [SerializeField] private PlayerConfig Config;
    [SerializeField] private LineRenderer RoadLine;


    [Header("Effect And Pos")]
    [SerializeField] private Transform Door;
    [SerializeField] private List<Transform> SkidMarkPos;
    [SerializeField] private TrailRenderer SkidMarkPrefab;
    [SerializeField] private GameObject ExploderFX;
    [SerializeField] private GameObject WindEffect;

    [Header("Sound")]
    [SerializeField] private AudioClip IdleSound;
    [SerializeField] private AudioClip BreakSound;
    [SerializeField] private AudioClip RunSound;
    [SerializeField] private AudioClip CrashSound;


    //in one session
    private bool PickedCustomerUp = false;
    private bool IsEndTrace = false;
    private bool Interactable = true;
    private GameObject CurrentModel;

    private int PickupPosIndex;
    private Vector3 PickupPos;

    private int LastTracePosIndex;
    private Vector3 LastTracePos;

    private List<CarWheels> Wheels;
    #endregion

    #region Unity Behaviours
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
    #endregion

    #region Events
    bool IsRunning = false;
    float TagetAngleBrake = 0;
    private void OnLevelComplete()
    {
        SetupTrace(LevelManager.Instance.CurrentLevelData.PlayerTrace);
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

                if (IsRunning == false)
                {
                    SoundManager.Instance.StopLoopSound(IdleSound);
                    SoundManager.Instance.PlayLoop(RunSound);
                    IsRunning = true;
                    StopSkidMark();
                    foreach (var wheel in Wheels) if (wheel != null) wheel.Run();
                }
            }
            else
            {
                TargetSpeed = Mathf.Max(TargetSpeed - Config.MaxSpeed * Time.deltaTime / Config.Drag, 0);
                if (IsRunning)
                {
                    SoundManager.Instance.PlayLoop(IdleSound);
                    SoundManager.Instance.StopLoopSound(RunSound);

                    IsRunning = false;
                    CreateSkidMark();
                    if ((ObjectBody.velocity.magnitude / Config.MaxSpeed) > 0.7f)
                    {
                        SoundManager.Instance.PlayEffect(BreakSound);
                        TagetAngleBrake = 20;
                    }
                    foreach (var wheel in Wheels) if (wheel != null) wheel.Stop();

                }

            }
        }
        TagetAngleBrake = Mathf.Max(TagetAngleBrake - Time.deltaTime * 30f, 0);
    }
    #endregion

    #region Initialize
    public override void SetupTrace(TraceData data)
    {
        base.SetupTrace(data);
        IsEndTrace = false;
        PickedCustomerUp = false;
        SetInteracableState(true);

        PickupPos = LevelManager.Instance.CurrentLevelData.PickupPoint;
        PickupPosIndex = Trace.GetIndexByPosition(PickupPos);

        LastTracePos = Trace.GetLastPoint();
        LastTracePosIndex = Trace.GetIndexByPosition(LastTracePos);

        GameManager.Instance.OnUpdatePickupPoint?.Invoke(Trace.GetIndexByPosition(LevelManager.Instance.CurrentLevelData.PickupPoint) * 1f / Points.Count);
        GameManager.Instance.OnUpdateProgress?.Invoke(0);
        LoadModel();
        StopSkidMark();
        StopSkidMark();
        IsRunning = false;
        IsDrifting = false;
        LastTimeDrift = 0;

        DOVirtual.DelayedCall(1, () => WindEffect.SetActive(true));
        VelocDirection = (Trace.GetPointAtIndex(Trace.GetIndexByPosition(transform.position) + 4) - transform.position);
        TargetAngle = Vector2.SignedAngle(new Vector2(VelocDirection.x, VelocDirection.z), Vector2.up);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            TargetAngle,
            transform.rotation.z);
    }
    private void LoadModel()
    {
        if (CurrentModel != null) PoolingSystem.Despawn(CurrentModel);
        GameObject Model = Resources.Load<GameObject>(GameConfig.SHOWROOM_MODEL_LINK + ShowroomManager.Instance.GetCarModel(PlayerData.Instance.CurrentSkinId).name);
        if (Model != null)
        {
            CurrentModel = PoolingSystem.Spawn(Model, transform.position, Quaternion.identity);
            CurrentModel.transform.SetParent(transform);
            CurrentModel.transform.localPosition = Vector3.zero;
            CurrentModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        if (Wheels != null) Wheels.Clear();
        Wheels = gameObject.GetComponentsInChildren<CarWheels>().ToList();
    }
    #endregion

    #region During Journey
    private bool IsDrifting = false;
    private float LastTimeDrift = 0;
    public override void Run()
    {
        base.Run();
        UpdateProgress();
        CheckCanPickCustomerUp();
        CheckEndTrace();

        float currentAngle = (CurrentModel.transform.localRotation.eulerAngles.z % 360 + 360) % 360;
        if (currentAngle > 180) currentAngle -= 360;
        float Angle = 0;
        Vector2 v = new Vector2(ObjectBody.velocity.x, ObjectBody.velocity.z);
        if (v.magnitude > 0.01f)
            Angle = Vector2.SignedAngle(v, new Vector2(VelocDirection.x, VelocDirection.z)) * v.magnitude / Config.MaxSpeed;


        if (IsDrifting == false && Mathf.Abs(Angle) > 15f && (v.magnitude / Config.MaxSpeed) > 0.7f)
        {
            IsDrifting = true;
            CreateSkidMark();
            LastTimeDrift = Time.time;
        }
        else if (IsDrifting && Time.time - LastTimeDrift > 0.3f)
        {
            IsDrifting = false;
            StopSkidMark();
        }
        // Debug.Log(CurrentModel.transform.localRotation.eulerAngles.x+ " "+ TagetAngleBrake);
        float xAngle = Mathf.Lerp(CurrentModel.transform.localRotation.eulerAngles.x, TagetAngleBrake, 0.1f);
        CurrentModel.transform.localRotation = Quaternion.Euler(xAngle, 0, Mathf.Lerp(currentAngle, -Angle, 0.2f));
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
    #endregion

    #region Checkpoints
    private void CheckCanPickCustomerUp()
    {
        if (PickedCustomerUp == false
            && Trace.GetIndexByPosition(transform.position) >= PickupPosIndex - 4)
        {
            PickedCustomerUp = true;
            PickCustomerUp();
        }
    }
    private void PickCustomerUp()
    {
        

        IsRunning = false;
        SetInteracableState(false);
        StopInstantly();
        CurrentModel.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.2f);
        transform.DOMove(new Vector3(PickupPos.x, transform.position.y, PickupPos.z), 0.4f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SoundManager.Instance.StopLoopSound(RunSound);
            SoundManager.Instance.PlayEffect(IdleSound);
            GameManager.Instance.OnPickCustomer?.Invoke(Door);
            foreach (var wheel in Wheels) if (wheel != null) wheel.Stop();
            UpdateProgress();
        });
        DOVirtual.DelayedCall(1.5f, () =>
        {
            SetInteracableState(true);

        });
    }
    #endregion

    #region Check Win
    private void CheckEndTrace()
    {
        if (IsEndTrace == false
            && Trace.GetIndexByPosition(transform.position) >= LastTracePosIndex - 5)
        {
            IsEndTrace = true;
            FinishJourney();
        }
    }
    private void FinishJourney()
    {
       

        SetInteracableState(false);
        StopInstantly();
        CurrentModel.transform.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.2f);
        transform.DOMove(new Vector3(LastTracePos.x, transform.position.y, LastTracePos.z), 0.4f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SoundManager.Instance.StopLoopSound(RunSound);
            SoundManager.Instance.PlayEffect(IdleSound);
            WindEffect.SetActive(false);
            UpdateProgress();
            GameManager.Instance.OnFinishTrace?.Invoke();
            GameManager.Instance.OnUpdateProgress?.Invoke(1);
            GameManager.Instance.OnEndGame?.Invoke(true);
            foreach (var wheel in Wheels) if (wheel != null) wheel.Stop();
        });

    }
    #endregion

    #region Check Lose (hit or be hit)
    private void OnTriggerEnter(Collider other)
    {
        if (other is null || Interactable == false) return;
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<MoveableObject>() != null)
        {
            StopSkidMark();

            Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();

            obstacle.StopInstantly();
            obstacle.OnHit((obstacle.transform.position - transform.position));
            StopInstantly();
            Interactable = false;
            foreach (var wheel in Wheels) if (wheel != null) wheel.Stop();

            if (obstacle.Data.Type == ObstacleType.Car)
            {
                OnHit(transform.position - obstacle.transform.position);
                GameObject Exploder = Instantiate(ExploderFX, (other.transform.position + transform.position) / 2, Quaternion.identity);

                DOVirtual.DelayedCall(3, () =>
                {
                    Destroy(Exploder);
                });
            }
            SoundManager.Instance.StopLoopSound(RunSound);
            SoundManager.Instance.StopLoopSound(IdleSound);
            SoundManager.Instance.PlayEffect(CrashSound);

            GameManager.Instance.OnEndGame?.Invoke(false);
            WindEffect.SetActive(false);


        }
    }
    public override void OnHit(Vector3 dir)
    {
        ObjectBody.AddForce(dir.normalized * 700);
    }
    #endregion

    #region State

    private void SetInteracableState(bool canCantrol)
    {
        Interactable = canCantrol;
        IsControllingVelocity = canCantrol;
    }

    #endregion

    #region SkidMark
    private int SkidMarkCount = 0;
    private List<TrailRenderer> SkidMarks;
    private void CreateSkidMark()
    {
        SkidMarkCount++;
        if (SkidMarkCount > 1) return;
        SkidMarks = new List<TrailRenderer>();
        for (int i = 0; i < SkidMarkPos.Count; i++)
        {
            TrailRenderer newSkidMark = Instantiate(SkidMarkPrefab, SkidMarkPos[i].position, Quaternion.identity);
            newSkidMark.emitting = true;
            SkidMarks.Add(newSkidMark);
            newSkidMark.transform.SetParent(SkidMarkPos[i]);
            Destroy(newSkidMark.gameObject, 5);
        }


    }
    private void StopSkidMark()
    {

        if (SkidMarkCount > 0) SkidMarkCount--;
        if (SkidMarkCount > 0) return;
        if (SkidMarks != null)
            for (int i = 0; i < SkidMarks.Count; i++)
            {
                if (SkidMarks[i] != null)
                    SkidMarks[i].emitting = false;
            }
    }
    #endregion
}
