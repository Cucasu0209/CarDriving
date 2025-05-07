using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Map : MonoBehaviour
{
    [SerializeField] private Button MapButton;

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
        MapButton.GetComponent<RectTransform>().DOAnchorPosX(232, 0.3f);
    }
}
