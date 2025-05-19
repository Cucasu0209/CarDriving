using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int FPSTarget = 60;
    public static GameManager Instance;
    public bool IsGameRunning { get; private set; }

    //Events
    public Action OnHideHomeUI;
    public Action OnShowHomeUI;

    public Action OnSetupGame;
    public Action OnGameStart;

    public Action<Transform> OnPickCustomer;

    public Action OnFinishTrace;
    public Action<bool> OnEndGame;

    public Action<float> OnUpdateProgress;
    public Action<float> OnUpdatePickupPoint;
    public Action OnShowRewardPopup;

    public Action OnNextLevel;
    public Action OnReset;
    public Action OnRevive;

    public Action<Vector3, Transform> OnCameraMove; // Cam Pos, Player Pos
    private void Awake()
    {
        IsGameRunning = false;
        Instance = this;
    }
    private IEnumerator Start()
    {
        Application.targetFrameRate = FPSTarget;
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


    public void ResetLevel()
    {
        IsGameRunning = false;
        OnSetupGame?.Invoke();
        OnShowHomeUI?.Invoke();
        OnReset?.Invoke();
    }
    public void Revive()
    {
        IsGameRunning = true;
        //OnSetupGame?.Invoke();
        //OnShowHomeUI?.Invoke();
        OnRevive?.Invoke();
    }
}
