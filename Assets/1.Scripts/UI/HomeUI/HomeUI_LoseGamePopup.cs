using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_LoseGamePopup : MonoBehaviour
{
    [SerializeField] private Image Background;
    [SerializeField] private List<RectTransform> ComponentsInPopup;
    [SerializeField] private Image Progress;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button RetryButton;
    [Header("Sounds")]
    [SerializeField] private AudioClip LoseSound;
    [SerializeField] private AudioClip TimmingSound;
    void Start()
    {
        OnClosePopup();
        ContinueButton.onClick.AddListener(Revive);
        RetryButton.onClick.AddListener(Retry);
        GameManager.Instance.OnEndGame += OnOpenPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnEndGame -= OnOpenPopup;

    }

    private void OnOpenPopup(bool isWin)
    {
        if (isWin) return;
        DOVirtual.DelayedCall(1, () =>
        {
            SoundManager.Instance.PlayEffect(LoseSound);
            Background.gameObject.SetActive(true);
            Background.DOFade(0.7f, 0.3f);
            for (int i = 0; i < ComponentsInPopup.Count; i++)
            {
                ComponentsInPopup[i].DOScale(1, 0.3f).SetDelay(0.3f);
            }
            ContinueButton.transform.DOScale(1.1f, 0.3f).SetDelay(0.6f).SetLoops(-1, LoopType.Yoyo);

            SoundManager.Instance.PlayLoop(TimmingSound);
            Progress.fillAmount = 0;
            Progress.DOFillAmount(1, GameConfig.TIME_WAIT_LOSE_GAME).SetEase(Ease.Linear).OnComplete(() =>
            {
                ContinueButton.transform.DOKill();
                ContinueButton.transform.DOScale(0, 0.2f);
                RetryButton.transform.DOScale(1, 0.2f).SetDelay(0.2f);
                SoundManager.Instance.StopLoopSound(TimmingSound);

            });
        });

    }
    private void OnClosePopup()
    {
        SoundManager.Instance.StopLoopSound(TimmingSound);
        ContinueButton.transform.DOKill();
        Background.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() =>
        {
            Background.gameObject.SetActive(false);
        });
        for (int i = 0; i < ComponentsInPopup.Count; i++)
        {
            ComponentsInPopup[i].DOScale(0, 0.3f);
        }
        RetryButton.transform.DOScale(0, 0.3f);
    }

    private void Revive()
    {
        SoundManager.Instance.PlayButtonSound();
        Progress.DOKill();
        OnClosePopup();
        GameManager.Instance.Revive();

    }
    private void Retry()
    {
        SoundManager.Instance.PlayButtonSound();
        OnClosePopup();
        GameManager.Instance.SetupLevel();

    }
}
