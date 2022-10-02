using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.SceneManagement;

public class AstronautManager : Singleton<AstronautManager>
{

    public bool placeholder = false;

    private static string playerPrefsKey = "astronaut_stats";

    public string mainScene;
    public string gameOverScene;

    public Action onUpdate;

    public struct SaveData
    {
        public float food;
        public float water;
        public float fitness;
        public float bladder;

        public int saveTime;
    }

    public SaveData data = new SaveData()
    {
        food = 100f,
        water = 100f,
        fitness = 100f,
        bladder = 0f,
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
        if (placeholder)
        {
            return;
        }

        LoadStats();


        onUpdate += OnDataUpdate;

        onUpdate.Invoke();

        StartCoroutine(UpdateData());
    }

    void OnDataUpdate()
    {
        // Check fail
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
            data.bladder += waterLossPerInterval;
            data.fitness -= fitnessLossPerInterval;

            ClampStats();

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
        data.bladder += intervals * waterLossPerInterval;
        data.fitness -= intervals * fitnessLossPerInterval;

        ClampStats();

        return data;
    }

    public void ChangeStat(string name, float change)
    {
        switch (name)
        {
            case "food":
                data.food += change;
                break;
            case "water":
                data.water += change;
                if (change < 0)
                {
                    data.bladder += change;
                }
                break;
            case "fitness":
                if (data.fitness < 100f)
                {
                    data.fitness += change;
                    data.water -= change * 0.5f;
                    data.food -= change * 0.5f;
                }
                break;
        }

        ClampStats();

        SaveStats();
    }

    public float GetStat(string name)
    {
        switch (name)
        {
            case "food":
                return data.food;
            case "water":
                return data.water;
            case "fitness":
                return data.fitness;
            case "bladder":
                return data.bladder;
        }

        return 0f;
    }

    void ClampStats()
    {
        data.food = Mathf.Clamp(data.food, 0f, 100f);
        data.water = Mathf.Clamp(data.water, 0f, 100f);
        data.fitness = Mathf.Clamp(data.fitness, 0f, 100f);
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteKey(playerPrefsKey);
        PlayerPrefs.DeleteKey(MissionTimerManager.playerPrefsKey);
        PlayerPrefs.DeleteKey(StorageManager.playerPrefsKey);

        SceneManager.LoadScene(mainScene);

        SceneManager.UnloadSceneAsync(gameOverScene);
    }
}
