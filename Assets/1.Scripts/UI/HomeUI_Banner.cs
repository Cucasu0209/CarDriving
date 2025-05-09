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
    [SerializeField] private Image LocationImage;
    [SerializeField] private TextMeshProUGUI LocationName;
    [SerializeField] private TextMeshProUGUI TitleNextLocation;

    [SerializeField] private TextMeshProUGUI TapToPlayTest;
    [SerializeField] private Image Dark;

    [Header("Level Progress")]
    [SerializeField] private Image CurrentLocation;
    [SerializeField] private Image NextLocation;
    [SerializeField] private List<Image> ProgressKnots;
    [SerializeField] private Color PassedColor, CurrentColor, NextColor;
    private float StartPosY;
    private void Start()
    {
        StartPosY = BodySelf.anchoredPosition.y;
        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
        LevelManager.Instance.OnLevelChange += UpdateLevel;

    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;
        LevelManager.Instance.OnLevelChange -= UpdateLevel;

    }
    private void UpdateLevel()
    {
        Level.SetText($"LEVEL {LevelManager.Instance.LevelIndex}");
        LocationName.SetText(GameConfig.LOCATIONS_NAME[Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP, 0, GameConfig.LOCATIONS_NAME.Length - 1)]);
        LocationImage.sprite = Resources.Load<Sprite>("Icons/Map" + Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP + 1, 1, GameConfig.LOCATIONS_NAME.Length));
        TitleNextLocation.SetText($"NEXT LOCATION IN LEVEL {(((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP) + 1) * GameConfig.LEVEL_PER_MAP}");

        CurrentLocation.sprite = Resources.Load<Sprite>("Icons/Map" + Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP + 1, 1, GameConfig.LOCATIONS_NAME.Length));
        NextLocation.sprite = Resources.Load<Sprite>("Icons/Map" + Mathf.Clamp((LevelManager.Instance.LevelIndex - 1) / GameConfig.LEVEL_PER_MAP + 2, 1, GameConfig.LOCATIONS_NAME.Length));

        for (int i = 0; i < ProgressKnots.Count; i++)
        {
            if (i == (LevelManager.Instance.LevelIndex - 1) % GameConfig.LEVEL_PER_MAP)
            {
                ProgressKnots[i].transform.localScale = Vector3.one * 1.3f;
                ProgressKnots[i].color = CurrentColor;
            }
            else if (i > (LevelManager.Instance.LevelIndex - 1) % GameConfig.LEVEL_PER_MAP)
            {
                ProgressKnots[i].transform.localScale = Vector3.one;
                ProgressKnots[i].color = NextColor;
            }
            else
            {
                ProgressKnots[i].transform.localScale = Vector3.one;
                ProgressKnots[i].color = PassedColor;
            }
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
