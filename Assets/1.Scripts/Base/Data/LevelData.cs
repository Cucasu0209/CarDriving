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
    [SerializeField] private TraceData PlayerTrace;
    [SerializeField] private Vector3 PickupPoint;
    [SerializeField] private List<TrapPlace> Traps;
    [SerializeField] private List<ObstacleData> Obstacles;
}
public enum TrapType
{
    Ice,
    Water,
}