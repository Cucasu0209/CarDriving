using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_JourneyProgress : MonoBehaviour
{
    [SerializeField] private RectTransform BodySelf;
    [SerializeField] private Image FilledImage;

    [SerializeField] private TextMeshProUGUI CurrentLevel;
    [SerializeField] private TextMeshProUGUI NextLevel;

    private void Start()
    {
        GameManager.Instance.OnGameStart += ShowProgress;
        GameManager.Instance.OnEndGame += HideProgress;
        GameManager.Instance.OnRevive += ShowProgress;
        GameManager.Instance.OnUpdateProgress += UpdateProgress;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= ShowProgress;
        GameManager.Instance.OnEndGame -= HideProgress;
        GameManager.Instance.OnRevive -= ShowProgress;
        GameManager.Instance.OnUpdateProgress -= UpdateProgress;

    }
    private void SetupPin()
    {
        FilledImage.fillAmount = 0;
        CurrentLevel.SetText(LevelManager.Instance.LevelIndex.ToString());
        NextLevel.SetText((LevelManager.Instance.LevelIndex + 1).ToString());
    }


    private void UpdateProgress(float rate)
    {
        FilledImage.fillAmount = rate;
    }



    private void ShowProgress()
    {
        BodySelf.DOAnchorPosY(-242, 0.4f).SetDelay(0.2f);
        FilledImage.fillAmount = 0;
        CurrentLevel.SetText(LevelManager.Instance.LevelIndex.ToString());
        NextLevel.SetText((LevelManager.Instance.LevelIndex + 1).ToString());
    }
    private void HideProgress(bool isWin)
    {
        BodySelf.DOAnchorPosY(242, 0.4f);

    }
}
