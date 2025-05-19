using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_ReceiveRewardPopup : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image CongrateText;
    [SerializeField] private Image NewSkinText;
    [SerializeField] private Image Shawdow;
    [SerializeField] private Image CarIcon;
    [SerializeField] private Button FreeAdsBtn;
    [SerializeField] private Button NoThanksBtn;
    [SerializeField] private Button AwesomeBtn;

    [Header("Sound")]
    [SerializeField] private AudioClip CongrateSound;
    private void Start()
    {
        ClosePopup();
        NoThanksBtn.onClick.AddListener(OnAwesomeBtnClick);
        AwesomeBtn.onClick.AddListener(OnAwesomeBtnClick);
        FreeAdsBtn.onClick.AddListener(OnWatchAdsBtnClick);
        GameManager.Instance.OnShowRewardPopup += OpenPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnShowRewardPopup -= OpenPopup;

    }

    private void OpenPopup()
    {
        CarIcon.sprite = ShowroomManager.Instance.GetCarIcon(PlayerData.Instance.GetRewardId());

        BG.gameObject.SetActive(true);
        BG.DOFade(1, 0.2f);
        CongrateText.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        Shawdow.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        CarIcon.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        FreeAdsBtn.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        NoThanksBtn.transform.DOScale(1, 0.2f).SetDelay(0.2f);
    }
    private void ClosePopup()
    {
        BG.DOFade(0, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            BG.gameObject.SetActive(false);

        });
        CongrateText.transform.DOScale(0, 0.2f);
        NewSkinText.transform.DOScale(0, 0.2f);
        Shawdow.transform.DOScale(0, 0.2f);
        CarIcon.transform.DOScale(0, 0.2f);
        FreeAdsBtn.transform.DOScale(0, 0.2f);
        NoThanksBtn.transform.DOScale(0, 0.2f);
        AwesomeBtn.transform.DOScale(0, 0.2f);
    }

    private void OnWatchAdsBtnClick()
    {
        SoundManager.Instance.PlayButtonSound();
        SoundManager.Instance.PlayEffect(CongrateSound);
        PlayerData.Instance.TakeReward();

        FreeAdsBtn.transform.DOScale(0, 0.2f);
        NoThanksBtn.transform.DOScale(0, 0.2f);
        CongrateText.transform.DOScale(0, 0.2f);

        NewSkinText.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        AwesomeBtn.transform.DOScale(1, 0.2f).SetDelay(0.2f);
    }

    private void OnAwesomeBtnClick()
    {
        SoundManager.Instance.PlayButtonSound();
        ClosePopup();
        GameManager.Instance.ResetLevel();
    }
}
