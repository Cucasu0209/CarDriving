using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Simple Singleton
    public static LevelManager Instance;
    public Player Player;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    private int MapIndex = 1;
    private int LevelIndex = 1;

    public MapData CurrentMapData { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    #endregion


    #region Unity Behaviour

    private void Start()
    {
        GameManager.Instance.OnSetupGame += LoadLevel;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSetupGame -= LoadLevel;

    }
    public void LoadLevel()
    {
        CurrentMapData = Resources.Load<MapData>($"Data/Level/Level_{MapIndex}/Map{MapIndex}");
        CurrentLevelData = Resources.Load<LevelData>($"Data/Level/Level_{LevelIndex}/Level{LevelIndex}");
        Player.SetupTrace(CurrentLevelData.PlayerTrace);
    }

    #endregion


}
