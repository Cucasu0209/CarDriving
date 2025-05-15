using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneImage : MonoBehaviour
{
    [SerializeField] private Image BG;

    private void Start()
    {
        BG.DOFade(0, 0.31f).SetDelay(0.8f).OnComplete(() =>
        {
            BG.gameObject.SetActive(false);
        });

    }
}
