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
    [SerializeField] private RectTransform EndPoint;
    float PickupPosRate = 0;
    private void Start()
    {
        GameManager.Instance.OnGameStart += ShowProgress;
        GameManager.Instance.OnUpdateProgress += UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint += SetPickupPoint;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= ShowProgress;
        GameManager.Instance.OnUpdateProgress -= UpdateProgress;
        GameManager.Instance.OnUpdatePickupPoint -= SetPickupPoint;
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
