using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_WinGamePopup : MonoBehaviour
{
    [SerializeField] private RectTransform Popup;
    [SerializeField] private Image Backgound;
    [SerializeField] private List<RectTransform> ComponentsInPopup;
    [SerializeField] private Image RewardProgressImage;
    [SerializeField] private TextMeshProUGUI RewardProgressText;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI MoneyMultiplier;
    [SerializeField] private TextMeshProUGUI MoneyPlus;

    [Header("Buttons")]
    [SerializeField] private Button NextButton;
    [SerializeField] private Button AdsButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip WinSound;
    void Start()
    {
        OnClosePopup();
        NextButton.onClick.AddListener(OnButtonNextClick);
        AdsButton.onClick.AddListener(OnButtonAdsClick);
        GameManager.Instance.OnEndGame += OnOpenPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnEndGame -= OnOpenPopup;

    }
    private void OnOpenPopup(bool isWin)
    {
        if (isWin == false) return;
        DOVirtual.DelayedCall(3.4f, () =>
        {
            SoundManager.Instance.PlayEffect(WinSound);

            Popup.gameObject.SetActive(true);
            Backgound.DOFade(0.8f, 0.3f);
            for (int i = 0; i < ComponentsInPopup.Count; i++)
            {
                ComponentsInPopup[i].DOScale(1, 0.3f).SetDelay(0.3f);
            }

            StartCoroutine(IIncreasePercentage());

            Money.SetText("+ " + LevelManager.Instance.CurrentLevelData.Money);
            MoneyMultiplier.SetText("Get x" + GameConfig.WIN_REWARD_MULTIPLIER_ADS);
            MoneyPlus.SetText("+ " + LevelManager.Instance.CurrentLevelData.Money * GameConfig.WIN_REWARD_MULTIPLIER_ADS);
            PlayerData.Instance.AddMoney(LevelManager.Instance.CurrentLevelData.Money);
        });
    }

    IEnumerator IIncreasePercentage()
    {
        int StartValue = Mathf.Clamp(PlayerData.Instance.CurrentRewardRate, 0, 100);
        PlayerData.Instance.AddRewardProgress(LevelManager.Instance.CurrentLevelData.RewardRate);
        int EndValue = Mathf.Clamp(PlayerData.Instance.CurrentRewardRate, 0, 100);
        float time = (EndValue - StartValue) * 0.05f;
        RewardProgressImage.fillAmount = StartValue / 100f;

        RewardProgressImage.DOFillAmount(EndValue / 100f, (EndValue - StartValue) * 0.1f).SetEase(Ease.Linear);
        for (int i = StartValue; i <= EndValue; i++)
        {
            RewardProgressText.SetText(i + "%");
            yield return new WaitForSeconds(0.1f);
        }

    }
    private void OnClosePopup()
    {
        Backgound.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() =>
        {
            Popup.gameObject.SetActive(false);
        });
        for (int i = 0; i < ComponentsInPopup.Count; i++)
        {
            ComponentsInPopup[i].DOScale(0, 0.3f);
        }
    }
    private void OnButtonNextClick()
    {
        SoundManager.Instance.PlayButtonSound();
        OnClosePopup();
        GameManager.Instance.NextLevel();
    }
    private void OnButtonAdsClick()
    {
        SoundManager.Instance.PlayButtonSound();
        PlayerData.Instance.AddMoney(LevelManager.Instance.CurrentLevelData.Money * GameConfig.WIN_REWARD_MULTIPLIER_ADS);
        AdsButton.transform.DOScale(0, 0.2f);
    }
}
