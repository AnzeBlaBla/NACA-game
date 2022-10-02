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


    public struct GardenSlot
    {
        public int index;
        public int saveTime;
        public int growTime;
    }

    public struct DataStruct
    {
        public List<GardenSlot> gardenSlots;
        public int foodAmount;
        public float filteredWater;
        public float unfilteredWater;
    }

    public DataStruct data = new DataStruct()
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
        onUpdate += OnDataUpdate;
        LoadData();

        StartCoroutine(FilterWater());
    }

    void OnDataUpdate()
    {
        AppNotificationManager.Instance.CancelChannelNotifications("ship");


        // if garden empty

        bool isEmpty = true;

        foreach (var slot in data.gardenSlots)
        {
            if (slot.saveTime > 0)
            {
                isEmpty = false;
                break;
            }
        }

        if (isEmpty)
        {
            AppNotificationManager.Instance.SendNotification(new AppNotificationManager.Notification()
            {
                title = "Garden is empty!",
                description = "The garden is empty. You should plant some seeds.",
                deliveryTime = Epoch.ToDateTime(Epoch.Current() + (int)(gardenEmptyReminderTimeMinutes * 60)),
                channel = AppNotificationManager.Instance.notificationChannels["ship"].id,
                badgeNumber = 1,
            });

        } else
        {
            // calculate when it will be all ready

            int readyTime = 0;

            foreach (var slot in data.gardenSlots)
            {
                if (slot.saveTime > 0)
                {
                    int timeLeft = slot.saveTime + slot.growTime - Epoch.Current();

                    if (timeLeft > readyTime)
                    {
                        readyTime = timeLeft;
                    }
                }
            }

            AppNotificationManager.Instance.SendNotification(new AppNotificationManager.Notification()
            {
                title = "Garden is ready!",
                description = "The garden is ready. You should harvest it.",
                deliveryTime = Epoch.ToDateTime(Epoch.Current() + readyTime),
                channel = AppNotificationManager.Instance.notificationChannels["ship"].id,
                badgeNumber = 1,
            });
        }
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
        Debug.Log(JsonConvert.SerializeObject(data));
        onUpdate?.Invoke();
        PlayerPrefs.SetString(playerPrefsKey, JsonConvert.SerializeObject(data));
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            data = JsonConvert.DeserializeObject<DataStruct>(PlayerPrefs.GetString(playerPrefsKey));
        }
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

