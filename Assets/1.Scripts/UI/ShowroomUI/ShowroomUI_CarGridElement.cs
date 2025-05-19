
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_CarGridElement : MonoBehaviour
{
    [SerializeField] private Image CarIcon;
    [SerializeField] private Image BackgroundSelected;
    [SerializeField] private Button ElButton;

    [Header("Bottom Button")]
    [SerializeField] private Button HandlerButton;
    [SerializeField] private RectTransform UsedMark;
    [SerializeField] private RectTransform UseMark;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private Image PriceIcon;
    [SerializeField] private Sprite BlueBtn, GreenBtn, GreyBtn;

    private CarData CurrentData;
    private void Start()
    {
        ElButton.onClick.AddListener(Select);
        HandlerButton.onClick.AddListener(HandleElement);
        ShowroomManager.Instance.OnSelectElement += UpdateState;
        PlayerData.Instance.OnSkinChange += UpdateState;
        PlayerData.Instance.OnSkinUnlocked += UpdateState;
    }

    private void OnDestroy()
    {
        ShowroomManager.Instance.OnSelectElement -= UpdateState;
        PlayerData.Instance.OnSkinChange -= UpdateState;
        PlayerData.Instance.OnSkinUnlocked -= UpdateState;

    }

    public void SetData(CarData data)
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        CurrentData = data;
        CarIcon.sprite = ShowroomManager.Instance.GetCarIcon(CurrentData.Id);
        UpdateState();

    }

    private void Select()
    {
        SoundManager.Instance.PlayButtonSound();
        if (CurrentData.Id != ShowroomManager.Instance.CurrentIdSelected)
        {
            ShowroomManager.Instance.SelectElement(CurrentData.Id);
        }

    }

    private void UpdateState()
    {
        BackgroundSelected.gameObject.SetActive(CurrentData.Id == ShowroomManager.Instance.CurrentIdSelected);
        Price.SetText(CurrentData.Price.ToString());
        PriceIcon.gameObject.SetActive(PlayerData.Instance.HaveSkin(CurrentData.Id) == false);
        if (PlayerData.Instance.HaveSkin(CurrentData.Id))
        {
            HandlerButton.image.sprite = (CurrentData.Id == PlayerData.Instance.CurrentSkinId) ? GreenBtn : BlueBtn;
            UsedMark.gameObject.SetActive(CurrentData.Id == PlayerData.Instance.CurrentSkinId);
            UseMark.gameObject.SetActive(CurrentData.Id != PlayerData.Instance.CurrentSkinId);
        }
        else
        {
            HandlerButton.image.sprite = (PlayerData.Instance.CurrentMoney < CurrentData.Price) ? GreyBtn : BlueBtn;
            UsedMark.gameObject.SetActive(false);
            UseMark.gameObject.SetActive(false);
        }
    }

    private void HandleElement()
    {
        SoundManager.Instance.PlayButtonSound();
        ShowroomManager.Instance.SelectElement(CurrentData.Id);

        if (PlayerData.Instance.HaveSkin(CurrentData.Id))
        {
            PlayerData.Instance.UseSkin(CurrentData.Id);
        }
        else
        {
            ShowroomManager.Instance.OnWantToBuyCar?.Invoke(ShowroomManager.Instance.GetDataById(CurrentData.Id));
        }
    }

}
