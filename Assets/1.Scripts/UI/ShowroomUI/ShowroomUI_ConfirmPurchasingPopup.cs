using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_ConfirmPurchasingPopup : MonoBehaviour
{
    [SerializeField] private Image Grey;
    [SerializeField] private RectTransform Popup;

    [SerializeField] private TextMeshProUGUI Message;
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;
    private CarData Data;



    private void Start()
    {
        YesButton.onClick.AddListener(OnClickYes);
        NoButton.onClick.AddListener(OnClickNo);
        ShowroomManager.Instance.OnWantToBuyCar += SetPopup;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnWantToBuyCar -= SetPopup;
    }
    private void OnClickYes()
    {
        ShowroomManager.Instance.BuyCar(Data);
        ClosePopup();
    }
    private void OnClickNo()
    {
        ClosePopup();
    }
    private void SetPopup(CarData data)
    {
        if (PlayerData.Instance.HaveEnoughMoney(data.Price))
        {
            Data = data;
            Message.SetText($"Are you sure to use {data.Price} cash to buy this car?");
            OpenPopup();
        }
    }

    private void OpenPopup()
    {
        Grey.gameObject.SetActive(true);
        Popup.gameObject.SetActive(true);
        Grey.DOFade(0.7f, 0.2f);
        Popup.DOScale(1, 0.2f);
    }
    private void ClosePopup()
    {
        Grey.DOFade(0f, 0.2f).OnComplete(() =>
        {
            Grey.gameObject.SetActive(false);

        });
        Popup.DOScale(0, 0.2f).OnComplete(() =>
        {
            Popup.gameObject.SetActive(false);
        });
    }
}
