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

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;

    }


    private void OnGameStart()
    {
        float duration = 0.4f;
        BodySelf.DOAnchorPosY(663, duration);
        TapToPlayTest.DOFade(0, duration);
        Dark.DOFade(0, duration);
    }
}
