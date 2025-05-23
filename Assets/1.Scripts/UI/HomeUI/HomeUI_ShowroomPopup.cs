using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_ShowroomPopup : MonoBehaviour
{
    [SerializeField] private RectTransform Background1;
    [SerializeField] private RectTransform Background2;
    [SerializeField] private Transform CarHolder;
    [SerializeField] private Button CloseButton;
    [SerializeField] private GameObject LightShowroom;

    private void Start()
    {
        CloseButton.onClick.AddListener(ClosePopup);
    }
    public void OpenPopup()
    {
        GameManager.Instance.OnHideHomeUI?.Invoke();
        Background1.gameObject.SetActive(true);
        Background2.gameObject.SetActive(true);
        CarHolder.gameObject.SetActive(true);
        Background1.DOAnchorPosY(0, 0.4f).SetEase(Ease.Linear).SetDelay(0.4f);
        Background2.DOAnchorPosY(0, 0.4f).SetEase(Ease.Linear).SetDelay(0.4f).OnComplete(() =>
        {
            GameManager.Instance.ChangeCameraMode(CameraMode.Showroom);
            DOVirtual.DelayedCall(0.05f, () =>
            {
                Camera.main.transform.localPosition = Vector3.zero;
                Camera.main.transform.localRotation = Quaternion.Euler(0, 0, 0);
            });
        });

        CarHolder.gameObject.SetActive(true);
        CarHolder.DOScale(0.6f, 0.3f).SetDelay(0.8f).OnComplete(() =>
        {
            SoundManager.Instance.PlayOpenPopupSound();
            LightShowroom.SetActive(true);
        });
        DOVirtual.DelayedCall(0.2f, () =>
        {
            ShowroomManager.Instance.OnLoadDataComplete?.Invoke();
           // ShowroomManager.Instance.OnPageChange?.Invoke(true);
            ShowroomManager.Instance.OnSelectElement?.Invoke();
        });

    }
    public void ClosePopup()
    {
        SoundManager.Instance.PlayButtonSound();
        LightShowroom.SetActive(false);

        SoundManager.Instance.PlayButtonSound();
        Background1.DOAnchorPosY(2500, 0.4f).SetDelay(0.2f).SetEase(Ease.Linear);
        Background2.DOAnchorPosY(2500, 0.4f).SetDelay(0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Background1.gameObject.SetActive(false);
            Background2.gameObject.SetActive(false);
            CarHolder.gameObject.SetActive(false);
            GameManager.Instance.OnShowHomeUI?.Invoke();
        });
        CarHolder.DOScale(0, 0.2f).OnComplete(() =>
        {
            GameManager.Instance.ChangeCameraMode(CameraMode.Gameplay);

            CarHolder.gameObject.SetActive(false);
        });
    }
}
