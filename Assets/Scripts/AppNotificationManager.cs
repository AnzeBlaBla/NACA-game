using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NotificationSamples;
using System;
using System.Text;
using TMPro;


public class AppNotificationManager : Singleton<AppNotificationManager>
{
    [SerializeField, Tooltip("Reference to the notification manager.")]
    public GameNotificationsManager manager;

    public class NotificationChannel
    {
        public string id;
        public string title;
        public string description;
        public GameNotificationChannel channel;
    }

    public struct Notification
    {
        public string title;
        public string description;
        public DateTime deliveryTime;
        public string channel;
        public int badgeNumber;
        public string smallIconName;
        public string largeIconName;
    }

    public Dictionary<string, NotificationChannel> notificationChannels = new Dictionary<string, NotificationChannel>()
    {
        {
            "astronaut",
            new NotificationChannel()
            {
                id = "astronaut",
                title = "Yuri needs your help!",
                description = "Yuri is struggling."
            }
        },
        {
            "ship",
            new NotificationChannel()
            {
                id = "ship",
                title = "Something on the ship needs maintenance!",
                description = "Something on the ship needs maintenance."
            }
        }
    };

    public static string smallIconName = "app_icon_small";
    public static string largeIconName = "app_icon_large";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        InitializeChannels();

    }

    private void InitializeChannels()
    {
        GameNotificationChannel[] channels = new GameNotificationChannel[notificationChannels.Count];

        int i = 0;

        foreach (var channel in notificationChannels)
        {
            channels[i] = new GameNotificationChannel(channel.Value.id, channel.Value.title, channel.Value.description);

            notificationChannels[channel.Key].channel = channels[i];

            i++;
        }

        manager.Initialize(channels);
    }

    public void CancelAllNotifications()
    {
        manager.CancelAllNotifications();
    }

    public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null, string channelId = null, string smallIcon = null, string largeIcon = null)
    {
        IGameNotification notification = manager.CreateNotification();
        if (notification == null)
        {
            return;
        }
        notification.Title = title;
        notification.Body = body;
        notification.Group = channelId;
        notification.DeliveryTime = deliveryTime;
        notification.SmallIcon = !string.IsNullOrEmpty(smallIcon) ? smallIcon : smallIconName;
        notification.LargeIcon = !string.IsNullOrEmpty(largeIcon) ? largeIcon : largeIconName;
        if (badgeNumber != null)
        {
            notification.BadgeNumber = badgeNumber;
        }
        PendingNotification notificationToDisplay = manager.ScheduleNotification(notification);
        //
        Debug.Log($"Queued notification for unactivity with ID \"{notification.Id}\" at time {deliveryTime:dd.MM.yyyy HH:mm:ss}");
    }

    public void SendNotification(Notification notification)
    {
        SendNotification(notification.title, notification.description, notification.deliveryTime, notification.badgeNumber, notification.channel, notification.smallIconName, notification.largeIconName);
    }

    public void CancelChannelNotifications(string channelName)
    {
        GameNotificationChannel channel = notificationChannels[channelName].channel;

        foreach (var notification in manager.PendingNotifications)
        {
            if (notification.Notification.Group == channel.Id)
            {
                manager.CancelNotification((int)notification.Notification.Id);
            }
        }
    }

}
