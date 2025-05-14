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
    private void Start()
    {
        GameManager.Instance.OnGameStart += ShowProgress;
        GameManager.Instance.OnEndGame += HideProgress;

        GameManager.Instance.OnUpdateProgress += UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint += SetPickupPoint;

        GameManager.Instance.OnPickCustomer += UpdateIconPickup;
        GameManager.Instance.OnFinishTrace += UpdateIconEndPoint;

        GameManager.Instance.OnSetupGame += SetupPin;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= ShowProgress;
        GameManager.Instance.OnEndGame -= HideProgress;


        GameManager.Instance.OnUpdateProgress -= UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint -= SetPickupPoint;

        GameManager.Instance.OnPickCustomer -= UpdateIconPickup;
        GameManager.Instance.OnFinishTrace -= UpdateIconEndPoint;

        GameManager.Instance.OnSetupGame -= SetupPin;
    }
    private void SetupPin()
    {
        Pickup_Pin.transform.DOScale(1, 0.1f).OnComplete(() => Pickup_Pin.SetActive(true));
        Pickup_Flag.transform.DOScale(0, 0.1f).OnComplete(() => Pickup_Flag.SetActive(false));
        EndPoint_Pin.transform.DOScale(1, 0.1f).OnComplete(() => EndPoint_Pin.SetActive(true));
        EndPoint_Flag.transform.DOScale(0, 0.1f).OnComplete(() => EndPoint_Flag.SetActive(false));
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
        PickupPoint.anchoredPosition = (EndPoint.anchoredPosition - StartPoint.anchoredPosition) * rate + StartPoint.anchoredPosition;
    }

    private void ShowProgress()
    {
        BodySelf.DOAnchorPosY(-242, 0.4f).SetDelay(0.2f);
    }
    private void HideProgress(bool isWin)
    {
        BodySelf.DOAnchorPosY(242, 0.4f);

    }
}
