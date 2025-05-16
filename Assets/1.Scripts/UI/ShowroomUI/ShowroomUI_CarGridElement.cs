
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_CarGridElement : MonoBehaviour
{
    [SerializeField] private Image CarIcon;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private Image PriceIcon;

    [SerializeField] private RectTransform UsedMark;
    [SerializeField] private Image BackgroundSelected;
    [SerializeField] private Button ElButton;

    private CarData CurrentData;
    private void Start()
    {
        ElButton.onClick.AddListener(Select);
        ShowroomManager.Instance.OnSelectElement += OnOneElementSelected;
        PlayerData.Instance.OnSkinChange += OnUseCarSkin;
        PlayerData.Instance.OnSkinUnlocked += OnOneSkinUnlock;
    }

    private void OnDestroy()
    {
        ShowroomManager.Instance.OnSelectElement -= OnOneElementSelected;
        PlayerData.Instance.OnSkinChange -= OnUseCarSkin;
        PlayerData.Instance.OnSkinUnlocked -= OnOneSkinUnlock;

    }

    public void SetData(CarData data)
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        CurrentData = data;
        CarIcon.sprite = ShowroomManager.Instance.GetCarIcon(CurrentData.Id);
        OnOneElementSelected();
        OnUseCarSkin();
        OnOneSkinUnlock();
    }

    private void Select()
    {
        if (CurrentData.Id != ShowroomManager.Instance.CurrentIdSelected)
        {
            ShowroomManager.Instance.SelectElement(CurrentData.Id);
        }
        else if (PlayerData.Instance.HaveSkin(ShowroomManager.Instance.CurrentIdSelected))
        {
            PlayerData.Instance.UseSkin(ShowroomManager.Instance.CurrentIdSelected);
        }
        else
        {
            ShowroomManager.Instance.OnWantToBuyCar?.Invoke(ShowroomManager.Instance.GetDataById(ShowroomManager.Instance.CurrentIdSelected));
        }
    }

    private void OnOneElementSelected()
    {
        BackgroundSelected.gameObject.SetActive(CurrentData != null && CurrentData.Id == ShowroomManager.Instance.CurrentIdSelected);
    }
    private void OnUseCarSkin()
    {
        UsedMark.gameObject.SetActive(CurrentData != null && CurrentData.Id == PlayerData.Instance.CurrentSkinId);
    }
    private void OnOneSkinUnlock()
    {
        PriceIcon.gameObject.SetActive(PlayerData.Instance.HaveSkin(CurrentData.Id) == false);
        if (PlayerData.Instance.HaveSkin(CurrentData.Id) == false) Price.SetText(CurrentData.Price.ToString());


    }
}
