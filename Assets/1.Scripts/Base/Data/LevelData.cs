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
    public CameraDirection CamDirection;
    [Header("Customer Infomation")]
    public Vector3 CustomerStartPoint;
    public Vector3 PickupPoint;
    public Vector3 FinalCustomerPoint;

    [Header("Traps")]
    public List<TrapPlace> Traps;
    public List<ObstacleData> Obstacles;

    [Header("Reward")]
    public int Money = 200;
    public int RewardRate = 5;
}
public enum TrapType
{
    Ice,
    Water,
}