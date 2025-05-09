using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Map : MonoBehaviour
{
    [SerializeField] private Button MapButton;
    private float StartPosX;
    [SerializeField] private RectTransform MapPopup;
    [SerializeField] private Button BackButton;

    private void Start()
    {
        StartPosX = MapButton.GetComponent<RectTransform>().anchoredPosition.x;

        MapButton.onClick.AddListener(OnOpenMap);
        BackButton.onClick.AddListener(OnHideMap);
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
        MapButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX + 800, 0.3f);
    }
    private void OnShow()
    {
        MapButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX, 0.3f);

    }

    private void OnOpenMap()
    {
        GameManager.Instance.OnHideHomeUI?.Invoke();
        MapPopup.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        MapPopup.DOAnchorPosY(0, 0.6f).SetDelay(0.4f);
        BackButton.transform.DOScale(1, 0.3f).SetDelay(1f);
    }
    private void OnHideMap()
    {
        MapPopup.DOAnchorPosY(4500, 0.6f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            MapPopup.gameObject.SetActive(false);
        });
        BackButton.transform.DOScale(0, 0.3f).OnComplete(() => BackButton.gameObject.SetActive(false));
    }
}
