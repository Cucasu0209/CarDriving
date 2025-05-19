using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUI_ShowroomButton : MonoBehaviour
{
    [SerializeField] private Button ShowroomButton;
    [SerializeField] private Image ShowroomBG;

    private float StartPosX;
    private void Start()
    {
        ShowroomButton.onClick.AddListener(GotoShowroom);
        StartPosX = ShowroomButton.GetComponent<RectTransform>().anchoredPosition.x;
        GameManager.Instance.OnHideHomeUI += OnHide;
        GameManager.Instance.OnShowHomeUI += OnShow;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnHideHomeUI -= OnHide;
        GameManager.Instance.OnShowHomeUI -= OnShow;

    }


    private void OnHide()
    {
        ShowroomButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX + 400, 0.3f);
    }
    private void OnShow()
    {
        ShowroomButton.GetComponent<RectTransform>().DOAnchorPosX(StartPosX, 0.3f);

    }

    private void GotoShowroom()
    {
        SoundManager.Instance.PlayButtonSound();
        ShowroomBG.gameObject.SetActive(true);
        ShowroomBG.DOFade(1, 0.2f).OnComplete(() =>
        {
            SceneManager.LoadScene("Showroom");
        });

    }
}
