using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField] private GameObject Foggy;
    [SerializeField] private GameObject Rainny;


    private void Start()
    {
        LevelManager.Instance.OnLoadLevelComplete += UpdateWeather;
    }
    private void OnDestroy()
    {
        LevelManager.Instance.OnLoadLevelComplete -= UpdateWeather;

    }


    private void UpdateWeather()
    {
        Foggy.SetActive(LevelManager.Instance.CurrentLevelData.Weather == WeatherType.Foggy);
        Rainny.SetActive(LevelManager.Instance.CurrentLevelData.Weather == WeatherType.Rainny);
    }
}
