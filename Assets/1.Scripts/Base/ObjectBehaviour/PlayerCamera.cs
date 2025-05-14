using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Height from ground")] private float Height = 21;
    [SerializeField, Tooltip("Distance form behind")] private float Distance = 11;
    [SerializeField] private float Angle = 50;
    [SerializeField] private float Damping = 15f;
    private Vector3 Direction = Vector3.forward;
    private float YAngle = 0;

    private void Start()
    {
        LevelManager.Instance.OnLoadLevelComplete += UpdateDirection;
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnLoadLevelComplete += UpdateDirection;
    }

    private void UpdateDirection()
    {
        switch (LevelManager.Instance.CurrentLevelData.CamDirection)
        {
            case LevelData.CameraDirection.Right:
                Direction = Vector3.right;
                YAngle = 90;
                break;
            case LevelData.CameraDirection.Up:
                Direction = Vector3.forward;
                YAngle = 0;
                break;
            case LevelData.CameraDirection.Left:
                Direction = Vector3.left;
                YAngle = -90;
                break;
            case LevelData.CameraDirection.Down:
                Direction = Vector3.back;
                YAngle = 180;
                break;
        }
        Camera.main.transform.rotation = Quaternion.Euler(Angle, YAngle, 0);

    }
    void Update()
    {
        if (LevelManager.Instance.CurrentLevelData != null)
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
                       transform.position + Vector3.up * Height - Direction * Distance, Time.deltaTime * Damping);

            GameManager.Instance.OnCameraMove?.Invoke(Camera.main.transform.position, transform.position);

        }

    }
}
