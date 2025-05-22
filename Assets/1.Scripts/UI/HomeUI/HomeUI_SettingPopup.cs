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
    float MusicVolume, SfxVolume;
    int isVibrationOn;
    private void Start()
    {
        MusicVolume = PlayerPrefs.GetFloat(GameConfig.MUSIC_KEY, 1);
        SfxVolume = PlayerPrefs.GetFloat(GameConfig.SOUND_KEY, 1);
        isVibrationOn = PlayerPrefs.GetInt(GameConfig.VIBRATION_KEY, 1);

        MusicBtn.image.sprite = MusicVolume > 0 ? OnState : OffState;
        SoundBtn.image.sprite = SfxVolume > 0 ? OnState : OffState;
        VibrationBtn.image.sprite = isVibrationOn > 0 ? OnState : OffState;

        SoundManager.Instance.ChangeBGVolume(MusicVolume);
        SoundManager.Instance.ChangeSFXVolume(SfxVolume);


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
        MusicVolume = MusicVolume > 0 ? 0 : 1;
        SoundManager.Instance.ChangeBGVolume(MusicVolume);
        MusicBtn.image.sprite = MusicVolume > 0 ? OnState : OffState;
    }
    private void ToggleSound()
    {
        SoundManager.Instance.PlayButtonSound();
        SfxVolume = SfxVolume > 0 ? 0 : 1;
        SoundManager.Instance.ChangeSFXVolume(SfxVolume);
        SoundBtn.image.sprite = SfxVolume > 0 ? OnState : OffState;
    }
    private void ToggleVibration()
    {
        SoundManager.Instance.PlayButtonSound();
        isVibrationOn = isVibrationOn > 0 ? 0 : 1;
        SoundManager.Instance.SetHaptic(isVibrationOn);
        VibrationBtn.image.sprite = isVibrationOn > 0 ? OnState : OffState;
    }
}
