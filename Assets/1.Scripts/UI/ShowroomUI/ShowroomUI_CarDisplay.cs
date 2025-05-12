using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShowroomUI_CarDisplay : MonoBehaviour
{
    private int CurrentIdDisplayed = -1;
    [SerializeField] private Transform CarHolder;
    [SerializeField] private float AngleVelocity = 35;
    private GameObject CurrentCar;
    private void Start()
    {
        ShowroomManager.Instance.OnSelectElement += OnCarSelected;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnSelectElement += OnCarSelected;
    }

    private void OnCarSelected()
    {
        if (CurrentIdDisplayed != ShowroomManager.Instance.CurrentIdSelected)
        {
            CurrentIdDisplayed = ShowroomManager.Instance.CurrentIdSelected;

            GameObject CarModel = ShowroomManager.Instance.GetCarModel(CurrentIdDisplayed);
            if (CarModel != null)
            {
                if (CurrentCar != null) Destroy(CurrentCar);
                CurrentCar = Instantiate(CarModel, CarHolder);
                CurrentCar.transform.localPosition = Vector3.zero;
            }

        }
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + AngleVelocity * Time.deltaTime, 0);
    }
}
