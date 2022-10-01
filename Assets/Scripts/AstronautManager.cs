using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class AstronautManager : Singleton<AstronautManager>
{
    private static string playerPrefsKey = "astronaut_stats";

    public string gameOverScene;
    
    public Action onUpdate;

    public struct SaveData
    {
        public float food;
        public float water;
        public float fitness;

        public int saveTime;
    }

    public SaveData data = new SaveData()
    {
        food = 100f,
        water = 100f,
        fitness = 100f,
        saveTime = Epoch.Current()
    };

    [JsonIgnore]
    public float lossIntervalOnlineMinutes = 2f;
    [JsonIgnore]
    public float lossIntervalOfflineMinutes = 30f;

    [JsonIgnore]
    public float waterLossPerInterval = 2f;
    [JsonIgnore]
    public float foodLossPerInterval = 1f;
    [JsonIgnore]
    public float fitnessLossPerInterval = 1f;


    private void Start()
    {
        LoadStats();


        onUpdate += CheckFail;
        
        onUpdate.Invoke();

        StartCoroutine(UpdateData());
    }

    void CheckFail()
    {
        if (data.food <= 0 || data.water <= 0)
        {
            // Load game over scene
            SceneManager.LoadScene(gameOverScene);
        }
    }

            IEnumerator UpdateData()
    {
        while (true)
        {
            yield return new WaitForSeconds(lossIntervalOnlineMinutes * 60f);

            data.water -= waterLossPerInterval;
            data.food -= foodLossPerInterval;
            data.fitness -= fitnessLossPerInterval;

            data.water = Mathf.Clamp(data.water, 0f, 100f);
            data.food = Mathf.Clamp(data.food, 0f, 100f);
            data.fitness = Mathf.Clamp(data.fitness, 0f, 100f);

            SaveStats();

        }
    }
    
    public void SaveStats()
    {
        onUpdate.Invoke();

        // save to playerprefs
        data.saveTime = Epoch.Current();
        PlayerPrefs.SetString(playerPrefsKey, JsonConvert.SerializeObject(data));
    }

    public void LoadStats()
    {
        // load from playerprefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            SaveData loadedData = JsonConvert.DeserializeObject<SaveData>(PlayerPrefs.GetString(playerPrefsKey));

            // calculate new values
            data = ApplyTime(loadedData);
        }
    }

    SaveData ApplyTime(SaveData data)
    {
        // calculate time elapsed since last save
        int secondsElapsed = Epoch.SecondsElapsed(data.saveTime);

        // calculate how many intervals have passed
        int intervals = Mathf.FloorToInt(secondsElapsed / (lossIntervalOfflineMinutes * 60f));

        // apply loss
        data.food -= intervals * foodLossPerInterval;
        data.water -= intervals * waterLossPerInterval;
        data.fitness -= intervals * fitnessLossPerInterval;

        // clamp values
        data.food = Mathf.Clamp(data.food, 0f, 100f);
        data.water = Mathf.Clamp(data.water, 0f, 100f);
        data.fitness = Mathf.Clamp(data.fitness, 0f, 100f);

        return data;
    }

}
