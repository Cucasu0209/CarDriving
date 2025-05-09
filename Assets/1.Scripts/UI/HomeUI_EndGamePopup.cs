using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_EndGamePopup : MonoBehaviour
{
    [SerializeField] private RectTransform Popup;
    [SerializeField] private Image Backgound;
    [SerializeField] private List<RectTransform> ComponentsInPopup;
    [SerializeField] private Button NextButton;
    void Start()
    {
        NextButton.onClick.AddListener(OnButtonNextClick);
        GameManager.Instance.OnShowEndgamePopup += OnOpenPopup;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnShowEndgamePopup -= OnOpenPopup;

    }
    private void OnOpenPopup()
    {
        Popup.gameObject.SetActive(true);
        Backgound.DOFade(1, 0.3f);
        for (int i = 0; i < ComponentsInPopup.Count; i++)
        {
            ComponentsInPopup[i].DOScale(1, 0.3f).SetDelay(0.3f);
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
            ComponentsInPopup[i].DOScale(0, 0.3f).SetEase(Ease.OutBounce);
        }
    }
    private void OnButtonNextClick()
    {
        OnClosePopup();
        GameManager.Instance.SetupGame();
    }
}
