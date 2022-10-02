using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

public class StorageManager : Singleton<StorageManager>
{
    public static string playerPrefsKey = "storage_items";


    public float filterInterval = 5f;

    public Action onUpdate;

    public int gardenEmptyReminderTimeMinutes = 60;


    public struct GardenSlotStruct
    {
        public int index;
        public int saveTime;
        public int growTime;
    }

    public struct DataStruct
    {
        public List<GardenSlotStruct> gardenSlots;
        public int foodAmount;
        public float filteredWater;
        public float unfilteredWater;
        public int saveTime;
    }

    public DataStruct data = new DataStruct()
    {
        gardenSlots = new List<GardenSlotStruct>()
        {
            new GardenSlotStruct() { index = 0, saveTime = 0, growTime = 10 },
            new GardenSlotStruct() { index = 1, saveTime = 0, growTime = 10 },
            new GardenSlotStruct() { index = 2, saveTime = 0, growTime = 10 },
            new GardenSlotStruct() { index = 3, saveTime = 0, growTime = 10 },
        },
        foodAmount = 5,
        filteredWater = 5f,
        unfilteredWater = 0f,
        saveTime = Epoch.Current()
    };

    private void Start()
    {
        LoadData();

        StartCoroutine(FilterWater());
    }

    IEnumerator FilterWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(filterInterval);

            //Debug.Log("Filtering water: " + data.unfilteredWater);

            if (data.unfilteredWater > 0)
            {
                data.unfilteredWater -= 1f;
                data.filteredWater += 1f;

                SaveData();
            }
        }
    }


    public void SaveData()
    {
        data.saveTime = Epoch.Current();
        Debug.Log(JsonConvert.SerializeObject(data));
        onUpdate?.Invoke();
        PlayerPrefs.SetString(playerPrefsKey, JsonConvert.SerializeObject(data));
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            DataStruct loadedData = JsonConvert.DeserializeObject<DataStruct>(PlayerPrefs.GetString(playerPrefsKey));
            data = ApplyTime(loadedData);
        }
    }

    public DataStruct ApplyTime(DataStruct data)
    {
        // apply water filter

        int timePassed = Epoch.Current() - data.saveTime;

        if (timePassed > 0)
        {
            int filterCount = (int)(timePassed / filterInterval);

            if (filterCount > 0)
            {
                if (filterCount > data.unfilteredWater)
                {
                    filterCount = (int)data.unfilteredWater;
                }

                data.unfilteredWater -= filterCount;
                data.filteredWater += filterCount;
            }
        }


        return data;
    }

    public float GetData(string key)
    {
        switch (key)
        {
            case "foodAmount":
                return data.foodAmount;
            case "filteredWater":
                return data.filteredWater;
            case "unfilteredWater":
                return data.unfilteredWater;
            default:
                return 0;
        }
    }

    public void SetData(string key, float value)
    {
        switch (key)
        {
            case "foodAmount":
                data.foodAmount = (int)value;
                break;
            case "filteredWater":
                data.filteredWater = value;
                break;
            case "unfilteredWater":
                data.unfilteredWater = value;
                break;
        }

        SaveData();
    }

    public void SetData(string key, int value)
    {
        switch (key)
        {
            case "foodAmount":
                data.foodAmount = value;
                break;
        }

        SaveData();
    }

    public void ChangeData(string key, float value)
    {
        switch (key)
        {
            case "foodAmount":
                data.foodAmount += (int)value;
                break;
            case "filteredWater":
                data.filteredWater += value;
                break;
            case "unfilteredWater":
                data.unfilteredWater += value;
                break;
        }

        SaveData();
    }

    public void ChangeData(string key, int value)
    {
        switch (key)
        {
            case "foodAmount":
                data.foodAmount += value;
                break;
        }

        SaveData();
    }

}

