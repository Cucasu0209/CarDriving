using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowroomUI_NotEnoughMoneyPopup : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI Noti;




    private void Start()
    {
        ShowroomManager.Instance.OnWantToBuyCar += SetPopup;
    }
    private void OnDestroy()
    {
        ShowroomManager.Instance.OnWantToBuyCar -= SetPopup;
    }

    private void SetPopup(CarData data)
    {
        if (PlayerData.Instance.HaveEnoughMoney(data.Price) == false)
        {
            ShowNoti();
        }
    }
    private void ShowNoti()
    {
        TextMeshProUGUI newNoti = Instantiate(Noti, transform);
        newNoti.rectTransform.anchoredPosition = Vector2.zero;
        newNoti.DOFade(1, 0.05f).OnComplete(() =>
        {
            newNoti.DOFade(0, 1).SetDelay(0.5f);
            newNoti.rectTransform.DOAnchorPosY(200, 1.5f).OnComplete(() =>
            {
                Destroy(newNoti.gameObject);
            });
        });
    }


}
