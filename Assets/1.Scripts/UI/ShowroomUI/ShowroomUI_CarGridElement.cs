using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_CarGridElement : MonoBehaviour
{
    [SerializeField] private Image CarIcon;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private Image Dark;

    [SerializeField] private Image BackgroundUsed;
    [SerializeField] private Image BackgroundSelected;
    [SerializeField] private Button ElButton;

    private CarData CurrentData;
    private void Start()
    {
        ElButton.onClick.AddListener(Select);
        ShowroomManager.Instance.OnSelectElement += OnOneElementSelected;
        PlayerData.Instance.OnSkinChange += OnUseCarShin;
        PlayerData.Instance.OnSkinUnlocked += OnOneSkinUnlock;
    }

    private void OnDestroy()
    {
        ShowroomManager.Instance.OnSelectElement -= OnOneElementSelected;
        PlayerData.Instance.OnSkinChange -= OnUseCarShin;
        PlayerData.Instance.OnSkinUnlocked -= OnOneSkinUnlock;

    }

    public void SetData(CarData data)
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        CurrentData = data;
        CarIcon.sprite = ShowroomManager.Instance.GetCarIcon(CurrentData.Id);
        CarIcon.SetNativeSize();
        OnOneElementSelected();
        OnUseCarShin();
        OnOneSkinUnlock();
    }

    private void Select()
    {
        ShowroomManager.Instance.SelectElement(CurrentData.Id);
    }

    private void OnOneElementSelected()
    {
        if (CurrentData != null && CurrentData.Id == ShowroomManager.Instance.CurrentIdSelected)
        {
            BackgroundSelected.DOFade(1, 0.2f);
        }
        else
        {
            BackgroundSelected.DOFade(0, 0.2f);
        }
    }
    private void OnUseCarShin()
    {
        if (CurrentData != null && CurrentData.Id == PlayerData.Instance.CurrentSkinId)
        {
            BackgroundUsed.DOFade(1, 0.2f);
        }
        else
        {
            BackgroundUsed.DOFade(0, 0.2f);
        }
    }
    private void OnOneSkinUnlock()
    {
        //Price.gameObject.SetActive(PlayerData.Instance.HaveSkin(CurrentData.Id) == false);
        Dark.DOFade(PlayerData.Instance.HaveSkin(CurrentData.Id) ? 0 : 0.5f, 0.2f);
        if (PlayerData.Instance.HaveSkin(CurrentData.Id) == false) Price.SetText(CurrentData.Price.ToString());
        else Price.SetText(CurrentData.CarName);

    }
}
