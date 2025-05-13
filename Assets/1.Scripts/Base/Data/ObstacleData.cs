using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ObstacleData", menuName = "Data/ObstacleData", order = 0)]
public class ObstacleData : ScriptableObject
{
    public float Speed;
    public ObstacleType Type;
    public TraceData Trace;
}
public enum ObstacleType
{
    Car,
    Character
}