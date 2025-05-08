using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapData", menuName = "Data/MapData", order = 0)]
public class MapData : ScriptableObject
{

    [Serializable]
    public class ConnectionDestination
    {
        public List<int> ConnectionDestinations;
    }
    [SerializeField] private int MapIndex;
    [SerializeField] private string MapName;
    [SerializeField] private string MapDescription;

    /// <summary>
    /// Giao lộ trên map, được đặt id theo Index của list
    /// </summary>
    [SerializeField] private List<Vector3> Intersections;


    /// <summary>
    /// Đường đi giữa các giao lộ, Điểm thứ i có thể nối được đến những điểm nào
    /// Ví dụ: Connection[0] = {1, 2, 3} thì Giao lộ 0 có thể đi đến giao lộ 1, 2 và 3
    /// </summary>
    [SerializeField] private List<ConnectionDestination> Connections;


    public Vector3 GetIntersectionPos(int index) => Intersections[Mathf.Clamp(index, 0, Intersections.Count - 1)];
}
