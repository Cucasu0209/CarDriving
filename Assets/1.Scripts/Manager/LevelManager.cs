using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Simple Singleton
    public static LevelManager Instance;
    public Player Player;

    public Action OnLevelChange;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    private string LevelKey = "CurrentLevel";
    public int MapIndex { get; private set; }
    [SerializeField] public int LevelIndex;/* { get; private set; }*/

    public MapData CurrentMapData { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    #endregion


    #region Unity Behaviour

    private void Start()
    {
        GameManager.Instance.OnSetupGame += LoadLevel;
    }
    private void Update()
    {
        OnLevelChange?.Invoke();
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSetupGame -= LoadLevel;
    }
    public void LoadLevel()
    {
        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1);
        MapIndex = LevelIndex / 10 + 1;
        OnLevelChange?.Invoke();

        CurrentMapData = Resources.Load<MapData>($"Data/Level/Level_{1}/Map{1}");
        CurrentLevelData = Resources.Load<LevelData>($"Data/Level/Level_{1}/Level{1}");
        Player.SetupTrace(CurrentLevelData.PlayerTrace);
    }
    public void NextLevel()
    {
        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1) + 1;
        PlayerPrefs.SetInt(LevelKey, LevelIndex);
        MapIndex = LevelIndex / GameConfig.LEVEL_PER_MAP + 1;
        OnLevelChange?.Invoke();
    }

    #endregion


}
