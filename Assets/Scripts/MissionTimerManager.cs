using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TMPro;
public class MissionTimerManager : MonoBehaviour
{
    public static string playerPrefsKey = "mission_start_time";
    int missionStartTime;

    public TMP_Text timerText;

    void Awake()
    {
        // load from playerprefs
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            missionStartTime = PlayerPrefs.GetInt(playerPrefsKey);
        }
        else
        {
            missionStartTime = Epoch.Current();
            PlayerPrefs.SetInt(playerPrefsKey, missionStartTime);
        }

        StartCoroutine(UpdateDataLoop());
    }
    
    public string GetMissionTime()
    {
        int seconds = Epoch.Current() - missionStartTime;
        return Epoch.SecondsToDisplay(seconds);
    }

    IEnumerator UpdateDataLoop()
    {
        while (true)
        {
            UpdateUI();

            yield return new WaitForSeconds(1f);
        }
    }

    void UpdateUI()
    {
        timerText.text = GetMissionTime();
    }
}
