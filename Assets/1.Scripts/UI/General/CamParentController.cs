using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamParentController : MonoBehaviour
{
    [SerializeField] private Transform NormalMode;
    [SerializeField] private Transform ShowroomMode;
    [SerializeField] private Transform ReceiveRewardMode;
    void Start()
    {
        GameManager.Instance.OnChangeCameraMode += OnCameraChangeMode;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnChangeCameraMode -= OnCameraChangeMode;

    }

    private void OnCameraChangeMode()
    {
        switch (GameManager.Instance.CurrentCameraMode)
        {
            case CameraMode.Gameplay:
                Camera.main.transform.SetParent(NormalMode);
                break;
            case CameraMode.Showroom:
                Camera.main.transform.SetParent(ShowroomMode);
                break;
            case CameraMode.Reward:
                Camera.main.transform.SetParent(ReceiveRewardMode);
                break;
        }
    }
}
