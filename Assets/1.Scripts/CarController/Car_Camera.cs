using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Car_Movement))]
public class Car_Camera : MonoBehaviour
{
    [SerializeField] private float Height = 20;
    [SerializeField] private float Distance = 15;
    [SerializeField] private float Angle = 45;
    [SerializeField] private float Damping = 5.0f;

    void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + new Vector3(0, Height, -Distance), Time.deltaTime * Damping);   
       // Camera.main.transform.position = transform.position + new Vector3(0, Height, -Distance);
        Camera.main.transform.rotation = Quaternion.Euler(Angle, 0, 0);
    }
}
