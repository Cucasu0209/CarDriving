using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CityBuilding : MonoBehaviour
{
    private Renderer Renderer;
    [SerializeField] private Material NormalMat;
    [SerializeField] private Material BlurMat;
    private bool LastCondition = false;
    private void Start()
    {
        Renderer = GetComponent<Renderer>();
        GameManager.Instance.OnCameraMove += OnCameraMove;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnCameraMove += OnCameraMove;
    }

    private void OnCameraMove(Vector3 camPos, Vector3 playerPos)
    {
        //Vector3 endPos = new Vector3((camPos.x + playerPos.x) / 2, playerPos.y, (camPos.z + playerPos.z) / 2);
        Ray ray = new Ray(camPos, (playerPos - camPos).normalized);

        bool condition = (Renderer.bounds.IntersectRay(ray, out float distance));

        if (LastCondition != condition)
        {
            Renderer.material = condition ? BlurMat : NormalMat;
            LastCondition = condition;
        }
    }

}
