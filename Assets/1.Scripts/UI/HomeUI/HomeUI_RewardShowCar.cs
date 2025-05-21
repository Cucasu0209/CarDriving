using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI_RewardShowCar : MonoBehaviour
{
    private int CurrentIdDisplayed = -1;
    [SerializeField] private Transform CarHolder;
    [SerializeField] private float AngleVelocity = 35;
    private GameObject CurrentCar;

    public void ShowCar(int id)
    {
        if (CurrentIdDisplayed != id)
        {
            CurrentIdDisplayed = id;

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
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + AngleVelocity * Time.deltaTime, transform.rotation.eulerAngles.z);
    }
}
