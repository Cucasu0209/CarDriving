using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneImage : MonoBehaviour
{
    [SerializeField] private Image BG;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Tittle;

    private void Start()
    {
        BG.DOFade(0, 0.31f).SetDelay(0.9f).OnComplete(() =>
        {
            BG.gameObject.SetActive(false);
        });
        Icon.DOFade(0, 0.3f).SetDelay(0.6f);
        Tittle.DOFade(0, 0.3f).SetDelay(0.6f);
    }
}
