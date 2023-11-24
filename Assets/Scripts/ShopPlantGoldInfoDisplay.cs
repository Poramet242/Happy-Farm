using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ShopPlantGoldInfoDisplay : MonoBehaviour
{
    public static ShopPlantGoldInfoDisplay Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public CharacterData characterData;
    [Header("Info Detail")]
    [SerializeField] private Image _planeIcon_img;
    [SerializeField] private Text _planeName_text;
    [SerializeField] private Text _planeInfo_text;
    [SerializeField] private Text _planeCount_text;
    [SerializeField] private Text _planePrice_text;
    [SerializeField] private Text _plantTime_text;
    [SerializeField] private Text _planePriceNFT_text;
    [SerializeField] public ScrollRect _plant_Scr;
    [SerializeField] public Button Buy_btn;
    public void setupDataPlantShop(CharacterData data)
    {
        characterData = data;
        setInfoDetail(data);
        getTimeToshowInfo(data);
        if (data.unitData._currentCountPlantGold == 0)
        {
            Buy_btn.interactable = false;
        }
        else
        {
            Buy_btn.interactable = true;
        }
    }
    public void setInfoDetail(CharacterData data)
    {
        _planeIcon_img.sprite = data.detail._unitLocalImage;
        _planeName_text.text = data.detail._unitName;
        if (data.unitData._currentCountPlantGold > 20)
        {
            _planeCount_text.text = data.unitData._currentCountPlantGold.ToString();
        }
        else
        {
            _planeCount_text.text = data.unitData._currentCountPlantGold + "/" + data.unitData._maxCountPlantGold;
        }
        _planeInfo_text.text = ThaiFontAdjuster.Adjust(data.unitData._unitInfo);
        _planePrice_text.text = data.unitData._unitCoineMax.ToString("#,##0");
        _planePriceNFT_text.text = data.unitData._priceBuyPlane.ToString("#,##0");
    }
    /*public void getTimeToshowInfo(CharacterData data)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(data.unitData.GrowTime);
        string hoursPlant = timeSpan.Hours.ToString();
        string minutesPlant = timeSpan.Minutes.ToString();
        string secondsPlant = timeSpan.Seconds.ToString();
        _plantTime_text.text = setDisplayTimeShowInfo(hoursPlant, minutesPlant, secondsPlant);
    }
    public string setDisplayTimeShowInfo(string hoursPlant, string minutesPlant, string secondsPlant)
    {
        string time = "";
        if (hoursPlant == "0" && minutesPlant == "0") time = secondsPlant + "s";
        else if (hoursPlant == "0" && secondsPlant == "0") time = minutesPlant + "m";
        else if (minutesPlant == "0" && secondsPlant == "0") time = hoursPlant + "h";
        else if (hoursPlant == "0") time = minutesPlant + "m" + secondsPlant + "s";
        else if (secondsPlant == "0") time = hoursPlant + "h" + minutesPlant + "m";
        else if (minutesPlant == "0") time = hoursPlant + "h" + secondsPlant + "s";
        else time = hoursPlant + "h" + minutesPlant + "m" + secondsPlant + "s";
        return time;
    }*/
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
    public void onClickBuyLeaf()
    {
        if (PlayerObject.instance._tokenNFTReward <= 0 || characterData.unitData._currentCountPlantGold <= 0)
        {
            if (characterData.unitData._currentCountPlantGold <= 0)
            {
                ShopLayerController.instance._WarningUi.SetActive(true);
                ShopLayerController.instance._WarningUi.GetComponent<WarningUi>()._innfo_txt.text = "This plant is not count";
                return;
            }
            else
            {
                ShopLayerController.instance._WarningUi.SetActive(true);
                ShopLayerController.instance._WarningUi.GetComponent<WarningUi>()._innfo_txt.text = "Not enough money";
                return;
            }
        }
        else
        {
            StartCoroutine(BuySeedNFT(characterData));
        }
    }
    IEnumerator BuySeedNFT(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.BuySeed(XCoreManager.instance.mXCoreInstance, data.detail._unitTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error get data to planting seed");
            yield break;
        }
        yield return PlayerObject.instance.GetWalletPlayer();
        yield return UserSeed.GetUserSeed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<UserSeed> all_seeds = UserSeed.ParseToList(response.RawResult().ToString());
        StakeUnitObject.instance._allUnitDetailList.Clear();
        StakeUnitObject.instance._allUnitDataList.Clear();
        for (int i = 0; i < all_seeds.Count; i++)
        {
            UnitDetail unitDetail = ScriptableObject.CreateInstance<UnitDetail>();
            UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
            //set plant ID
            unitDetail._unitTokenID = all_seeds[i].plantID;
            unitData._unitTokenID = all_seeds[i].plantID;
            unitData._unitCurrentPlant = all_seeds[i].id;
            unitDetail._unitCurrentPlant = all_seeds[i].id;
            var plant = GameData.instance.GetPlantInfoByPlantID(unitDetail._unitTokenID);
            StakeUnitObject.instance.SetUpDataPlantinfo(plant, unitDetail, unitData);

            StakeUnitObject.instance._allUnitDetailList.Add(unitDetail);
            StakeUnitObject.instance._allUnitDataList.Add(unitData);
        }

        characterData.unitData._currentCountPlantGold -= 1;
        _planeCount_text.text = characterData.unitData._currentCountPlantGold.ToString();
        ShopLayerController.instance.infoGoldPlant_obj.SetActive(false);
        SoundListObject.instance.OnclickSFX(1);
    }
    public void playSoundVFX()
    {
        SoundListObject.instance.OnclickSFX(0);
    }
}
