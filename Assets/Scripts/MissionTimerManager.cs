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
        // Display in "1d 2h 3m 4s" format
        int seconds = Epoch.Current() - missionStartTime;
        int days = seconds / 86400;
        seconds -= days * 86400;
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds / 60;
        seconds -= minutes * 60;
        return days + "d " + hours + "h " + minutes + "m " + seconds + "s";
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
