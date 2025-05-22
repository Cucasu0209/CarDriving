using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Banner : MonoBehaviour
{
    [SerializeField] private RectTransform BodySelf;
    [SerializeField] private TextMeshProUGUI Level;
    [SerializeField] private TextMeshProUGUI LocationName;
    [SerializeField] private TextMeshProUGUI TitleNextLocation;

    [SerializeField] private TextMeshProUGUI TapToPlayTest;
    [SerializeField] private Image Dark;

    [Header("Level Progress")]
    [SerializeField] private Image CurrentLocation;
    [SerializeField] private Image NextLocation;
    [SerializeField] private List<Image> ProgressKnots;
    [SerializeField] private Sprite LevelOn, LevelOff, LevelPassed;

    [Header("Weather")]
    [SerializeField] private Image WeatherIcon;
    [SerializeField] private Sprite SunnyIcon, SnowwyIcon, RainnyIcon, FoggyIcon;

    private float StartPosY;
    private void Start()
    {
        TapToPlayTest.transform.localScale = Vector3.one;
        TapToPlayTest.transform.DOScale(0.93f, 0.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        StartPosY = BodySelf.anchoredPosition.y;
        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
        LevelManager.Instance.OnLevelChange += UpdateLevel;
    }

    private void OnDestroy()
    {
        TapToPlayTest.DOKill();
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;
        LevelManager.Instance.OnLevelChange -= UpdateLevel;
    }
    private void UpdateLevel()
    {
        Level.SetText($"LEVEL {LevelManager.Instance.LevelIndex}");
        LocationName.SetText(GameConfig.LOCATIONS_NAME[Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP, 0, GameConfig.LOCATIONS_NAME.Length - 1)]);
        TitleNextLocation.SetText($"Next location on level {(((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP) + 1) * GameConfig.LEVEL_PER_MAP + 1}");

        CurrentLocation.sprite = Resources.Load<Sprite>("Icons/Map" + Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP + 1, 1, GameConfig.LOCATIONS_NAME.Length));
        NextLocation.sprite = Resources.Load<Sprite>("Icons/Map" + Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP + 2, 1, GameConfig.LOCATIONS_NAME.Length));

        int levelInProgress = (LevelManager.Instance.LevelIndex - 1) % GameConfig.LEVEL_PER_MAP;
        for (int i = 0; i < ProgressKnots.Count; i++)
        {
            ProgressKnots[i].transform.DOKill();
            ProgressKnots[i].transform.localScale = Vector3.one;
            if (i == levelInProgress)
            {
                ProgressKnots[i].transform.DOScale(1.2f, 0.6f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }
            ProgressKnots[i].sprite = (i == levelInProgress) ? LevelOn : ((i > levelInProgress) ? LevelOff : LevelPassed);
        }

        switch (LevelManager.Instance.CurrentLevelData.Weather)
        {
            case WeatherType.Foggy:
                WeatherIcon.sprite = FoggyIcon;
                break;
            case WeatherType.Sunny:
                WeatherIcon.sprite = SunnyIcon;
                break;
            case WeatherType.Rainny:
                WeatherIcon.sprite = RainnyIcon;
                break;
            case WeatherType.Snowy:
                WeatherIcon.sprite = SnowwyIcon;
                break;
        }
    }
    private void OnHide()
    {
        float duration = 0.4f;
        BodySelf.DOAnchorPosY(StartPosY + 2500, duration);
        TapToPlayTest.DOFade(0, duration);
        Dark.DOFade(0, duration);
    }
    private void OnShow()
    {
        float duration = 0.4f;
        BodySelf.DOAnchorPosY(StartPosY, duration);
        TapToPlayTest.DOFade(1, duration);
        Dark.DOFade(0.6f, duration);
    }
}
