using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI Money;

    private void Start()
    {
        PlayerData.Instance.OnMoneyChange += OnMoneyChange;
    }
    private void OnDestroy()
    {
        PlayerData.Instance.OnMoneyChange -= OnMoneyChange;

    }
    private void OnMoneyChange()
    {
        Money.SetText(PlayerData.Instance.CurrentMoney.ToString());
    }
}
