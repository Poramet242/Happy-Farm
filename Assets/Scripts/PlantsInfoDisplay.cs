using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class PlantsInfoDisplay : MonoBehaviour
{
    public static PlantsInfoDisplay Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public List<CharacterData> _characterDataList;
    [SerializeField] public bool isplant;
    [Header("Info Detail")]
    [SerializeField] private Image _planeIcon_img;
    [SerializeField] private Text _planeName_text;
    [SerializeField] private Text _planeInfo_text;
    [SerializeField] private Text _planeCount_text;
    [SerializeField] private Text _planePrice_text;
    [SerializeField] private Text _plantTime_text;
    [SerializeField] private Text _plantTopPrice_text;
    [SerializeField] public ScrollRect _plant_Scr;

    public void setUpPlaneInfoDisplay(List<CharacterData> data)
    {
        _characterDataList = data;
        setInfoDetail(data[0]);
        getTimeToshowInfo(data[0]);
        _plant_Scr.gameObject.SetActive(true);
    }
    public void setInfoDetail(CharacterData data)
    {
        _planeIcon_img.sprite = data.detail._unitLocalImage;
        _planeName_text.text = data.detail._unitName;
        _planeInfo_text.text = data.unitData._unitInfo;
        //int num = _planeShowData._unitCountPlane * _planeShowData._pricePlane;
        _planePrice_text.text = data.unitData._priceSellPlane.ToString("#,##0");
        _plantTopPrice_text.text = data.unitData._unitCoineMax.ToString("#,##0");
    }
    public void getTimeToshowInfo(CharacterData data)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(data.unitData.GrowTime);
        int totalDays = (int)timeSpan.TotalDays;
        int remainingHours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        _plantTime_text.text = setDisplayTimeShowInfo(totalDays, remainingHours, minutes, seconds);
    }

    public string setDisplayTimeShowInfo(int totalDays, int remainingHours, int minutes, int seconds)
    {
        string time = "";

        if (totalDays > 0) time += (totalDays * 24) + "h "; // Convert total days to hours
        if (remainingHours > 0) time += remainingHours + "h ";
        if (minutes > 0) time += minutes + "m ";
        if (seconds > 0) time += seconds + "s";
        return time;
    }
    public void setInfoNoneDetail()
    {
        _planeIcon_img.sprite = null;
        _planeName_text.text = null;
        //_planeInfo_text.text = null;
        //int num = _planeShowData._unitCountPlane * _planeShowData._pricePlane;
        _planePrice_text.text = "0";
        _planeCount_text.text = "x" + "0";
    }
    private void Update()
    {
        _planeCount_text.text = "x" + _characterDataList.Count.ToString();
        if (_characterDataList.Count > 0 && InventoryLayerController.instance._unitinventoryDisplayList.Count > 0)
        {
            InventoryLayerController.instance._sell_btn.GetComponent<Button>().interactable = true;
        }
    }
    public void onClickPlant()
    {
        isplant = true;
        SoundListObject.instance.OnclickSFX(3);
        StakeLayerController.instance.CloseUiLayerGameplay();
        CharacterData data = new CharacterData();
        for (int i = 0; i < 1; i++)
        {
            data.detail = _characterDataList[i].detail;
            data.unitData = _characterDataList[i].unitData;
        }
        data.detail._growthPlante = GrowthPlante.Seed;
        data.detail._zonePos = StakeLayerController.instance.zone;
        data.detail._unitStaking = true; 

        StakeLayerController.instance.tsc.OnSelectCharacterToPlant(data);
        StakeLayerController.instance.setUpPlantPotPrefads(data);
        setDataPlaneZoneDisplay(data);
        StartCoroutine(plantingSeed(data));
        //remove list data
        for (int i = 0; i < _characterDataList.Count; i++)
        {
            if (_characterDataList[i].unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
            {
                _characterDataList.Remove(_characterDataList[i]);
                InventoryLayerController.instance.removePlantZeroInList(data);
            }
        }
        StakeLayerController.instance.plantEff(data,true);
        //close
    }
    IEnumerator plantingSeed(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.Planting(XCoreManager.instance.mXCoreInstance, data.detail._unitTokenID, ZoneTypeEnumToString(data.detail._zonePos), "block" + (data.detail._unitPos + 1), (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error get data to planting seed");
            yield break;
        }
        else
        {
            Debug.Log("get data to planting seed");
        }
    }
    public string ZoneTypeEnumToString(ZoneType @enum)
    {
        return @enum switch
        {
            ZoneType.Garage => "zone1",
            ZoneType.BasketBall => "zone2",
            ZoneType.BoxingStadium => "zone3",
        };
    }
    public void setDataPlaneZoneDisplay(CharacterData characterData)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (characterData.detail._zonePos == ZoneUnitObject.instance.unitDataZones[i].ZoneType) 
            {
                ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Add(characterData);
            }
        }
    }
    public void onClickSellUnit()
    {
        SoundListObject.instance.OnclickSFX(1);
        CharacterData data = new CharacterData();
        for (int i = 0; i < 1; i++)
        {
            data.detail = _characterDataList[i].detail;
            data.unitData = _characterDataList[i].unitData;
        }
        //Sell Plane NFT
        //---------------------------------------------------------------------------
        if (data.detail._planeNFT)
        {
            PlayerObject.instance._tokenNFTReward += data.unitData._priceSellPlane / 2;
        }
        //----------------------------------------------------------------------------
        else 
        {
            StartCoroutine(sellSeed(data));
        }
        for (int i = 0; i < _characterDataList.Count; i++)
        {
            if (_characterDataList[i].unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
            {
                _characterDataList.Remove(_characterDataList[i]);
                InventoryLayerController.instance.removePlantZeroInList(data);
            }
        }
        if (!PlayerObject.instance._checkplayTutorial)
        {
            InventoryLayerController.instance._totur_Info.SetActive(false);
            InventoryLayerController.instance._totur_Close.SetActive(true);

        }
    }
    IEnumerator sellSeed(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.SellSeed(XCoreManager.instance.mXCoreInstance, data.detail._unitTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error get data to selling seed");
            yield break;
        }
        yield return PlayerObject.instance.GetWalletPlayer();
    }

}
