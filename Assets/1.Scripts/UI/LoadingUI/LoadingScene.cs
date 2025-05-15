using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Image LoadingProcess;
    [SerializeField] private TextMeshProUGUI LoadingText;
    [SerializeField] private Image BG;


    private void Start()
    {
        LoadingProcess.DOFillAmount(1, 2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            BG.gameObject.SetActive(true);
            BG.DOFade(1, 0.2f).OnComplete(() =>
            {
                SceneManager.LoadScene("Home");
            });
        });

    }
}
