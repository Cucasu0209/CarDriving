using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MoveableObject
{
    protected ObstacleData Data;
    protected GameObject CurrentModel;

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
            model = Resources.Load<GameObject>(GameConfig.CAR_MODEL_LINK + "Car" + Random.Range(0, 11));
        }
        else if (Data.Type == ObstacleType.Character)
        {
            model = Resources.Load<GameObject>(GameConfig.HUMAN_MODEL_LINK + "Human" + Random.Range(0, 8));

        }
        if (model != null)
        {
            CurrentModel = Instantiate(model, transform);
            CurrentModel.transform.localPosition = Vector3.zero;
        }
    }
    public override void OnHit(Vector3 dir)
    {
        if (Data.Type == ObstacleType.Car) ObjectBody.AddForce(dir.normalized * 700);
        else GetComponentInChildren<Animator>().SetTrigger("Die");
    }
}
