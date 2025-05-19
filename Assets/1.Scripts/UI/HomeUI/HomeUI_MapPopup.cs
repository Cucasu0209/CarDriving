using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_MapPopup : MonoBehaviour
{
    [SerializeField] private RectTransform MapPopup;
    [SerializeField] private Button BackButton;

    [SerializeField] private Image Progress;
    [SerializeField] private RectTransform CurrentPoint;


    [Header("Levels")]
    [SerializeField] private List<Vector2> LevelPositions;
    [SerializeField] private List<Image> UnlockIcon;
    [SerializeField] private List<RectTransform> LockBanner;

    private void Start()
    {
        BackButton.onClick.AddListener(HidePopup);
        LevelManager.Instance.OnLevelChange += UpdatePopup;
    }
    private void OnDestroy()
    {
        LevelManager.Instance.OnLevelChange -= UpdatePopup;

    }
    public void ShowPopup()
    {
        DOVirtual.DelayedCall(0.4f, () => SoundManager.Instance.PlayOpenPopupSound());

        GameManager.Instance.OnHideHomeUI?.Invoke();
        MapPopup.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        MapPopup.DOAnchorPosY(0, 0.6f).SetDelay(0.4f);
        BackButton.transform.DOScale(1, 0.3f).SetDelay(1f);
    }
    private void HidePopup()
    {
        SoundManager.Instance.PlayClosePopupSound();

        MapPopup.DOAnchorPosY(4500, 0.6f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            MapPopup.gameObject.SetActive(false);
        });
        BackButton.transform.DOScale(0, 0.3f).OnComplete(() => BackButton.gameObject.SetActive(false));
    }
    private void UpdatePopup()
    {
        Progress.fillAmount = LevelPositions[Mathf.Clamp(LevelManager.Instance.LevelIndex - 1, 0, LevelPositions.Count - 1)].y * 1f / Progress.rectTransform.sizeDelta.y;
        CurrentPoint.anchoredPosition = LevelPositions[Mathf.Clamp(LevelManager.Instance.LevelIndex - 1, 0, LevelPositions.Count - 1)];
        //Update Location positions
        for (int i = 0; i < UnlockIcon.Count; i++)
        {
            UnlockIcon[i].gameObject.SetActive(LevelManager.Instance.LevelIndex > i * GameConfig.LEVEL_PER_MAP);
            LockBanner[i].gameObject.SetActive(LevelManager.Instance.LevelIndex <= i * GameConfig.LEVEL_PER_MAP);
        }


    }
}
