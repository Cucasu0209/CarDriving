using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_ReceiveRewardPopup : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image BGInCamera;
    [SerializeField] private Image NewSkinBanner;
    [SerializeField] private Image CongrateBanner;
    [SerializeField] private Button FreeAdsBtn;
    [SerializeField] private Button NoThanksBtn;
    [SerializeField] private Button AwesomeBtn;
    [SerializeField] private RectTransform MoneyIcon;

    [Header("3D Component")]
    [SerializeField] private GameObject Confeti;
    [SerializeField] private HomeUI_RewardShowCar Holder;

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


        BG.gameObject.SetActive(true);
        BG.DOFade(0, 0.2f);

        BGInCamera.gameObject.SetActive(true);
        BGInCamera.DOFade(1, 0.2f).OnComplete(() =>
        {
            GameManager.Instance.ChangeCameraMode(CameraMode.Reward);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
            });
        });

        FreeAdsBtn.transform.DOScale(1, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            FreeAdsBtn.transform.DOScale(1.1f, 0.3f).SetLoops(-1, LoopType.Yoyo);

        });
        NoThanksBtn.transform.DOScale(1, 0.2f).SetDelay(3.7f);

        //3d
        if (PlayerData.Instance.GetRewardId() >= 0)
        {
            NewSkinBanner.transform.DOScale(1, 0.2f).SetDelay(0.2f);

            Holder.gameObject.SetActive(true);
            Holder.ShowCar(PlayerData.Instance.GetRewardId());
            Holder.transform.DOScale(0.7f, 0.2f);
        }
        else
        {
            SoundManager.Instance.PlayEffect(CongrateSound);

            CongrateBanner.transform.DOScale(1, 0.2f).SetDelay(0.2f);
            MoneyIcon.DOScale(1, 0.2f).SetDelay(0.2f);
        }

        Confeti.SetActive(true);

    }
    private void ClosePopup()
    {
        BG.DOFade(0, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            BG.gameObject.SetActive(false);
        });
        BGInCamera.DOFade(0, 0.2f).SetDelay(0.2f).OnComplete(() =>
        {
            BGInCamera.gameObject.SetActive(false);
        });
        NewSkinBanner.transform.DOScale(0, 0.2f);
        CongrateBanner.transform.DOScale(0, 0.2f);
        FreeAdsBtn.transform.DOKill();
        FreeAdsBtn.transform.DOScale(0, 0.2f);
        NoThanksBtn.transform.DOKill();
        NoThanksBtn.transform.DOScale(0, 0.2f);
        AwesomeBtn.transform.DOScale(0, 0.2f);

        //3d
        Holder.transform.DOScale(0, 0.2f).OnComplete(() =>
        {
            GameManager.Instance.ChangeCameraMode(CameraMode.Gameplay);

            Holder.gameObject.SetActive(false);
        });
        Confeti.SetActive(false);
        MoneyIcon.DOScale(0, 0.2f);

    }

    private void OnWatchAdsBtnClick()
    {
        SoundManager.Instance.PlayButtonSound();
        if (PlayerData.Instance.GetRewardId() >= 0) SoundManager.Instance.PlayEffect(CongrateSound);
        PlayerData.Instance.TakeReward();

        FreeAdsBtn.transform.DOKill();
        FreeAdsBtn.transform.DOScale(0, 0.2f);
        NoThanksBtn.transform.DOKill();
        NoThanksBtn.transform.DOScale(0, 0.2f);

        MoneyIcon.DOScale(0, 0.2f);

        if (PlayerData.Instance.GetRewardId() >= 0)
        {

            NewSkinBanner.transform.DOScale(0, 0.2f);

            CongrateBanner.transform.DOScale(1, 0.2f).SetDelay(0.2f);
            AwesomeBtn.transform.DOScale(1, 0.2f).SetDelay(0.2f);
        }
        else
        {
            DOVirtual.DelayedCall(1.2f, () =>
            {
                ClosePopup();
                GameManager.Instance.SetupLevel();
            });

        }
    }

    private void CancelReceiveReward()
    {
        OnAwesomeBtnClick();
        PlayerData.Instance.CancelReward();
    }
    private void OnAwesomeBtnClick()
    {
        SoundManager.Instance.PlayButtonSound();
        ClosePopup();
        GameManager.Instance.SetupLevel();
    }
}
