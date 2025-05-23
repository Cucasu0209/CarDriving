﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CameraMode
{
    Gameplay,
    Showroom,
    Reward
}
public class GameManager : MonoBehaviour
{
    public int FPSTarget = 60;
    public static GameManager Instance;
    public bool IsGameRunning { get; private set; }
    public CameraMode CurrentCameraMode { get; private set; }

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
    public Action OnChangeCameraMode;

    private void Awake()
    {
        IsGameRunning = false;
        Instance = this;
    }
    private IEnumerator Start()
    {
        Application.targetFrameRate = FPSTarget;
        yield return null;
        SetupLevel();
    }
    public void StartGame()
    {
        IsGameRunning = true;
        SoundManager.Instance.SwitchToGameplay();
        OnGameStart?.Invoke();
        OnHideHomeUI?.Invoke();
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        CurrentCameraMode = mode;
        OnChangeCameraMode?.Invoke();
    }

    public void SetupLevel()
    {
        IsGameRunning = false;

        SoundManager.Instance.SwitchToMainMenuBGM();
        OnSetupGame?.Invoke();
        OnShowHomeUI?.Invoke();
    }
    public void Revive()
    {
        SoundManager.Instance.SwitchToGameplay();
        IsGameRunning = true;
        OnRevive?.Invoke();
    }
    public void EndGame(bool isWin)
    {
        SoundManager.Instance.StopBGMusic();

        IsGameRunning = false;
        OnEndGame?.Invoke(isWin);
    }
}
