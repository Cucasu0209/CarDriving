using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsGameRunning { get; private set; }

    //Events
    public Action OnHideHomeUI;
    public Action OnShowHomeUI;

    public Action OnSetupGame;
    public Action OnGameStart;

    public Action<Transform> OnPickCustomer;

    public Action OnFinishTrace;
    public Action OnShowEndgamePopup;

    public Action<float> OnUpdateProgress;
    public Action<float> OnUpdatePickupPoint;


    private void Awake()
    {
        IsGameRunning = false;
        Instance = this;
    }
    private IEnumerator Start()
    {
        yield return null;
        SetupGame();
    }

    public void StartGame()
    {
        IsGameRunning = true;

        OnGameStart?.Invoke();
        OnHideHomeUI?.Invoke();
    }

    public void SetupGame()
    {
        IsGameRunning = false;
        OnSetupGame?.Invoke();
        OnShowHomeUI?.Invoke();
    }

}
