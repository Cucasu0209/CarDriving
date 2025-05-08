using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName ="Data/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float Throttle = 0.7f;
    public float Drag = 0.4f;
    public float MaxSpeed = 50;
}
