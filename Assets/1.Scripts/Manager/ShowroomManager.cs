using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowroomManager : MonoBehaviour
{
    #region Simple Singleton
    public static ShowroomManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Variables
    public int CurrentIdSelected { get; private set; }
    public int CurrentPageIndex { get; private set; }

    public ShowroomData CurrentShowroomData { get; private set; }

    //Events
    public Action OnLoadDataComplete;
    public Action<bool> OnPageChange;//true = Next, false = back
    public Action OnSelectElement;
    public Action<CarData> OnWantToBuyCar;

    #endregion

    #region Unity Behaviours
    private IEnumerator Start()
    {
        CurrentShowroomData = Resources.Load<ShowroomData>(GameConfig.SHOWROOM_DATA_LINK);
        CurrentPageIndex = 0;
        yield return null;
        CurrentIdSelected = PlayerData.Instance.CurrentSkinId;
        OnLoadDataComplete?.Invoke();
        OnPageChange?.Invoke(true);
        OnSelectElement?.Invoke();
    }


    #endregion

    #region Public Actions
    public void ChangePage(bool isNextPage)
    {
        CurrentPageIndex += (isNextPage ? 1 : -1);
        OnPageChange?.Invoke(isNextPage);
    }
    public Sprite GetCarIcon(int carID)
    {
        return Resources.Load<Sprite>(GameConfig.SHOWROOM_ICON_LINK + GetDataById(carID).CarName);
    }
    public GameObject GetCarModel(int carID)
    {
        return Resources.Load<GameObject>(GameConfig.SHOWROOM_MODEL_LINK + GetDataById(carID).CarName);
    }
    public CarData GetDataById(int id)
    {
        return CurrentShowroomData.CarListData.Find(x => x.Id == id);
    }
    public List<CarData> GetElementsInPage(int pageIndex)
    {
        List<CarData> carInPage = new List<CarData>();

        for (int i = pageIndex * GameConfig.SHOWROOM_ELEMENT_PER_PAGE; i < Mathf.Min((pageIndex + 1) * GameConfig.SHOWROOM_ELEMENT_PER_PAGE, CurrentShowroomData.CarListData.Count); i++)
        {
            carInPage.Add(CurrentShowroomData.CarListData[i]);
        }
        return carInPage;
    }
    public void SelectElement(int id)
    {
        CurrentIdSelected = id;
        OnSelectElement?.Invoke();
    }
    public int GetPageCount() => CurrentShowroomData.CarListData.Count / GameConfig.SHOWROOM_ELEMENT_PER_PAGE + 1;
    public void BuyCar(CarData data)
    {
        if (PlayerData.Instance.HaveEnoughMoney(data.Price))
        {
            PlayerData.Instance.MinusMoney(data.Price);
            PlayerData.Instance.UnlockSkin(data.Id);
            PlayerData.Instance.UseSkin(data.Id);

        }
    }
    #endregion


}
