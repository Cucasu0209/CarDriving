using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ObstacleData", menuName = "Data/ObstacleData", order = 0)]
public class ObstacleData : ScriptableObject
{
    [SerializeField] private ObstacleData Type;
    [SerializeField] private TraceData Trace;
}
public enum ObstacleType
{
    Car,
    Character
}