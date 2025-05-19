using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Money;
    [SerializeField] private TextMeshProUGUI MoneyPlus;

    private void Start()
    {
        MoneyPlus.color = new Color(MoneyPlus.color.r, MoneyPlus.color.g, MoneyPlus.color.b, 0);
        PlayerData.Instance.OnMoneyChange += OnMoneyChange;
        PlayerData.Instance.OnAddMoney += OnMoneyChange;
    }
    private void OnDestroy()
    {
        PlayerData.Instance.OnMoneyChange -= OnMoneyChange;
        PlayerData.Instance.OnAddMoney -= OnMoneyChange;
        MoneyPlus.DOKill();
    }
    private void OnMoneyChange()
    {
        Money.SetText(PlayerData.Instance.CurrentMoney.ToString());
    }
    private void OnMoneyChange(int count)
    {
        MoneyPlus.DOKill();
        MoneyPlus.color = new Color(MoneyPlus.color.r, MoneyPlus.color.g, MoneyPlus.color.b, 0);
        MoneyPlus.rectTransform.anchoredPosition = new Vector2(MoneyPlus.rectTransform.anchoredPosition.x, 0);
        MoneyPlus.DOFade(1, 0.05f);
        MoneyPlus.SetText(((count >= 0) ? "+" : "") + count.ToString());
        MoneyPlus.color = count >= 0 ? Color.green : Color.red;

        MoneyPlus.rectTransform.DOAnchorPosY(60, 0.2f);
        MoneyPlus.DOFade(0, 0.4f).SetDelay(0.8f);
        Money.SetText(PlayerData.Instance.CurrentMoney.ToString());

    }

}
