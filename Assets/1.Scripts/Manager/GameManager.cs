using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsGameRunning { get; private set; }

    //Events
    public Action OnGameStart;
    public Action<Transform> OnPickCustomer;
    public Action OnFinishTrace;

    public Action<float> OnUpdateProgress;
    public Action<float> OnUpdatePickupPoint;


    private void Awake()
    {
        IsGameRunning = false;
        Instance = this;
    }

    public void StartGame()
    {
        IsGameRunning = true;
        OnGameStart?.Invoke();
    }

}
