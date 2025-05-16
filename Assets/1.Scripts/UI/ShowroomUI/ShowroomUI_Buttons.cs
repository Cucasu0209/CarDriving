using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_Buttons : MonoBehaviour
{
    [SerializeField] private Button PrevButton;
    [SerializeField] private Button NextButton;
    [SerializeField] private Button SelectButton;
    [SerializeField] private Image SelectedButton;
    [SerializeField] private Button UnlockButton;
    [SerializeField] private Button AdsButton;
    private void Start()
    {
        PrevButton.onClick.AddListener(OnPrevButtonClick);
        NextButton.onClick.AddListener(OnNextButtonClick);
        SelectButton.onClick.AddListener(OnSelectButtonClick);
        UnlockButton.onClick.AddListener(OnUnlockButtonClick);
        AdsButton.onClick.AddListener(OnAdsButtonClick);

        ShowroomManager.Instance.OnPageChange += OnPageChange;
        ShowroomManager.Instance.OnSelectElement += OnSelectOneElement;
        PlayerData.Instance.OnSkinChange += OnSelectOneElement;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnPageChange -= OnPageChange;
        ShowroomManager.Instance.OnSelectElement -= OnSelectOneElement;
        PlayerData.Instance.OnSkinChange -= OnSelectOneElement;

    }
    private void OnPrevButtonClick()
    {
        PrevButton.interactable = false;
        DOVirtual.DelayedCall(0.4f, () => PrevButton.interactable = true);
        ShowroomManager.Instance.ChangePage(false);
    }
    private void OnNextButtonClick()
    {
        NextButton.interactable = false;
        DOVirtual.DelayedCall(0.4f, () => NextButton.interactable = true);
        ShowroomManager.Instance.ChangePage(true);

    }
    private void OnSelectButtonClick()
    {
        PlayerData.Instance.UseSkin(ShowroomManager.Instance.CurrentIdSelected);
    }
    private void OnUnlockButtonClick()
    {
        ShowroomManager.Instance.OnWantToBuyCar?.Invoke(ShowroomManager.Instance.GetDataById(ShowroomManager.Instance.CurrentIdSelected));
    }
    private void OnAdsButtonClick()
    {
        PlayerData.Instance.AddMoney(500);

    }

    private void OnPageChange(bool isNext)
    {
        PrevButton.gameObject.SetActive(ShowroomManager.Instance.CurrentPageIndex > 0);
        NextButton.gameObject.SetActive(ShowroomManager.Instance.CurrentPageIndex < ShowroomManager.Instance.GetPageCount() - 1);
    }
    private void OnSelectOneElement()
    {
        SelectedButton.gameObject.SetActive(false);
        SelectButton.gameObject.SetActive(false);
        UnlockButton.gameObject.SetActive(false);

        if (ShowroomManager.Instance.CurrentIdSelected == PlayerData.Instance.CurrentSkinId)
        {
            SelectedButton.gameObject.SetActive(true);
        }
        else if (PlayerData.Instance.HaveSkin(ShowroomManager.Instance.CurrentIdSelected))
        {
            SelectButton.gameObject.SetActive(true);
        }
        else
        {
            UnlockButton.gameObject.SetActive(true);

        }
    }

}
