using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [Header("Game Setting")]
    #region sound
    [SerializeField] private Sprite[] _frameVol;
    [SerializeField] private Sprite[] _iconVol;
    [Header("SFXSlider")]
    [SerializeField] private Slider _SFXslider;
    [SerializeField] private float _valueSFX_ft;
    [SerializeField] private bool _closeSFX_vol;
    [SerializeField] private Image _group_SFX;
    [SerializeField] private GameObject[] _frameSFX_obj;
    //[SerializeField] private Text _SFXtext;
    [Header("BGMSlider")]
    [SerializeField] private Slider _BGMslider;
    [SerializeField] private float _valueBGM_ft;
    [SerializeField] private bool _closeBGM_vol;
    [SerializeField] private Image _group_BGM;
    [SerializeField] private GameObject[] _frameBGM_obj;
    //[SerializeField] private Text _BGMtext;
    #endregion

    #region Notification
    [Header("Notification")]
    [SerializeField] private NotificationsController _notificationsController;
    [SerializeField] private bool _checkCannabisCoinMaxCapacity;
    [SerializeField] private int _numCheckCannabisCoinMax;
    [SerializeField] private Toggle Toggle_checkCannabisCoinMaxCapacity;
    #endregion

    #region Language
    [Header("Lamguage")]
    [SerializeField] public int numLanguage;
    [SerializeField] private Toggle Toggle_LanguageTH;
    [SerializeField] private Toggle Toggle_LanguageEN;
    #endregion

    #region Animation
    [Header("Animation")]
    [SerializeField] public int numAnimation;
    [SerializeField] private Toggle Toggle_Default;
    [SerializeField] private Toggle Toggle_Skip;
    #endregion
    private void Start()
    {
        //GameObject notify_obj = GameObject.Find("NotificationController");
        _notificationsController = FindObjectOfType<NotificationsController>();
        //_notificationsController = notify_obj.GetComponent<NotificationsController>();
        setupStartSetting();
    }

    private void Update()
    {
        setCloseSoundSFX_volme(_SFXslider.value);
        setCloseSoundBGM_volme(_BGMslider.value);
        _notificationsController.isNotification = _checkCannabisCoinMaxCapacity;
    }
    public void setupStartSetting()
    {
        _valueSFX_ft = DataSoundHolder.SFX_Volume;
        _SFXslider.value = _valueSFX_ft;
        _valueBGM_ft = DataSoundHolder.BGM_Volume;
        _BGMslider.value = _valueBGM_ft;
        //Notification setting
        _numCheckCannabisCoinMax = DataSoundHolder.cannabisCoinMaxCapacity;
        if (_numCheckCannabisCoinMax == 0)
        { Toggle_checkCannabisCoinMaxCapacity.isOn = true; }
        else
        { Toggle_checkCannabisCoinMaxCapacity.isOn = false; }
        //Language setting
        numLanguage = DataSoundHolder.LanguageSetting;
        if (numLanguage == 0)
        { Toggle_LanguageEN.isOn = true; }
        if (numLanguage == 1) 
        { Toggle_LanguageTH.isOn = true; }
        //Aniamtion Gacha setting 
        numAnimation = DataSoundHolder.animationSetting;
        if (numAnimation == 0)
        { 
            Toggle_Default.isOn = true; 
            SettingController.instance._checkAnimation = false; 
        }
        if (numAnimation == 1)
        { 
            Toggle_Skip.isOn = true; 
            SettingController.instance._checkAnimation = true; 
        }
    }
    public void SaveAllSetting()
    {
        PlayerPrefs.SetFloat("SFX_Volume", _valueSFX_ft);
        PlayerPrefs.SetFloat("BGM_Volume", _valueBGM_ft);
        PlayerPrefs.SetInt("CannabisCoinMaxCapacity", _numCheckCannabisCoinMax);
        PlayerPrefs.SetInt("LanguageSetting",numLanguage);
        PlayerPrefs.SetInt("AnimationSetting", numAnimation);
    }
    #region sound setting
    public void setCloseSoundSFX_volme(float num)
    {
        if (num == 0)
        {
            _closeSFX_vol = true;
            _group_SFX.sprite = _iconVol[1];
            _frameSFX_obj[1].SetActive(false);
            _frameSFX_obj[0].SetActive(true);
        }
        else
        {
            _closeSFX_vol = false;
            _group_SFX.sprite = _iconVol[0];
            _frameSFX_obj[1].SetActive(true);
            _frameSFX_obj[0].SetActive(false);
        }
    }
    public void onclickCloseSFX_btn(bool check)
    {
        if (check)
        {
            _group_SFX.sprite = _iconVol[1];
            _SFXslider.value = 0f;
            DataSoundHolder.SFX_Volume = _SFXslider.value;
        }
        else
        {
            _group_SFX.sprite = _iconVol[0];
            _SFXslider.value = 0.5f;
            DataSoundHolder.SFX_Volume = _SFXslider.value;
        }
    }
    public void setCloseSoundBGM_volme(float num)
    {
        if (num == 0)
        {
            _closeBGM_vol = true;
            _group_BGM.sprite = _iconVol[1];
            _frameBGM_obj[1].SetActive(false);
            _frameBGM_obj[0].SetActive(true);
        }
        else
        {
            _closeBGM_vol = false;
            _group_BGM.sprite = _iconVol[0];
            _frameBGM_obj[1].SetActive(true);
            _frameBGM_obj[0].SetActive(false);
        }
    }
    public void onclickCloseBGM_btn(bool check)
    {
        if (check)
        {
            _group_BGM.sprite = _iconVol[1];
            _BGMslider.value = 0f;
            DataSoundHolder.BGM_Volume = _BGMslider.value;
        }
        else
        {
            _group_BGM.sprite = _iconVol[0];
            _BGMslider.value = 0.5f;
            DataSoundHolder.BGM_Volume = _BGMslider.value;
        }
    }
    public void setSoundSFX_Volume()
    {
        _valueSFX_ft = _SFXslider.value;
        DataSoundHolder.SFX_Volume = _valueSFX_ft;
        float numVolSFX = _valueSFX_ft * 100;
        //_SFXtext.text = Mathf.FloorToInt(numVolSFX).ToString();
    }
    public void setSoundBGM_volume()
    {
        _valueBGM_ft = _BGMslider.value;
        DataSoundHolder.BGM_Volume = _valueBGM_ft;
        float numVolBGM = _valueBGM_ft * 100;
        //_BGMtext.text = Mathf.FloorToInt(numVolBGM).ToString();
        if (SoundManager.instance != null)
        {
            if (SoundManager.instance.isPlayingBgm())
            {
                SoundManager.instance.GetBGMInstance().volume = _valueBGM_ft;
            }
        }
    }
    #endregion

    #region Notification setting
    public void setAllNotificaiton()
    {
        _checkCannabisCoinMaxCapacity = Toggle_checkCannabisCoinMaxCapacity.isOn;
#if UNITY_ANDROID
        StartCoroutine(RequestNotificationPermission());
#endif
#if UNITY_IOS
        StartCoroutine(RequestAuthorization());
#endif
    }
