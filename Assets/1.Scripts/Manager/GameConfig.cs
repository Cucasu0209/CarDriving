using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public static int MAX_LEVEL = 30;
    public static int LEVEL_PER_MAP = 10;
    public static string[] LOCATIONS_NAME = {
        "LONDON",
        "AMERICAN",
        "ITALIA"
    };

    public static float TIME_WAIT_LOSE_GAME = 10;
    public static int WIN_REWARD_MULTIPLIER_ADS = 3;


    //Resource Link
    public static string SHOWROOM_DATA_LINK = "Data/ShowroomData";
    public static string SHOWROOM_ICON_LINK = "Vehicles/SkinIcons/";
    public static string SHOWROOM_MODEL_LINK = "Vehicles/SkinModels/";
    public static string CAR_MODEL_LINK = "Vehicles/Traps/";
    public static string HUMAN_MODEL_LINK = "Humans/Models/";
    public static string OBSTACLE_PREFAB_LINK = "GameplayPrefab/Obstacle";
    public static int SHOWROOM_ELEMENT_PER_PAGE = 6;


    //PlayerPref Keys
    public static string MUSIC_KEY = "Setting_Music";
    public static string SOUND_KEY = "Setting_Sound";
    public static string VIBRATION_KEY = "Setting_VIBRARION";
}
