using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_ShowroomPopup : MonoBehaviour
{
    [SerializeField] private RectTransform Background1;
    [SerializeField] private RectTransform Background2;
    [SerializeField] private Transform CarHolder;
    [SerializeField] private Button CloseButton;

    private void Start()
    {
        CloseButton.onClick.AddListener(ClosePopup);
    }
    public void OpenPopup()
    {
        GameManager.Instance.OnHideHomeUI?.Invoke();
        Background1.gameObject.SetActive(true);
        Background2.gameObject.SetActive(true);
        CarHolder.gameObject.SetActive(true);
        Background1.DOAnchorPosY(0, 0.4f).SetEase(Ease.Linear).SetDelay(0.4f);
        Background2.DOAnchorPosY(0, 0.4f).SetEase(Ease.Linear).SetDelay(0.4f);
        CarHolder.DOScale(1, 0.2f).SetDelay(0.6f);
        DOVirtual.DelayedCall(0.1f, () =>
        {
            ShowroomManager.Instance.OnLoadDataComplete?.Invoke();
            ShowroomManager.Instance.OnPageChange?.Invoke(true);
            ShowroomManager.Instance.OnSelectElement?.Invoke();
        });

    }
    public void ClosePopup()
    {
        Background1.DOAnchorPosY(2000, 0.4f).SetEase(Ease.Linear);
        Background2.DOAnchorPosY(2000, 0.4f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Background1.gameObject.SetActive(false);
            Background2.gameObject.SetActive(false);
            CarHolder.gameObject.SetActive(false);
            GameManager.Instance.OnShowHomeUI?.Invoke();
        });
        CarHolder.DOScale(0, 0.2f);
    }
}
