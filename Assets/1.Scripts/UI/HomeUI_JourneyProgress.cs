using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_JourneyProgress : MonoBehaviour
{
    [SerializeField] private RectTransform BodySelf;
    [SerializeField] private Image FilledImage;

    [Header("Points")]
    [SerializeField] private RectTransform StartPoint;
    [SerializeField] private RectTransform PickupPoint;
    [SerializeField] private GameObject Pickup_Pin;
    [SerializeField] private GameObject Pickup_Flag;
    [SerializeField] private RectTransform EndPoint;
    [SerializeField] private GameObject EndPoint_Pin;
    [SerializeField] private GameObject EndPoint_Flag;
    float PickupPosRate = 0;
    private void Start()
    {
        GameManager.Instance.OnGameStart += ShowProgress;
        GameManager.Instance.OnUpdateProgress += UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint += SetPickupPoint;

        GameManager.Instance.OnPickCustomer += UpdateIconPickup;
        GameManager.Instance.OnFinishTrace += UpdateIconEndPoint;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= ShowProgress;
        GameManager.Instance.OnUpdateProgress -= UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint -= SetPickupPoint;

        GameManager.Instance.OnPickCustomer -= UpdateIconPickup;
        GameManager.Instance.OnFinishTrace -= UpdateIconEndPoint;
    }
    private void UpdateIconPickup(Transform door)
    {
        Pickup_Pin.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            Pickup_Pin.SetActive(false);
            Pickup_Flag.SetActive(true);
            Pickup_Flag.transform.DOScale(1, 0.3f);
        });
    }

    private void UpdateIconEndPoint()
    {
        EndPoint_Pin.transform.DOScale(0, 0.3f).OnComplete(() =>
        {
            EndPoint_Pin.SetActive(false);
            EndPoint_Flag.SetActive(true);
            EndPoint_Flag.transform.DOScale(1, 0.3f);
        });
    }
    private void UpdateProgress(float rate)
    {
        FilledImage.fillAmount = rate;
    }

    private void SetPickupPoint(float rate)
    {
        PickupPosRate = rate;
        PickupPoint.anchoredPosition = (EndPoint.anchoredPosition - StartPoint.anchoredPosition) * rate + StartPoint.anchoredPosition;
    }

    private void ShowProgress()
    {
        BodySelf.DOAnchorPosY(-412, 0.4f).SetDelay(0.2f);
    }
}
