using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{

    public static string[] LOCATIONS_NAME = {
        "LONDON",
        "AMERICAN",
        "ITALIA"
    };


    //Relate to ads
    public static float TIME_WAIT_LOSE_GAME = 5;
    public static int WIN_REWARD_MULTIPLIER_ADS = 3;


    //Resource Link
    public static string SHOWROOM_DATA_LINK = "Data/ShowroomData";
    public static string SKIN_ICON_LINK = "Vehicles/SkinIcons/";
    public static string SKIN_MODEL_LINK = "Vehicles/SkinModels/";
    public static string SKIN_SHADOW_LINK = "Vehicles/SkinShadows/";
    public static string CAR_TRAP_LINK = "Vehicles/Traps/";
    public static string HUMAN_MODEL_LINK = "Humans/Models/";
    public static string OBSTACLE_PREFAB_LINK = "GameplayPrefab/Obstacle";
    public static int SHOWROOM_ELEMENT_PER_PAGE = 6;

    //Name In Prefab
    public static string TRAP_CAR_NAME = "Car";
    public static string TRAP_HUMAN_NAME = "Human";
    public static string PLAYER_CAR_MODEL_NAME = "Car";
    public static string PLAYER_CAR_ICON_NAME = "Car";
    public static string PLAYER_CAR_SHADOW_NAME = "Car";
    public static string REWARD_MONEY_ICON_NAME = "Money";
    public static string REWARD_MONEY_SHADOW_NAME = "Money";

    //Count
    public static int MAX_LEVEL = 30;
    public static int LEVEL_PER_MAP = 10;
    public static int PLAYER_CARS_COUNT = 9;
    public static int TRAP_CARS_COUNT = 8;
    public static int TRAP_HUMANS_COUNT = 8;

    //PlayerPref Keys
    public static string MUSIC_KEY = "Setting_Music";
    public static string SOUND_KEY = "Setting_Sound";
    public static string VIBRATION_KEY = "Setting_Vibration";
}
