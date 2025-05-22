using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour
{
    #region Variables
    public static PlayerData Instance;
    private const string CURRENT_SKIN_KEY = "CurrentSkin";
    private const string UNLOCKED_SKINS_KEY = "UnlockedSkins";
    private const string CURRENT_MONEY_KEY = "MoneyAmount";
    private const string CURRENT_REWARD_PROGRESS_KEY = "RewardProgress";


    public int CurrentSkinId { get; private set; }
    private List<int> SkinUnlocked = new List<int>();
    public Action OnSkinChange;
    public Action OnSkinUnlocked;

    public int CurrentMoney { get; private set; }
    public int CurrentRewardRate { get; private set; }
    public Action OnMoneyChange;
    public Action<int> OnAddMoney;
    public Action<int> OnShowEffectAddMoney;
    #endregion

    #region Unity
    private void Awake()
    {
        Instance = this;
    }
    private IEnumerator Start()
    {

        //load Skin
        CurrentSkinId = PlayerPrefs.GetInt(CURRENT_SKIN_KEY, 0);
        LoadSkinUnlock();

        //Always unlock
        UnlockSkin(0);

        //load Money
        CurrentMoney = PlayerPrefs.GetInt(CURRENT_MONEY_KEY, 0);

        //Reward
        LoadRewardProgress();

        yield return null;
        OnSkinChange?.Invoke();
        OnMoneyChange?.Invoke();
    }
    #endregion

    #region Skin
    private void LoadSkinUnlock()
    {
        string data = PlayerPrefs.GetString(UNLOCKED_SKINS_KEY, "");
        string[] ids = data.Split(',');
        SkinUnlocked = new List<int>();
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i].Length > 0)
                SkinUnlocked.Add(int.Parse(ids[i]));
        }
    }
    private void SaveSkinUnlock()
    {
        string data = "";
        for (int i = 0; i < SkinUnlocked.Count; i++) data += (i == 0 ? "" : ",") + SkinUnlocked[i].ToString();
        PlayerPrefs.SetString(UNLOCKED_SKINS_KEY, data);
    }
    public void UseSkin(int SkinId)
    {
        CurrentSkinId = SkinId;
        PlayerPrefs.SetInt(CURRENT_SKIN_KEY, CurrentSkinId);
        OnSkinChange?.Invoke();
    }
    public bool HaveSkin(int SkinId)
    {
        return SkinUnlocked.Contains(SkinId);
    }
    public void UnlockSkin(int SkinId)
    {
        SkinUnlocked.Add(SkinId);
        OnSkinUnlocked?.Invoke();
        SaveSkinUnlock();

    }
    #endregion

    #region Money
    private void SaveMoney()
    {
        PlayerPrefs.SetInt(CURRENT_MONEY_KEY, CurrentMoney);

    }
    public bool HaveEnoughMoney(int price)
    {
        return CurrentMoney >= price;
    }

    public void MinusMoney(int count, bool callEvent = true)
    {
        if (HaveEnoughMoney(count))
        {
            CurrentMoney -= count;
            SaveMoney();
            if (callEvent)
            {
                OnMoneyChange?.Invoke();
                OnAddMoney?.Invoke(-count);
            }
        }
    }
    public void AddMoney(int count, bool callEvent = true)
    {
        CurrentMoney += count;
        SaveMoney();
        if (callEvent)
        {
            OnMoneyChange?.Invoke();
            OnAddMoney?.Invoke(count);
        }
    }
    #endregion

    #region Reward
    private int CountReward = 0;
    public void LoadRewardProgress()
    {
        CurrentRewardRate = PlayerPrefs.GetInt(CURRENT_REWARD_PROGRESS_KEY, 0);
    }
    public void SaveRewardProgress()
    {
        PlayerPrefs.SetInt(CURRENT_REWARD_PROGRESS_KEY, CurrentRewardRate);
    }
    public void AddRewardProgress(int value)
    {
        CurrentRewardRate += value;
        if (CurrentRewardRate >= 100)
        {
            CurrentRewardRate %= 100;
            CountReward++;
        }
        SaveRewardProgress();
    }
    public int GetRewardId()
    {
        Debug.LogWarning("Khi nao co day du GD thi fix cho nay");
        int result = ((LevelManager.Instance.LevelIndex - 2) / 4 + 1);
        if (result >= GameConfig.PLAYER_CARS_COUNT)
        {
            return -1;
        }
        return result;
    }
    public bool CanTakeReward()
    {
        return CountReward >= 1;
    }
    public void TakeReward()
    {
        if (CanTakeReward())
        {
            CountReward--;
            if (GetRewardId() >= 0)
            {
                UnlockSkin(GetRewardId());
                UseSkin(GetRewardId());
            }
            else
            {
                AddMoney(GameConfig.REWARD_MONEY, false);
                OnShowEffectAddMoney?.Invoke(GameConfig.REWARD_MONEY);
            }
        }
    }
    #endregion
}
