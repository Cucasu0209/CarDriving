using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData", order = 0)]

public class LevelData : ScriptableObject
{
    [Serializable]
    public class TrapPlace
    {
        public TrapType Trap;
        public List<Vector3> Positions;
    }
    public enum CameraDirection { Right, Left, Up, Down }
    public TraceData PlayerTrace;
    public List<Vector3> SafePoints;
    public CameraDirection CamDirection;
    [Header("Customer Infomation")]
    public Vector3 CustomerStartPoint;
    public Vector3 PickupPoint;
    public Vector3 FinalCustomerPoint;

    [Header("Traps")]
    public WeatherType Weather = WeatherType.Sunny;
    public List<TrapPlace> Traps;
    public List<ObstacleData> Obstacles;

    [Header("Reward")]
    public int Money = 200;
    public int RewardRate = 5;

    public Vector3 GetSafePoint(Vector3 position)
    {
        int posIndex = PlayerTrace.GetIndexByPosition(position);
        for (int i = 0; i < SafePoints.Count; i++)
        {
            if (PlayerTrace.GetIndexByPosition(SafePoints[i]) > posIndex)
            {
                return PlayerTrace.GetPointAtIndex(PlayerTrace.GetIndexByPosition(SafePoints[i]));
            }
        }
        return PlayerTrace.GetPointAtIndex(PlayerTrace.GetIndexByPosition(PlayerTrace.GetLastPoint()) - 6);
    }
    public Vector3 GetPickupPoint()
    {
        return PlayerTrace.GetPointAtIndex(PlayerTrace.GetIndexByPosition(PickupPoint));
    }
}
public enum TrapType
{
    Ice,
    Water,
}

public enum WeatherType
{
    Foggy,
    Sunny,
    Rainny
}