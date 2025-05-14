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
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.OnEndGame -= OnEndGame;

    }
    private void OnEndGame(bool isWin)
    {
        StopInstantly();
    }
    public virtual void SetObstacleData(ObstacleData data)
    {
        Data = data;
        SetupTrace(Data.Trace);
        SpawnModel();
        TargetSpeed = data.Speed;
    }
    private void SpawnModel()
    {
        GameObject model = null;
        if (Data.Type == ObstacleType.Car)
        {
            model = Resources.Load<GameObject>(GameConfig.CAR_MODEL_LINK + "Car" + Random.Range(1, 9));
        }
        else if (Data.Type == ObstacleType.Character)
        {
            model = Resources.Load<GameObject>(GameConfig.HUMAN_MODEL_LINK + "Human" + Random.Range(1, 9));

        }
        if (model != null)
        {
            CurrentModel = Instantiate(model, transform);
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
            GetComponentInChildren<Animator>().SetTrigger("Die");
        }
    }
}
