using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_SettingPopup : MonoBehaviour
{
    [SerializeField] private RectTransform SettingPopup;
    [SerializeField] private Image PopupDark;
    [SerializeField] private Button CancelButton;

    [Header("Toggles")]
    [SerializeField] private Button MusicBtn;
    [SerializeField] private Button SoundBtn;
    [SerializeField] private Button VibrationBtn;
    [SerializeField] private Sprite OnState, OffState;
    float isMusicOn, isSoundOn, isVibrationOn;
    private void Start()
    {
        isMusicOn = PlayerPrefs.GetFloat(GameConfig.MUSIC_KEY, 1);
        isSoundOn = PlayerPrefs.GetFloat(GameConfig.SOUND_KEY, 1);
        isVibrationOn = PlayerPrefs.GetFloat(GameConfig.VIBRATION_KEY, 1);
        MusicBtn.image.sprite = isMusicOn > 0 ? OnState : OffState;
        SoundBtn.image.sprite = isSoundOn > 0 ? OnState : OffState;
        VibrationBtn.image.sprite = isVibrationOn > 0 ? OnState : OffState;

        MusicBtn.onClick.AddListener(ToggleMusic);
        SoundBtn.onClick.AddListener(ToggleSound);
        VibrationBtn.onClick.AddListener(ToggleVibration);

        CancelButton.onClick.AddListener(HidePopup);
    }
    public void ShowPopup()
    {
        DOVirtual.DelayedCall(0.4f, () => SoundManager.Instance.PlayOpenPopupSound());

        GameManager.Instance.OnHideHomeUI?.Invoke();
        SettingPopup.gameObject.SetActive(true);
        PopupDark.gameObject.SetActive(true);
        SettingPopup.DOScale(1, 0.4f).SetDelay(0.4f);
        PopupDark.DOFade(0.8f, 0.3f).SetDelay(0.4f);
    }
    private void HidePopup()
    {
        SoundManager.Instance.PlayClosePopupSound();
        SettingPopup.DOScale(0, 0.3f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            SettingPopup.gameObject.SetActive(false);
        });
        PopupDark.DOFade(0, 0.3f).OnComplete(() => PopupDark.gameObject.SetActive(false));
    }
    private void ToggleMusic()
    {
        SoundManager.Instance.PlayButtonSound();
        isMusicOn = isMusicOn > 0 ? 0 : 1;
        PlayerPrefs.SetFloat(GameConfig.MUSIC_KEY, isMusicOn);
        MusicBtn.image.sprite = isMusicOn > 0 ? OnState : OffState;
    }
    private void ToggleSound()
    {
        SoundManager.Instance.PlayButtonSound();
        isSoundOn = isSoundOn > 0 ? 0 : 1;
        PlayerPrefs.SetFloat(GameConfig.SOUND_KEY, isSoundOn);
        SoundBtn.image.sprite = isSoundOn > 0 ? OnState : OffState;
    }
    private void ToggleVibration()
    {
        SoundManager.Instance.PlayButtonSound();
        isVibrationOn = isVibrationOn > 0 ? 0 : 1;
        PlayerPrefs.SetFloat(GameConfig.VIBRATION_KEY, isVibrationOn);
        VibrationBtn.image.sprite = isVibrationOn > 0 ? OnState : OffState;
    }
}
