using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Car_Movement : MonoBehaviour
{
    private Rigidbody CarBody;

    private void Start()
    {
        CarBody = GetComponent<Rigidbody>();
    }

}
