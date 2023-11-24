using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif


public class NotificationsController : MonoBehaviour
{
    public static NotificationsController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        DataSoundHolder.cannabisCoinMaxCapacity = PlayerPrefs.GetInt("CannabisCoinMaxCapacity", 0);
        _numCheckCannabisCoinMax = DataSoundHolder.cannabisCoinMaxCapacity;
        if (_numCheckCannabisCoinMax == 0)
        { isNotification = true; }
        else
        { isNotification = false; }

    }
    [SerializeField] public bool isNotification;
    [SerializeField] private int _numCheckCannabisCoinMax;
#if UNITY_ANDROID
    #region setting Android notification
    [SerializeField] public string notificationId = "";
    private int identifier;
    private void Start()
    {
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
        StartCoroutine(RequestNotificationPermission());
    }
    public void CreateAndroidNotificationChannel()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notification Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification();
        notification.Title = "Hey! Come Back!";
        notification.Text = "Play a Cannabisfarm game";
        notification.SmallIcon = "icon_id_1";
        notification.LargeIcon = "icon_id_2";
        notification.FireTime = System.DateTime.Now.AddSeconds(10);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        /*var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(id);

        if (notificationStatus == NotificationStatus.Scheduled)
        {
            // Replace the scheduled notification with a new notification.
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }*/
    }
    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        // here use request.Status to determine users response
        if (request.Status == PermissionStatus.Allowed)
        {
            CreateAndroidNotificationChannel();
            isNotification = true;
            _numCheckCannabisCoinMax = 0;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
        }
        else
        {
            isNotification = false;
            _numCheckCannabisCoinMax = 1;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
            yield break;
        }
    }
    #endregion
#endif

#if UNITY_IOS
    #region setting IOS notification
    public string notificationId_IOS = "test_notification";
    private int identifier_IOS;
    private void Start()
    {
        UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
        UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();
        StartCoroutine(RequestAuthorization());
    }
    public void CreateIOSNotificationChannel()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, 3),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "test_notification",
            Title = "Hey! Come Back!",
            Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
            Subtitle = "Play a Cannabisfarm game",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);

        iOSNotificationCenter.RemoveScheduledNotification(notification.Identifier);
        iOSNotificationCenter.RemoveDeliveredNotification(notification.Identifier);
    }
    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        bool permissionGranted = false;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            permissionGranted = req.IsFinished;
            Debug.Log(res);
        }
        if (permissionGranted)
        {
            CreateIOSNotificationChannel();
            isNotification = true;
            _numCheckCannabisCoinMax = 0;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
        }
        else
        {
            isNotification = false;
            _numCheckCannabisCoinMax = 1;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
            yield break;
        }
    }
    #endregion
#endif
}
