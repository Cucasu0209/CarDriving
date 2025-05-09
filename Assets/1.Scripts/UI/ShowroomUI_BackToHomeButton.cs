using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowroomUI_BackToHomeButton : MonoBehaviour
{
    [SerializeField] private Button BackToHomeBtn;
    [SerializeField] private Image HomeBG;
    [SerializeField] private Image HomeIcon;
    [SerializeField] private TextMeshProUGUI HomeTittle;
    void Start()
    {
        BackToHomeBtn.onClick.AddListener(BackToHome);
    }

    private void BackToHome()
    {
        HomeBG.gameObject.SetActive(true);
        HomeBG.DOFade(1, 0.2f);
        HomeIcon.DOFade(1, 0.2f).SetDelay(0.2f);
        HomeTittle.DOFade(1, 0.21f).SetDelay(0.2f).OnComplete(() =>
        {
            SceneManager.LoadScene("Home");
        });
    }
}
