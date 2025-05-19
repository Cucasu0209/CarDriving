using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MoveableObject
{
    [SerializeField]
    public ObstacleData Data { get; private set; }
    protected GameObject CurrentModel;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnEndGame += OnEndGame;
        GameManager.Instance.OnRevive += OnRevive;

    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.OnEndGame -= OnEndGame;
        GameManager.Instance.OnRevive -= OnRevive;

    }

    private void OnRevive()
    {
        IsControllingVelocity = true;
        IsPositiveDir = true;
        TargetSpeed = Data.Speed;
        transform.position = Trace.GetStartPoint();
        if (Data.Type == ObstacleType.Character) GetComponentInChildren<Animator>().SetBool("Alive", true);

    }
    private void OnEndGame(bool isWin)
    {
        StopInstantly();
    }
    public virtual void SetObstacleData(ObstacleData data)
    {
        IsControllingVelocity = true;
        IsPositiveDir = true;
        Data = data;
        SetupTrace(Data.Trace);
        SpawnModel();
        TargetSpeed = data.Speed;
        if (Data.Type == ObstacleType.Character)
            GetComponentInChildren<Animator>().SetBool("Alive", true);
    }
    private void SpawnModel()
    {
        if (CurrentModel != null)
        {
            PoolingSystem.Despawn(CurrentModel);
        }

        GameObject model = null;
        if (Data.Type == ObstacleType.Car)
        {
            if (Random.Range(0, 1f) > 0.5f)
                model = Resources.Load<GameObject>(GameConfig.CAR_TRAP_LINK + "Car" + Random.Range(1, 9));
            else
                model = Resources.Load<GameObject>(GameConfig.SKIN_MODEL_LINK + "Car" + Random.Range(0, 9));
        }
        else if (Data.Type == ObstacleType.Character)
        {
            model = Resources.Load<GameObject>(GameConfig.HUMAN_MODEL_LINK + "Human" + Random.Range(1, 9));

        }
        if (model != null)
        {
            CurrentModel = PoolingSystem.Spawn(model, transform.position, Quaternion.identity);
            CurrentModel.transform.SetParent(transform);
            CurrentModel.transform.localRotation = Quaternion.Euler(0, 0, 0);
            CurrentModel.transform.localPosition = Vector3.zero;
            GetComponent<BoxCollider>().center = CurrentModel.GetComponent<BoxCollider>().center;
            GetComponent<BoxCollider>().size = CurrentModel.GetComponent<BoxCollider>().size;

        }
    }
    public override void OnHit(Vector3 dir)
    {
        if (Data.Type == ObstacleType.Car) ObjectBody.AddForce(dir.normalized * 700);
        else
        {
            ObjectBody.AddForce(dir.normalized * 400);
            transform.LookAt(transform.position - dir);
            GetComponentInChildren<Animator>().SetBool("Alive", false);
        }
    }
}
