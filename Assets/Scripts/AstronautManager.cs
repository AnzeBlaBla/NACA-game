using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class AstronautManager : MonoBehaviour
{
    private static string playerPrefsKey = "astronaut_stats";
    
    public float food = 100;
    public float water = 100;
    public float fitness = 100;

    public void SaveStats()
    {
        // save to playerprefs
        PlayerPrefs.SetString(playerPrefsKey, JsonConvert.SerializeObject(this));
    }

    public void LoadStats()
    {
        // load from playerprefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            JsonConvert.PopulateObject(PlayerPrefs.GetString(playerPrefsKey), this);
        }
    }
}