#if UNITY_ANDROID
    #region Android Notifications
    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        // here use request.Status to determine users response
        if (request.Status == PermissionStatus.Allowed)
        {
            NotificationsController.instance.CreateAndroidNotificationChannel();
            _checkCannabisCoinMaxCapacity = true;
            _numCheckCannabisCoinMax = 0;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
        }
        else
        {
            _checkCannabisCoinMaxCapacity = false;
            _numCheckCannabisCoinMax = 1;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
            yield break;
        }
    }
    #endregion
#endif
#if UNITY_IOS
    #region IOS Notifications
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
            NotificationsController.instance.CreateIOSNotificationChannel();
            _checkCannabisCoinMaxCapacity = true;
            _numCheckCannabisCoinMax = 0;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
        }
        else
        {
            _checkCannabisCoinMaxCapacity = false;
            _numCheckCannabisCoinMax = 1;
            DataSoundHolder.cannabisCoinMaxCapacity = _numCheckCannabisCoinMax;
            yield break;
        }
    }
    #endregion
#endif
    #endregion

    #region Language setting
    public void setLanguage()
    {
        int numcheckLanguage;
        if (Toggle_LanguageEN.isOn)
        {
            numcheckLanguage = 0;
            numLanguage = 0;
            DataSoundHolder.LanguageSetting = numcheckLanguage;
            Debug.Log(numcheckLanguage);
        }
        if (Toggle_LanguageTH.isOn)
        {
            numcheckLanguage = 1;
            numLanguage = 1;
            DataSoundHolder.LanguageSetting = numcheckLanguage;
            Debug.Log(numcheckLanguage);
        }
    }
#endregion

#region Animation setting
    public void setAnimation()
    {
        int numcheckAnimation;
        if (Toggle_Default.isOn)
        {
            numcheckAnimation = 0;
            numAnimation = 0;
            DataSoundHolder.animationSetting = numcheckAnimation;
            SettingController.instance._checkAnimation = false;
        }
        if (Toggle_Skip.isOn)
        {
            numcheckAnimation = 1;
            numAnimation = 1;
            DataSoundHolder.animationSetting = numcheckAnimation;
            SettingController.instance._checkAnimation = true;
        }
    }
#endregion
}
