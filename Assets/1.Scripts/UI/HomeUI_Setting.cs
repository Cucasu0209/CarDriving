using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Setting : MonoBehaviour
{
    [SerializeField] private Button SettingButton;
    [SerializeField] private RectTransform SettingPopup;
    [SerializeField] private Image PopupDark;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button CancelButton;

    private void Start()
    {
        SettingButton.onClick.AddListener(OnOpenPopup);
        SaveButton.onClick.AddListener(SaveSetting);
        CancelButton.onClick.AddListener(OnClosePopup);

        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;
    }

    private void SaveSetting()
    {
        OnClosePopup();
    }
    private void OnHide()
    {
        SettingButton.transform.DOScale(0, 0.3f);
    }
    private void OnShow()
    {
        SettingButton.transform.DOScale(1, 0.3f);

    }

    private void OnOpenPopup()
    {
        GameManager.Instance.OnHideHomeUI?.Invoke();
        SettingPopup.gameObject.SetActive(true);
        PopupDark.gameObject.SetActive(true);
        SettingPopup.DOScale(1, 0.4f).SetDelay(0.4f);
        PopupDark.DOFade(0.8f, 0.3f).SetDelay(0.4f);
    }
    private void OnClosePopup()
    {
        SettingPopup.DOScale(0, 0.3f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            SettingPopup.gameObject.SetActive(false);
        });
        PopupDark.DOFade(0, 0.3f).OnComplete(() => PopupDark.gameObject.SetActive(false));
    }
}
