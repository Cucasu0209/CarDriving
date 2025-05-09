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
    [SerializeField] private List<TextMeshProUGUI> LevelTexts;
    [SerializeField] private List<RectTransform> CurrentPoints;


    [Header("Levels")]
    [SerializeField] private List<RectTransform> Locations;
    [SerializeField] private List<TextMeshProUGUI> LocationNames;
    [SerializeField] private List<Image> LocationImages;
    [SerializeField] private List<Image> LockIcons;
    [SerializeField] private Color CompleteColor, PreparedColor;
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
        GameManager.Instance.OnHideHomeUI?.Invoke();
        MapPopup.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        MapPopup.DOAnchorPosY(0, 0.6f).SetDelay(0.4f);
        BackButton.transform.DOScale(1, 0.3f).SetDelay(1f);
    }
    private void HidePopup()
    {
        MapPopup.DOAnchorPosY(4500, 0.6f).OnComplete(() =>
        {
            GameManager.Instance.OnShowHomeUI?.Invoke();
            MapPopup.gameObject.SetActive(false);
        });
        BackButton.transform.DOScale(0, 0.3f).OnComplete(() => BackButton.gameObject.SetActive(false));
    }
    private void UpdatePopup()
    {
        Progress.fillAmount = LevelManager.Instance.LevelIndex * 1f / GameConfig.MAX_LEVEL;

        //Update Location positions
        for (int i = 0; i < Locations.Count; i++)
        {
            Locations[i].anchoredPosition = new Vector2(0, Progress.rectTransform.sizeDelta.y * i / Locations.Count);
            LockIcons[i].gameObject.SetActive(LevelManager.Instance.LevelIndex <= i * GameConfig.LEVEL_PER_MAP);
            LocationImages[i].color = new Color(1, 1, 1, LevelManager.Instance.LevelIndex <= i * GameConfig.LEVEL_PER_MAP ? 0.6f : 1);
            LocationNames[i].color = LevelManager.Instance.LevelIndex <= i * GameConfig.LEVEL_PER_MAP ? PreparedColor : CompleteColor;
        }

        //Current Location
        for (int i = 0; i < CurrentPoints.Count; i++)
        {
            CurrentPoints[i].gameObject.SetActive(((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP) % 2 == i % 2);
            LevelTexts[i].SetText(LevelManager.Instance.LevelIndex.ToString());
            CurrentPoints[i].anchoredPosition = new Vector2(0, Progress.rectTransform.sizeDelta.y * LevelManager.Instance.LevelIndex * 1f / GameConfig.MAX_LEVEL);
        }

    }
}
