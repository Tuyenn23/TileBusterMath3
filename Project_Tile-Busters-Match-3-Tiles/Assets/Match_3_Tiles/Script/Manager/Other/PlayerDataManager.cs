using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataManager
{
    [Header("zz")]
    private static string Sound = "SOUND";
    private static string Music = "MUSIC";
    private static string Coin = "COIN";
    private static string FirstTimePlay = "FirstTimePlay";
    private static string CompletedTut = "CompletedTut";

    private static string LAST_PLAY_TIME_KEY_HOME = "LastPlayTime";
    private static string PLAYER_TURNS_KEY = "PlayerTurns";

    private static string CLAIM_X5 = "CLAIM_X5";


    private static string ALL_DATA_LEVEL = "ALL_DATA_LEVEL";

    private static DATALEVEL DATALEVEL;


    static PlayerDataManager()
    {
        DATALEVEL = JsonConvert.DeserializeObject<DATALEVEL>(PlayerPrefs.GetString(ALL_DATA_LEVEL));

        if (DATALEVEL == null)
        {
            DATALEVEL = new DATALEVEL();
            DATALEVEL.SetLevel(1);
        }

        SaveDataLevel();
    }

    private static void SaveDataLevel()
    {
        string Json = JsonConvert.SerializeObject(DATALEVEL);
        PlayerPrefs.SetString(ALL_DATA_LEVEL, Json);
    }

    public static void SetLevel(int level)
    {
        DATALEVEL.SetLevel(level);
        SaveDataLevel();
    }

    public static int GetCurrentLevel()
    {
        return DATALEVEL.GetCurrentLevel();
    }

    public static void InCreaseLevel(int _currentLevel)
    {
        DATALEVEL.IncreaseLevel(_currentLevel);
        SaveDataLevel();
    }
    public static bool GetSound()
    {
        return PlayerPrefs.GetInt(Sound, 1) == 1;
    }

    public static void SetSound(bool isOn)
    {
        PlayerPrefs.SetInt(Sound, isOn ? 1 : 0);
    }

    public static bool GetMusic()
    {
        return PlayerPrefs.GetInt(Music, 1) == 1;
    }

    public static void SetMusic(bool isOn)
    {
        PlayerPrefs.SetInt(Music, isOn ? 1 : 0);
    }

    public static int GetCoin()
    {
        return PlayerPrefs.GetInt(Coin, 0);
    }

    public static void SetCoin(int QuantityCoin)
    {
        PlayerPrefs.SetInt(Coin, QuantityCoin);
    }

    public static bool GetIsClaimX5()
    {
        return PlayerPrefs.GetInt(CLAIM_X5, 0) == 1;
    }

    public static void SetClaimX5(bool IsClaim)
    {
        PlayerPrefs.SetInt(CLAIM_X5, IsClaim ? 1 : 0);
    }

    public static bool GetIsFirstTimePlay()
    {
        return PlayerPrefs.GetInt(FirstTimePlay, 0) == 1;
    }

    public static void SetIsFirstTimePlay(bool IsfirstTime)
    {
        PlayerPrefs.SetInt(FirstTimePlay, IsfirstTime ? 1 : 0);
    }

    public static bool GetCompletedTut()
    {
        return PlayerPrefs.GetInt(CompletedTut, 0) == 1;
    }

    public static void setCompletedTut(bool isComplete)
    {
        PlayerPrefs.SetInt(CompletedTut, isComplete ? 1 : 0);
    }

    public static int GetAmoutTurn()
    {
        return PlayerPrefs.GetInt(PLAYER_TURNS_KEY, 5);
    }

    public static void SetAmoutTurn(int AmoutTurn)
    {
        PlayerPrefs.SetInt(PLAYER_TURNS_KEY, AmoutTurn);
    }

    public static string GetLastPlayTimeHome()
    {
        return PlayerPrefs.GetString(LAST_PLAY_TIME_KEY_HOME, DateTime.Now.Ticks.ToString());
    }

    public static void SetLastPlayHome(string time)
    {
        PlayerPrefs.SetString(LAST_PLAY_TIME_KEY_HOME, time);
    }

    public static bool IsLastPlayTime()
    {
        return PlayerPrefs.HasKey(LAST_PLAY_TIME_KEY_HOME);
    }
}

public class DATALEVEL
{
    public int CurrentLevel;

    public void SetLevel(int level)
    {
        CurrentLevel = level;
    }

    public int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public void IncreaseLevel(int _CurrentLevel)
    {
        _CurrentLevel++;
        CurrentLevel = _CurrentLevel;
    }
}
