using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Simple Singleton
    public static LevelManager Instace;

    private void Awake()
    {
        Instace = this;
    }
    #endregion

    #region Variables
    private int MapIndex;
    private int LevelIndex;

    private MapData CurrentMapData;
    private LevelData CurrentLevelData;
    #endregion


    #region Unity Behaviour
    private void LoadLevel()
    {

    }

    #endregion


}
