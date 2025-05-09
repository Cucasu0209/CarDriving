using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Banner : MonoBehaviour
{
    [SerializeField] private RectTransform BodySelf;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI LocationName;
    [SerializeField] private TextMeshProUGUI TitleNextLocation;

    [SerializeField] private TextMeshProUGUI TapToPlayTest;
    [SerializeField] private Image Dark;
    private float StartPosY;
    private void Start()
    {
        StartPosY = BodySelf.anchoredPosition.y;
        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;

    }
    private void OnHide()
    {
        float duration = 0.4f;
        BodySelf.DOAnchorPosY(StartPosY + 2500, duration);
        TapToPlayTest.DOFade(0, duration);
        Dark.DOFade(0, duration);
    }
    private void OnShow()
    {
        float duration = 0.4f;
        BodySelf.DOAnchorPosY(StartPosY, duration);
        TapToPlayTest.DOFade(1, duration);
        Dark.DOFade(0.6f, duration);
    }
}
