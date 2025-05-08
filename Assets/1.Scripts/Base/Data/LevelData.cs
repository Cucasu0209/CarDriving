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
    public TraceData PlayerTrace;
    public Vector3 PickupPoint;
    public Vector3 FinalCustomerPoint;
    public List<TrapPlace> Traps;
    public List<ObstacleData> Obstacles;
}
public enum TrapType
{
    Ice,
    Water,
}