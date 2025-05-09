using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_MapButton : MonoBehaviour
{
    [SerializeField] private Button MapButton;
    private float StartPosX;
    [SerializeField] private HomeUI_MapPopup MapPopup;
 

    private void Start()
    {
        StartPosX = MapButton.GetComponent<RectTransform>().anchoredPosition.x;

        MapButton.onClick.AddListener(OnClick);
        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;

    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;

    }
    private void OnClick()
    {
        MapPopup.ShowPopup();
    }

    private void OnHide()
    {
        MapButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX + 800, 0.3f);
    }
    private void OnShow()
    {
        MapButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX, 0.3f);

    }

   
}
