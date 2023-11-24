using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class Zone : MonoBehaviour
{
    public static Zone instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public bool checkunLock;
    [SerializeField] public bool checkIsBuy;
    [SerializeField] public ZoneType zones;
    [SerializeField] private string NameScenes;
    [SerializeField] private string isIdZone;
    [Header("Lock")]
    [SerializeField] public GameObject _lockZonePanel;
    [Header("UnLock")]
    [SerializeField] public GameObject _UnlockZonePanel;
    [Header("FaceDown")]
    [SerializeField] public GameObject _faceDown;
    [SerializeField] public Text _countTeam_text;
    [Header("Button endter")]
    [SerializeField] public Button _enterToZone;
    [Header("Text price")]
    [SerializeField] public GameObject _buyZone_Coine;
    [SerializeField] public GameObject _buyZone_NFT;
    [SerializeField] public Text _CoinePriceZone_text;
    [SerializeField] public Text _TokenPriceZone_text;
    [Header("locationMap")]
    [SerializeField] public GameObject _miniLocation;
    private void OnEnable()
    {
        StartCoroutine(setunLockArea());   
    }
    private void Update()
    {
        if (checkunLock && checkIsBuy)
        {
            OpenZone();
        }
        if (PlayerObject.instance._zone == zones)
        {
            _miniLocation.SetActive(true);
        }
        else
        {
            _miniLocation.SetActive(false);
        }
    }
    public void OpenBuyZone()
    {
        _lockZonePanel.SetActive(false);
        _faceDown.SetActive(false);
        _UnlockZonePanel.SetActive(true);
    }
    public void OpenZone()
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == zones)
            {
                ZoneUnitObject.instance.unitDataZones[i]._checkUnlock = true;
                _countTeam_text.text = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count.ToString();
            }
        }
        _lockZonePanel.SetActive(false);
        _UnlockZonePanel.SetActive(false);
        _faceDown.SetActive(true);
    }
    public void onClickNextZone()
    {
        if (checkunLock && checkIsBuy)
        {
            if (zones == PlayerObject.instance._zone)
            {
                return;
            }
            ZoneUnitObject.instance.resetDatathisZone(true);
            PlayerObject.instance._zone = zones;
            StakeUnitObject.instance.zone = zones;
            SettingController.instance.facetoPlayGame(true, false);
            SceneManager.LoadScene(NameScenes);
        }
        else
        {
            return;
        }
    }
    public void onClickBuyZone(Spin_btn detail)
    {
        SoundListObject.instance.OnclickSFX(1);
        Debug.Log("Area ID: " + isIdZone);
        if (detail._checkNFT)
        {
            StartCoroutine(setBuyZone(detail._checkNFT));
        }
        else
        {
            StartCoroutine(setBuyZone(detail._checkNFT));
        }
        #region checkdetailData
        /*if (detail._checkNFT)
         {
             PlayerObject.instance._tokenNFTReward -= detail._countSpin;
             checkIsBuy = true;
             this.enabled = true;
             OpenZone();
             return;
         }
         PlayerObject.instance._coineReward -= detail._countSpin;
         checkIsBuy = true;
         this.enabled = true;
         OpenZone();*/
        #endregion
    }
    IEnumerator setunLockArea()
    {
        IWSResponse response = null;
        yield return AreaInfo.GetAreaInfo(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        List<AreaInfo> areaInfos = AreaInfo.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < areaInfos.Count; i++)
        {
            if (areaInfos[i].areaID == "zone" + (int)zones)
            {
                isIdZone = areaInfos[i].areaID;
            }
        }
    }
   IEnumerator setBuyZone(bool check)
    {
        IWSResponse response = null;
        yield return GameAPI.UnlockArea(XCoreManager.instance.mXCoreInstance, isIdZone, check, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        checkIsBuy = true;
        this.enabled = true;
        OpenZone();
        yield return PlayerObject.instance.GetWalletPlayer();

    }
}
