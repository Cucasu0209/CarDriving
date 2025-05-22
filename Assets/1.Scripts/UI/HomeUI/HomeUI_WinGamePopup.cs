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
    [SerializeField] private RectTransform CompleteBanner;
    [SerializeField] private RectTransform PercentCarHolder;
    [SerializeField] private RectTransform MoneyHolder;

    [Header("Reward")]
    [SerializeField] private Image RewardShadow;
    [SerializeField] private Image RewardProgressImage;
    [SerializeField] private TextMeshProUGUI RewardProgressText;

    [Header("Money")]
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI MoneyMultiplier;

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
        //reset and set data
        if (isWin == false) return;
        AdsButton.interactable = true;
        NextButton.interactable = true;
        LevelManager.Instance.NextLevel();
        PlayerData.Instance.AddMoney(LevelManager.Instance.CurrentLevelData.Money, false);
        PlayerData.Instance.AddRewardProgress(LevelManager.Instance.CurrentLevelData.RewardRate);

        //Wait 
        DOVirtual.DelayedCall(2f, () =>
        {
            //Sound
            SoundManager.Instance.PlayEffect(WinSound);

            //set Icon
            RewardProgressImage.sprite = ShowroomManager.Instance.GetCarIcon(PlayerData.Instance.GetRewardId());
            RewardShadow.sprite = ShowroomManager.Instance.GetCarShadow(PlayerData.Instance.GetRewardId());

            //Open Animations
            Popup.gameObject.SetActive(true);
            Backgound.DOFade(0.8f, 0.3f);
            CompleteBanner.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
            PercentCarHolder.transform.DOScale(1, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack);



            // AdsButton.transform.DOScale(1.1f, 0.3f).SetDelay(0.6f).SetLoops(-1, LoopType.Yoyo);
            StartCoroutine(IIncreasePercentage());

            MoneyMultiplier.SetText("Get x" + GameConfig.WIN_REWARD_MULTIPLIER_ADS);
        });
    }

    IEnumerator IIncreasePercentage()
    {
        //set start value
        int StartValue = ((PlayerData.Instance.CurrentRewardRate - LevelManager.Instance.CurrentLevelData.RewardRate) % 100 + 100) % 100;
        int EndValue = PlayerData.Instance.CurrentRewardRate == 0 ? 100 : PlayerData.Instance.CurrentRewardRate;
        float time = (EndValue - StartValue) * 0.05f;
        RewardProgressText.SetText(StartValue + "%");
        RewardProgressImage.fillAmount = StartValue / 100f;

        //run percent car
        yield return new WaitForSeconds(0.7f);
        SoundManager.Instance.PlayLoop(PercentSound);
        RewardProgressImage.DOFillAmount(EndValue / 100f, (EndValue - StartValue + 1) * 0.03f).SetEase(Ease.Linear);
        for (int i = StartValue; i <= EndValue; i++)
        {
            RewardProgressText.SetText(i + "%");
            yield return new WaitForSeconds(0.03f);
        }
        SoundManager.Instance.StopLoopSound(PercentSound);

        //run percent Money
        MoneyHolder.transform.DOScale(1, 0.4f);
        Money.SetText("+ 0");
        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlayLoop(PercentSound);
        for (int i = 0; i <= LevelManager.Instance.CurrentLevelData.Money; i++)
        {
            Money.SetText("+ " + i);
            yield return new WaitForSeconds(0.03f);
        }
        SoundManager.Instance.StopLoopSound(PercentSound);

        yield return new WaitForSeconds(0.5f);
        AdsButton.transform.DOScale(1, 0.3f);
        NextButton.transform.DOScale(1, 0.3f);
    }
    private void OnClosePopup()
    {
        AdsButton.transform.DOKill();

        Backgound.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() =>
        {
            Popup.gameObject.SetActive(false);
        });
        CompleteBanner.transform.DOScale(0, 0.3f);
        AdsButton.transform.DOScale(0, 0.3f);
        NextButton.transform.DOScale(0, 0.3f);
        MoneyHolder.transform.DOScale(0, 0.3f);
        PercentCarHolder.transform.DOScale(0, 0.3f);
    }
    private void OnButtonNextClick()
    {
        AdsButton.interactable = false;
        NextButton.interactable = false;
        AdsButton.transform.DOKill();


        PlayerData.Instance.OnShowEffectAddMoney?.Invoke(LevelManager.Instance.CurrentLevelData.Money);
        SoundManager.Instance.PlayButtonSound();
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
    private void OnButtonAdsClick()
    {
        AdsButton.transform.DOKill();
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
