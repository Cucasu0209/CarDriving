using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ShowroomData", menuName = "Data/ShowroomData", order = 0)]
public class ShowroomData : ScriptableObject
{
    public List<CarData> CarListData;
}
