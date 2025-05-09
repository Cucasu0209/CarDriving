using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_SettingPopup : MonoBehaviour
{
    [SerializeField] private RectTransform SettingPopup;
    [SerializeField] private Image PopupDark;
    [SerializeField] private Button SaveButton;
    [SerializeField] private Button CancelButton;

    private void Start()
    {
        SaveButton.onClick.AddListener(SaveSetting);
        CancelButton.onClick.AddListener(HidePopup);
    }
    private void SaveSetting()
    {
        HidePopup();
    }
    public void ShowPopup()
    {
        GameManager.Instance.OnHideHomeUI?.Invoke();
        SettingPopup.gameObject.SetActive(true);
        PopupDark.gameObject.SetActive(true);
        SettingPopup.DOScale(1, 0.4f).SetDelay(0.4f);
        PopupDark.DOFade(0.8f, 0.3f).SetDelay(0.4f);
    }
    private void HidePopup()
    {
        SettingPopup.DOScale(0, 0.3f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            SettingPopup.gameObject.SetActive(false);
        });
        PopupDark.DOFade(0, 0.3f).OnComplete(() => PopupDark.gameObject.SetActive(false));
    }
}
