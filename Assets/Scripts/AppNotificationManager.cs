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


    //public TextMeshProUGUI notificationScheduledText;
    void Start()
    {
        //StartCoroutine(DisplayPendingNotifications());

        AstronautManager.Instance.onUpdate += ScheduleOfflineNotifications;
        StorageManager.Instance.onUpdate += ScheduleOfflineNotifications;
    }

    /*IEnumerator DisplayPendingNotifications()
    {
        while (true)
        {
            Debug.Log("Updating pending notifications");
            StringBuilder notificationStringBuilder = new StringBuilder("Pending notifications at:");
            notificationStringBuilder.AppendLine();
            for (int i = manager.PendingNotifications.Count - 1; i >= 0; --i)
            {
                PendingNotification queuedNotification = manager.PendingNotifications[i];
                DateTime? time = queuedNotification.Notification.DeliveryTime;
                if (time != null)
                {
                    notificationStringBuilder.Append($"{time:dd.MM.yyyy HH:mm:ss}");
                    // title
                    notificationStringBuilder.Append($" - {queuedNotification.Notification.Title}");
                    notificationStringBuilder.Append($" - {queuedNotification.Notification.Id}");
                    notificationStringBuilder.AppendLine();
                }
            }
            notificationScheduledText.text = notificationStringBuilder.ToString();

            yield return new WaitForSeconds(1f);
        }
    }*/


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
        if (manager.PendingNotifications == null)
        {
            return;
        }
        GameNotificationChannel channel = notificationChannels[channelName].channel;

        List<int> cancelIds = new List<int>();
        foreach (var notification in manager.PendingNotifications)
        {
            if (notification.Notification.Group == channel.Id)
            {
                cancelIds.Add((int)notification.Notification.Id);
            }
        }

        foreach (var id in cancelIds)
        {
            manager.CancelNotification(id);
        }
    }

    public void ScheduleOfflineNotifications()
    {
        CancelAllNotifications();

        StorageManager sm = StorageManager.Instance;

        // if garden empty
        bool isEmpty = true;

        foreach (var slot in sm.data.gardenSlots)
        {
            if (slot.saveTime > 0)
            {
                isEmpty = false;
                break;
            }
        }

        if (isEmpty)
        {
            SendNotification(new Notification()
            {
                title = "Garden is empty!",
                description = "The garden is empty. You should plant some seeds.",
                deliveryTime = Epoch.ToDateTime(Epoch.Current() + (int)(sm.gardenEmptyReminderTimeMinutes * 60)),
                channel = notificationChannels["ship"].id,
                badgeNumber = 1,
            });

        }
        else
        {
            // calculate when it will be all ready

            int readyTime = 0;

            foreach (var slot in sm.data.gardenSlots)
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

            // if ready time is more than 30sec away
            if (readyTime > 30)
            {
                SendNotification(new Notification()
                {
                    title = "Garden is ready!",
                    description = "The garden is ready. You should harvest it.",
                    deliveryTime = Epoch.ToDateTime(Epoch.Current() + readyTime),
                    channel = notificationChannels["ship"].id,
                    badgeNumber = 1,
                });
            }

        }

        // Astronaut notifications

        AstronautManager am = AstronautManager.Instance;

        float notifAtValue = 20f;

        CancelChannelNotifications("astronaut");

        // Food notification
        if (am.data.food > notifAtValue)
        {
            float foodToNotif = am.data.food - notifAtValue;

            DateTime notifAt = Epoch.ToDateTime().AddMinutes(foodToNotif / am.foodLossPerInterval * am.lossIntervalOfflineMinutes);

            SendNotification(new Notification()
            {
                title = "Yuri is hungry!",
                description = "Make sure Yuri eats something...",
                deliveryTime = notifAt,
                channel = notificationChannels["astronaut"].id,
                badgeNumber = 1,
            });
        }
        else
        {
            // Calculate when it will reach zero
            DateTime notifAt = Epoch.ToDateTime().AddMinutes((am.data.food / am.foodLossPerInterval * am.lossIntervalOfflineMinutes));

            SendNotification(new Notification()
            {
                title = "Yuri is very hungry!",
                description = "Make sure Yuri eats something...",
                deliveryTime = notifAt,
                channel = notificationChannels["astronaut"].id,
                badgeNumber = 1,
            });
        }

        // Water notification

        if (am.data.water > notifAtValue)
        {
            float waterToNotif = am.data.water - notifAtValue;

            DateTime notifAt = Epoch.ToDateTime().AddMinutes(waterToNotif / am.waterLossPerInterval * am.lossIntervalOfflineMinutes);

            // Schedule notifications
            SendNotification(new Notification()
            {
                title = "Yuri is thirsty!",
                description = "Make sure Yuri drinks something...",
                deliveryTime = notifAt,
                channel = notificationChannels["astronaut"].id,
                badgeNumber = 1,
            });
        }
        else
        {
            // Calculate when it will reach zero

            DateTime notifAt = Epoch.ToDateTime().AddMinutes((am.data.water / am.waterLossPerInterval * am.lossIntervalOfflineMinutes));

            SendNotification(new Notification()
            {
                title = "Yuri is very thirsty!",
                description = "Make sure Yuri drinks something...",
                deliveryTime = notifAt,
                channel = notificationChannels["astronaut"].id,
                badgeNumber = 1,
            });
        }
    }

}
