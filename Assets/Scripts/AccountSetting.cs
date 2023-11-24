using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class AccountSetting : MonoBehaviour
{
    [Header("Profile Player")]
    [SerializeField] private Image _profile_img;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _accType;
    [SerializeField] private TMP_Text _accNO;

    private void Update()
    {
        if (PlayerObject.instance._imagesProfile_spr != null)
        {
            _profile_img.sprite = PlayerObject.instance._imagesProfile_spr;
        }
        _accNO.text = PlayerObject.instance._accNo;
        _accType.text = PlayerObject.instance._accType.ToString();
        _name.text = PlayerObject.instance._playerName;
    }

    public void onclickUnbindAccount_btn()
    {
        throw new NotImplementedException();
    }
    public void onclickDeleteAccount_btn()
    {
        StartCoroutine(setDeleteAccount());
        SettingController.instance.ClearSettingObject();
        Destroy(SoundManager.instance.gameObject);
        PlayerObject.instance.ClearPlayerObject();
        ProfileLayerController.instance.ClearPlayerProfileObject();
        PlayerPrefs.DeleteKey("sessionToken");
        SceneManager.LoadScene("LoginScene");
    }
    public void onclickHelp_btn()
    {
        throw new NotImplementedException();
    }
    public void onclickCustomer_btn()
    {
        throw new NotImplementedException();
    }
    public void onclickLogout_btn()
    {
        StartCoroutine(setLogout());
        SettingController.instance.ClearSettingObject();
        Destroy(SoundManager.instance.gameObject);
        PlayerObject.instance.ClearPlayerObject();
        ProfileLayerController.instance.ClearPlayerProfileObject();
        PlayerPrefs.DeleteKey("sessionToken");
        SceneManager.LoadScene("LoginScene");
    }

    IEnumerator setLogout()
    {
        IWSResponse response = null;
        yield return XUser.Logout(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
    }
    IEnumerator setDeleteAccount()
    {
        IWSResponse response = null;
        yield return Account.DeleteAccount(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
    }
}
