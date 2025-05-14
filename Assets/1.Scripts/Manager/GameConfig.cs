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


    //Resource Link
    public static string SHOWROOM_DATA_LINK = "Data/ShowroomData";
    public static string SHOWROOM_ICON_LINK = "Vehicles/Icons/";
    public static string SHOWROOM_MODEL_LINK = "Vehicles/Models/";
    public static string CAR_MODEL_LINK = "Vehicles/Traps/";
    public static string HUMAN_MODEL_LINK = "Humans/Models/";
    public static string OBSTACLE_PREFAB_LINK = "GameplayPrefab/Obstacle";
    public static int SHOWROOM_ELEMENT_PER_PAGE = 9;

}
