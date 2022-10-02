using UnityEngine;
using System.Collections;
using System;

public static class Epoch
{

    public static int Current()
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

        return currentEpochTime;
    }

    public static int SecondsElapsed(int t1)
    {
        int difference = Current() - t1;

        return Mathf.Abs(difference);
    }

    public static int SecondsElapsed(int t1, int t2)
    {
        int difference = t1 - t2;

        return Mathf.Abs(difference);
    }

    public static string SecondsToDisplay(int seconds)
    {
        int days = seconds / 86400;
        seconds -= days * 86400;
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds / 60;
        seconds -= minutes * 60;
        return days + "d " + hours + "h " + minutes + "m " + seconds + "s";
    }

    public static DateTime ToDateTime(int epoch)
    {
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime time = epochStart.AddSeconds(epoch);

        return time;
    }

    public static DateTime ToDateTime()
    {
        return ToDateTime(Current());
    }

}