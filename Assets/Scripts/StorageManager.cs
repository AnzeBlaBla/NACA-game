using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class StorageManager : Singleton<StorageManager>
{
    private static string playerPrefsKey = "storage_items";


    public float filterInterval = 5f;

    public Action onUpdate;


    public struct GardenSlot
    {
        public int index;
        public int saveTime;
        public int growTime;
    }

    public struct SaveData
    {
        public List<GardenSlot> gardenSlots;
        public int foodAmount;
        public float filteredWater;
        public float unfilteredWater;
    }

    public SaveData data = new SaveData()
    {
        gardenSlots = new List<GardenSlot>()
        {
            new GardenSlot() { index = 0, saveTime = 0, growTime = 10 },
            new GardenSlot() { index = 1, saveTime = 0, growTime = 10 },
            new GardenSlot() { index = 2, saveTime = 0, growTime = 10 },
            new GardenSlot() { index = 3, saveTime = 0, growTime = 10 },
        },
        foodAmount = 5,
        filteredWater = 5f,
        unfilteredWater = 0f
    };

    private void Start()
    {
        LoadStats();

        StartCoroutine(FilterWater());
    }

    IEnumerator FilterWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(filterInterval);

            Debug.Log("Filtering water: " + data.unfilteredWater);

            if (data.unfilteredWater > 0)
            {
                data.unfilteredWater -= 1f;
                data.filteredWater += 1f;

                SaveStats();
            }
        }
    }


    public void SaveStats()
    {
        onUpdate?.Invoke();
        PlayerPrefs.SetString(playerPrefsKey, JsonConvert.SerializeObject(data));
    }

    public void LoadStats()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            data = JsonConvert.DeserializeObject<SaveData>(PlayerPrefs.GetString(playerPrefsKey));
        }
    }

    SaveData ApplyTime(SaveData data)
    {
        for (int i = 0; i < data.gardenSlots.Count; i++)
        {
            GardenSlot slot = data.gardenSlots[i];

            if (slot.saveTime != 0)
            {
                int timePassed = Epoch.Current() - slot.saveTime;
                int timeLeft = slot.growTime - timePassed;
            }
        }

        return data;
    }
}

