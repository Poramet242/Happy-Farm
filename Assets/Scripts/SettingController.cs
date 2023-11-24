using System;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public static SettingController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Data Setting")]
    [SerializeField] public bool _checkNotification;
    [SerializeField] public bool _checkLanguageTH;
    [SerializeField] public bool _checkLanguageEN;
    [SerializeField] public bool _checkAnimation;
    [Header("This Object")]
    [SerializeField] public GameObject thisObj;
    [SerializeField] public GameObject face_bg;
    [SerializeField] public GameObject Warning_bg;
    [Header("Bar button")]
    [SerializeField] private Sprite[] _bar_Btn;
    [Header("GameSetting Bar")]
    [SerializeField] private GameObject gamesetting_obj;
    [SerializeField] private Image _gamesetting_bar;
    [Header("AccountSetting Bar")]
    [SerializeField] private GameObject accountsetting_Obj;
    [SerializeField] private Image _accountsetting_bar;

    public void onclickGameSetting_bar()
    {
        SoundListObject.instance.OnclickSFX(0);
        gamesetting_obj.SetActive(true);
        accountsetting_Obj.SetActive(false);
        _gamesetting_bar.sprite = _bar_Btn[1];
        _accountsetting_bar.sprite = _bar_Btn[0];
    }
    public void onclickAccountSetting_Bar()
    {
        SoundListObject.instance.OnclickSFX(0);
        gamesetting_obj.SetActive(false);
        accountsetting_Obj.SetActive(true);
        _gamesetting_bar.sprite = _bar_Btn[0];
        _accountsetting_bar.sprite = _bar_Btn[1];
    }

    public void facetoPlayGame(bool check,bool checkmainSc)
    {
        if (!checkmainSc)
        {
            thisObj.SetActive(check);
            face_bg.SetActive(check);
        }
        else
        {
            thisObj.SetActive(check);
            face_bg.SetActive(!check);
        }

    }
    public void ClearSettingObject()
    {
        Destroy(this.gameObject);
    }

    public void CloseThisLayer()
    {
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        this.gameObject.SetActive(false);
    }
}
