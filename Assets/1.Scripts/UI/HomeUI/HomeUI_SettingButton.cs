using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_SettingButton : MonoBehaviour
{
    [SerializeField] private Button SettingButton;
    [SerializeField] private HomeUI_SettingPopup SettingPopup;
  

    private void Start()
    {
        SettingButton.onClick.AddListener(OpenPopup);
      

        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;
    }
    private void OpenPopup()
    {
        SettingPopup.ShowPopup();
    }
   
    private void OnHide()
    {
        SettingButton.transform.DOScale(0, 0.3f);
    }
    private void OnShow()
    {
        SettingButton.transform.DOScale(1, 0.3f);

    }

  
}
