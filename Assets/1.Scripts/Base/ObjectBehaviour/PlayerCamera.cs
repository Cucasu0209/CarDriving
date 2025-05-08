using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Height from ground")] private float Height = 21;
    [SerializeField, Tooltip("Distance form behind")] private float Distance = 11;
    [SerializeField] private float Angle = 50;
    [SerializeField] private float Damping = 15f;

    void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + new Vector3(0, Height, -Distance), Time.deltaTime * Damping);
        Camera.main.transform.rotation = Quaternion.Euler(Angle, 0, 0);
    }
}
