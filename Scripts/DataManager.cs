using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("DataManager를 처음 선언하여 객체를 생성합니다");
                var go = new GameObject("DataManager(Singleton)");
                _instance = go.AddComponent<DataManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private int _highscore;

    public int highScore
    {
        get
        {
            return _highscore;
        }
        set
        {
            if (value < 0)
            {
                Debug.LogError("하이스코어 갱신되지 않음(value < 0)");
                return;
            }

            _highscore = value;
        }
    }

    private int _carSelected;

    public int carSelected
    {
        get
        {
            return _carSelected;
        }
        set
        {
            if (value < 0)
            {
                Debug.LogError("CarSelected 변수 값 세팅 오류(value < 0)");
                return;
            }

            _carSelected = value;
        }
    }
    public List<int> carObtained;

    private int _coin;

    public int coin
    {
        get
        {
            return _coin;
        }
        set
        {
            if (value < 0)
            {
                Debug.LogError("코인 값 이상(value < 0)");
                return;
            }
            _coin = value;
        }
    }
    private string flag; //0,1,2,3,4,5,

    void OnEnable()
    {
        carObtained = new List<int>();
        highScore = 0;
        coin = 1000;
        carSelected = 0;
        flag = "0,";

        Debug.Log(PlayerPrefs.HasKey("Coin"));

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScore = PlayerPrefs.GetInt("HighScore");
            coin = PlayerPrefs.GetInt("Coin");
            carSelected = PlayerPrefs.GetInt("CarSelected", carSelected);
            flag = PlayerPrefs.GetString("CarObtained", flag);
        }
        else
            SaveGameData();

        for (int i = 0; i < flag.Split(',').Length - 1; i++)
            carObtained.Add(int.Parse(flag.Split(',')[i]));
        //PlayerPrefs.DeleteAll();
    }

    void OnDisable()
    {
        Debug.Log("DataManager 삭제");
        _instance = null;
        Destroy(gameObject);
    }

    public void SaveGameData()
    {
        carObtained.Sort();
        flag = "";

        for (int i = 0; i < carObtained.Count; i++)
        {
            flag += carObtained[i].ToString() + ",";
        }

        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("CarSelected", carSelected);
        PlayerPrefs.SetString("CarObtained", flag);
        PlayerPrefs.Save();

        Debug.Log("All Data Saved!!!");
    }
}
