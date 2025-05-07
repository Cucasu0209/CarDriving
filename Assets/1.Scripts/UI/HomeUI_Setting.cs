using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI_Setting : MonoBehaviour
{
    [SerializeField] private Button SettingButton;

    private void Start()
    {
        GameManager.Instance.OnGameStart += OnGameStart;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= OnGameStart;

    }


    private void OnGameStart()
    {
        SettingButton.transform.DOScale(0, 0.3f);
    }
}
