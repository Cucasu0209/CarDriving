using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GotoSceneButton : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private Button Button;
    [SerializeField] private Image BG;
    void Start()
    {
        Button.onClick.AddListener(BackToHome);
    }

    private void BackToHome()
    {
        SoundManager.Instance.PlayButtonSound();
        BG.gameObject.SetActive(true);
        BG.DOFade(1, 0.2f).OnComplete(() =>
        {
            SceneManager.LoadScene(SceneName);
        });
    }
}
