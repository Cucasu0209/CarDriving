using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Simple Singleton
    public static LevelManager Instance;

    public Action OnLevelChange;
    public Action OnUnlockNewMap;
    public Action OnLoadLevelComplete;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    private string LevelKey = "CurrentLevel";
    private string ShowedAnimLevelKey = "ShowedAnimLevel";
    public int LevelIndex { get; private set; }

    public LevelData CurrentLevelData { get; private set; }
    #endregion

    #region Unity Behaviour

    private void Start()
    {
        GameManager.Instance.OnSetupGame += LoadLevel;
        GameManager.Instance.OnNextLevel += NextLevel;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSetupGame -= LoadLevel;
        GameManager.Instance.OnNextLevel -= NextLevel;
    }
    public void LoadLevel()
    {

        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1);
        CurrentLevelData = Resources.Load<LevelData>($"Data/Level/Level_{(LevelIndex - 1) % 10 + 1}/Level{(LevelIndex - 1) % 10 + 1}");
        OnLoadLevelComplete?.Invoke();
        OnLevelChange?.Invoke();

        CreateObstacles();

        if (IsStartLevelOfNewMap() && PlayerPrefs.GetInt(ShowedAnimLevelKey, 1) < LevelIndex)
        {
            OnUnlockNewMap?.Invoke();
            PlayerPrefs.SetInt(ShowedAnimLevelKey, LevelIndex);
        }
    }
    public void NextLevel()
    {
        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1) + 1;
        PlayerPrefs.SetInt(LevelKey, LevelIndex);
    }
    public bool IsStartLevelOfNewMap()
    {
        return LevelIndex % GameConfig.LEVEL_PER_MAP == 1;
    }

    #endregion

    #region Setup Start Game
    private List<GameObject> Obstacles = new List<GameObject>();
    private void CreateObstacles()
    {
        //clear cache
        for (int i = 0; i < Obstacles.Count; i++) PoolingSystem.Despawn(Obstacles[i]);
        Obstacles.Clear();

        //Create Obstacle
        GameObject ObstaclePrefab = Resources.Load<GameObject>(GameConfig.OBSTACLE_PREFAB_LINK);
        if (ObstaclePrefab != null)
        {
            for (int i = 0; i < CurrentLevelData.Obstacles.Count; i++)
            {
                GameObject newObstacle = PoolingSystem.Spawn(ObstaclePrefab, Vector3.zero, Quaternion.identity);
                Obstacles.Add(newObstacle);
                newObstacle.GetComponent<Obstacle>().SetObstacleData(CurrentLevelData.Obstacles[i]);
            }
        }

    }
    #endregion

    #region Reset End Game
    #endregion
}
