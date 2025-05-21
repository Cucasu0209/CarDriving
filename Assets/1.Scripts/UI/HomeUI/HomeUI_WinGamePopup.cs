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
    [SerializeField] private Image RewardShadow;
    [SerializeField] private Image RewardProgressImage;
    [SerializeField] private TextMeshProUGUI RewardProgressText;
    [SerializeField] private RectTransform MoneyHolder;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI MoneyMultiplier;
    [SerializeField] private TextMeshProUGUI MoneyPlus;

    [Header("Buttons")]
    [SerializeField] private Button NextButton;
    [SerializeField] private Button AdsButton;

    [Header("Sounds")]
    [SerializeField] private AudioClip WinSound;
    [SerializeField] private AudioClip PercentSound;
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
        AdsButton.interactable = true;
        NextButton.interactable = true;
        LevelManager.Instance.NextLevel();
        PlayerData.Instance.AddMoney(LevelManager.Instance.CurrentLevelData.Money, false);
        PlayerData.Instance.AddRewardProgress(LevelManager.Instance.CurrentLevelData.RewardRate);


        DOVirtual.DelayedCall(2f, () =>
        {
            SoundManager.Instance.PlayEffect(WinSound);

            RewardProgressImage.sprite = ShowroomManager.Instance.GetCarIcon(PlayerData.Instance.GetRewardId());
            RewardShadow.sprite = ShowroomManager.Instance.GetCarShadow(PlayerData.Instance.GetRewardId());

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
        });
    }

    IEnumerator IIncreasePercentage()
    {
        int StartValue = ((PlayerData.Instance.CurrentRewardRate - LevelManager.Instance.CurrentLevelData.RewardRate) % 100 + 100) % 100;
        int EndValue = PlayerData.Instance.CurrentRewardRate == 0 ? 100 : PlayerData.Instance.CurrentRewardRate;
        float time = (EndValue - StartValue) * 0.05f;
        RewardProgressImage.fillAmount = StartValue / 100f;

        yield return new WaitForSeconds(0.6f);
        SoundManager.Instance.PlayLoop(PercentSound);
        RewardProgressImage.DOFillAmount(EndValue / 100f, (EndValue - StartValue) * 0.05f).SetEase(Ease.Linear);
        for (int i = StartValue; i <= EndValue; i++)
        {
            RewardProgressText.SetText(i + "%");
            yield return new WaitForSeconds(0.05f);
        }
        SoundManager.Instance.StopLoopSound(PercentSound);

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
        AdsButton.interactable = false;
        NextButton.interactable = false;
        PlayerData.Instance.OnShowEffectAddMoney?.Invoke(LevelManager.Instance.CurrentLevelData.Money);
        SoundManager.Instance.PlayButtonSound();
        MoneyHolder.DOScale(0, 0.3f);


        if (PlayerData.Instance.CanTakeReward())
        {
            DOVirtual.DelayedCall(1, () =>
            {
                OnClosePopup();
                GameManager.Instance.OnShowRewardPopup?.Invoke();
            });
        }
        else
        {
            DOVirtual.DelayedCall(1, () =>
            {
                OnClosePopup();
                GameManager.Instance.SetupLevel();
            });
        }
    }
    private void OnButtonAdsClick()
    {
        AdsButton.interactable = false;
        NextButton.interactable = false;
        SoundManager.Instance.PlayButtonSound();
        PlayerData.Instance.AddMoney(LevelManager.Instance.CurrentLevelData.Money * (GameConfig.WIN_REWARD_MULTIPLIER_ADS - 1), false);
        PlayerData.Instance.OnShowEffectAddMoney?.Invoke(LevelManager.Instance.CurrentLevelData.Money * GameConfig.WIN_REWARD_MULTIPLIER_ADS);
        AdsButton.transform.DOScale(0, 0.2f);
        MoneyHolder.DOScale(0, 0.3f);
        NextButton.transform.DOScale(0, 0.3f);


        if (PlayerData.Instance.CanTakeReward())
        {
            DOVirtual.DelayedCall(1, () =>
            {
                OnClosePopup();
                GameManager.Instance.OnShowRewardPopup?.Invoke();
            });
        }
        else
        {
            DOVirtual.DelayedCall(1, () =>
            {
                OnClosePopup();
                GameManager.Instance.SetupLevel();
            });
        }

    }
}
