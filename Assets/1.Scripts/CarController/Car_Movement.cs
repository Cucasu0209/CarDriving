using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Car_Movement : MonoBehaviour
{
    private Rigidbody CarBody;
    private float TargetSpeed;
    [SerializeField] private float MaxSpeed = 15;
    [SerializeField] private SplineAnimate Animate;
    [SerializeField] private SplineExtrude Extrude;
    [SerializeField] private SplineContainer Container;
    [SerializeField] private float Drag = 0.3f;
    private void Start()
    {
        CarBody = GetComponent<Rigidbody>();
        Animate.MaxSpeed = 1;
        TargetSpeed = 0;

    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            TargetSpeed = Mathf.Min(TargetSpeed + 100 * Time.deltaTime, MaxSpeed);

        }
        else
        {
            TargetSpeed = Mathf.Max(TargetSpeed - Drag * 100 * Time.deltaTime, 0);

        }
        //Animate.MaxSpeed = Mathf.Lerp(Animate.MaxSpeed, TargetSpeed, 0.1f);
        Animate.ElapsedTime += TargetSpeed * Time.deltaTime;
        Extrude.Range = new Vector2(Animate.NormalizedTime, 1);
        //Debug.Log(Animate.ElapsedTime);
    }

}
