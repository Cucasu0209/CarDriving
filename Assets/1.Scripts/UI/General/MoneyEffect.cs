using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoneyEffect : MonoBehaviour
{
    [SerializeField] private List<RectTransform> Money;
    private List<Vector2> StartPos;
    [SerializeField] private RectTransform MoneyHolder;
    [SerializeField] private AudioClip CashSound;

    private void Start()
    {
        StartPos = new List<Vector2>();
        for (int i = 0; i < Money.Count; i++)
        {
            Money[i].localScale = Vector3.zero;
            StartPos.Add(Money[i].anchoredPosition);
        }
        PlayerData.Instance.OnShowEffectAddMoney += ShowEffect;
    }

    private void OnDestroy()
    {
        PlayerData.Instance.OnShowEffectAddMoney -= ShowEffect;

    }

    private void ShowEffect(int count)
    {
        if (count > 0)
        {
            for (int i = 0; i < Money.Count; i++)
            {
                Money[i].localScale = Vector3.zero;
                Money[i].anchoredPosition = StartPos[i];
            }
            for (int i = 0; i < Money.Count; i++)
            {
                Money[i].DOScale(1, 0.2f).SetDelay(i * 0.05f);
                Money[i].transform.DOMove(MoneyHolder.transform.position, 0.4f).SetDelay(0.5f + i * 0.05f);
                Money[i].DOScale(0, 0.4f).SetDelay(i * 0.05f).SetDelay(0.5f + i * 0.05f);
            }

            DOVirtual.DelayedCall(1.1f, () =>
            {
                SoundManager.Instance.PlayEffect(CashSound);
                PlayerData.Instance.OnAddMoney?.Invoke(count);
            });
        }
    }
}
