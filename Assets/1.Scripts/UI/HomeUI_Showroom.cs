using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Showroom : MonoBehaviour
{
    [SerializeField] private Button ShowroomButton;
    private float StartPosX;
    private void Start()
    {
        StartPosX = ShowroomButton.GetComponent<RectTransform>().anchoredPosition.x;
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
        ShowroomButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX - 800, 0.3f);
    }
    private void OnShow()
    {
        ShowroomButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX, 0.3f);

    }
}
