using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Simple Singleton
    public static LevelManager Instance;

    public Action OnLevelChange;
    public Action OnLoadLevelComplete;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    private string LevelKey = "CurrentLevel";
    public int MapIndex { get; private set; }
    public int LevelIndex { get; private set; }

    public MapData CurrentMapData { get; private set; }
    public LevelData CurrentLevelData { get; private set; }
    #endregion

    #region Unity Behaviour

    private void Start()
    {
        GameManager.Instance.OnSetupGame += LoadLevel;
        GameManager.Instance.OnNextLevel += NextLevel;
        GameManager.Instance.OnReset += LoadLevel;
    }
    private void Update()
    {
        OnLevelChange?.Invoke();
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnSetupGame -= LoadLevel;
        GameManager.Instance.OnNextLevel -= NextLevel;
        GameManager.Instance.OnReset -= LoadLevel;
    }
    public void LoadLevel()
    {
        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1);
        MapIndex = LevelIndex / 10 + 1;
        OnLevelChange?.Invoke();
        CurrentLevelData = Resources.Load<LevelData>($"Data/Level/Level_{(LevelIndex - 1) % 10 + 1}/Level{(LevelIndex - 1) % 10 + 1}");
        OnLoadLevelComplete?.Invoke();

        CreateObstacles();

    }
    public void NextLevel()
    {
        LevelIndex = PlayerPrefs.GetInt(LevelKey, 1) + 1;
        PlayerPrefs.SetInt(LevelKey, LevelIndex);
        MapIndex = LevelIndex / GameConfig.LEVEL_PER_MAP + 1;
        OnLevelChange?.Invoke();
        LoadLevel();
    }

    #endregion

    #region Setup Start Game
    private List<GameObject> Obstacles = new List<GameObject>();
    private void CreateObstacles()
    {
        //clear cache
        for (int i = 0; i < Obstacles.Count; i++) Destroy(Obstacles[i]);
        Obstacles.Clear();

        //Create Obstacle
        GameObject ObstaclePrefab = Resources.Load<GameObject>(GameConfig.OBSTACLE_PREFAB_LINK);
        if (ObstaclePrefab != null)
        {
            for (int i = 0; i < CurrentLevelData.Obstacles.Count; i++)
            {
                GameObject newObstacle = Instantiate(ObstaclePrefab, Vector3.zero, Quaternion.identity);
                Obstacles.Add(newObstacle);
                newObstacle.GetComponent<Obstacle>().SetObstacleData(CurrentLevelData.Obstacles[i]);
            }
        }

    }
    #endregion

    #region Reset End Game

    #endregion
}
