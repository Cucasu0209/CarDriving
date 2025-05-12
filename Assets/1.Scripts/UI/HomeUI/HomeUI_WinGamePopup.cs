using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_WinGamePopup : MonoBehaviour
{
    [SerializeField] private RectTransform Popup;
    [SerializeField] private Image Backgound;
    [SerializeField] private List<RectTransform> ComponentsInPopup;
    [SerializeField] private Button NextButton;
    [SerializeField] private Image RewardProgressImage;
    [SerializeField] private TextMeshProUGUI RewardProgressText;
    [SerializeField] private TextMeshProUGUI MoneyPlus;

    void Start()
    {
        OnClosePopup();
        NextButton.onClick.AddListener(OnButtonNextClick);
        GameManager.Instance.OnShowEndgamePopup += OnOpenPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnShowEndgamePopup -= OnOpenPopup;

    }
    private void OnOpenPopup(bool isWin)
    {
        if (isWin == false) return;
        Popup.gameObject.SetActive(true);
        Backgound.DOFade(1, 0.3f);
        for (int i = 0; i < ComponentsInPopup.Count; i++)
        {
            ComponentsInPopup[i].DOScale(1, 0.3f).SetDelay(0.3f);
        }

        StartCoroutine(IIncreasePercentage());

        MoneyPlus.SetText("+ " + Random.Range(200, 400).ToString());
    }

    IEnumerator IIncreasePercentage()
    {
        int StartValue = 0;
        int EndValue = Random.Range(10, 101);
        float time = (EndValue - StartValue) * 0.05f;
        RewardProgressImage.fillAmount = StartValue / 100f;
        RewardProgressImage.DOFillAmount(EndValue / 100f, (EndValue - StartValue) * 0.05f).SetEase(Ease.Linear);
        for (int i = StartValue; i <= EndValue; i++)
        {
            RewardProgressText.SetText(i + "%");
            yield return new WaitForSeconds(0.05f);
        }

    }
    private void OnClosePopup()
    {
        Backgound.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() =>
        {
            Popup.gameObject.SetActive(false);
        });
        for (int i = 0; i < ComponentsInPopup.Count; i++)
        {
            ComponentsInPopup[i].DOScale(0, 0.3f);
        }
    }
    private void OnButtonNextClick()
    {
        OnClosePopup();
        GameManager.Instance.SetupGame();
    }
}
