using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XSystem;

public class AssistantsSkillActived : MonoBehaviour
{
    public static AssistantsSkillActived instance;

    public Action AssistantChoseWorkThisZone_ac;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        AssistantChoseWorkThisZone_ac += CheckAssistantChoseWorkThisZone;
    }
    [Header("This zone")]
    [SerializeField] private ZoneType _thisZone;
    [Header("Color Actived")]
    [SerializeField] private Color actived_color;
    [Header("Skill")]
    [SerializeField] public bool check_Skill_increase_Coin;
    [SerializeField] public bool check_Skill_increase_EXP;
    [SerializeField] public bool check_Skill_extendperiod_PlantGold_GrowTime;
    [SerializeField] public bool check_Skill_increase_PlantStack;
    [SerializeField] public bool check_Skill_sale_PriceGachaPlant;
    [SerializeField] public bool check_Skill_sale_Sale_PriceFunitrue;
    [SerializeField] public bool check_Skill_sale_Sale_PriceBlock;
    [SerializeField] public bool check_Skill_speed_GrowTime;
    //[Header("Assistants")]
    //[SerializeField] public List<AssisstantDetail> assisstantDetailsList = new List<AssisstantDetail>();
    [SerializeField] public Dictionary<UnitSkill, RarityType> Active_unitSkills = new Dictionary<UnitSkill, RarityType>();
    [SerializeField] public Dictionary<UnitSkill, RarityType> inventory_unitSkills_dic = new Dictionary<UnitSkill, RarityType>();

    [SerializeField] public Dictionary<UnitSkill, RarityType> old_unitSkills_dic = new Dictionary<UnitSkill, RarityType>();

    [SerializeField] public Dictionary<UnitSkill, bool> keyValuePairs = new Dictionary<UnitSkill, bool>();
    public bool checkonce;
    public void Initialize()
    {
        checkonce = true;
        AssistantChoseWorkThisZone_ac?.Invoke();
    }
    public void CheckAssistantChoseWorkThisZone()
    {
        if (checkonce)
        {
            old_unitSkills_dic.Add(UnitSkill.Increase_Coin, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Increase_EXP, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Extendperiod_PlantGold_GrowTime, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Increase_PlantStack, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Sale_PriceGachaPlant, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Sale_PriceFunitrue, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Sale_PriceBlock, RarityType.None);
            old_unitSkills_dic.Add(UnitSkill.Speed_GrowTime, RarityType.None);
            checkonce = false;
        }
        else
        {
            foreach (var item in Active_unitSkills)
            {
                old_unitSkills_dic[item.Key] = Active_unitSkills[item.Key];
            }
        }
        Active_unitSkills.Clear();
        Active_unitSkills.Add(UnitSkill.Increase_Coin, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Increase_EXP, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Extendperiod_PlantGold_GrowTime, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Increase_PlantStack, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Sale_PriceGachaPlant, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Sale_PriceFunitrue, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Sale_PriceBlock, RarityType.None);
        Active_unitSkills.Add(UnitSkill.Speed_GrowTime, RarityType.None);

        inventory_unitSkills_dic.Clear();
        #region old skill actived
        /*for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == _thisZone)
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    if (Active_unitSkills.ContainsKey(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill))
                    {
                        Debug.Log("ContainsKey");
                        if (Active_unitSkills[ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill] <
                            ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType)
                        {

                            Active_unitSkills[ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill] = ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType;
                        }
                    }
                    else
                    {
                        Debug.Log("add key");
                        Active_unitSkills.Add(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill, ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType);
                    }
                }
            }
            else
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill == UnitSkill.Sale_PriceGachaPlant)
                    {
                        if (Active_unitSkills.ContainsKey(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill))
                        {
                            Debug.Log("ContainsKey");
                            if (Active_unitSkills[ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill] <
                                ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType)
                            {

                                Active_unitSkills[ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill] = ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType;
                            }
                        }
                        else
                        {
                            Debug.Log("add key");
                            Active_unitSkills.Add(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill, ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType);
                        }
                    }
                }

            }
        }*/
        #endregion
        for (int i = 0; i < AssistantsObject.instance._assistantsAllList.Count; i++)
        {
            if (AssistantsObject.instance._assistantsAllList[i]._zonePos == PlayerObject.instance._zone && AssistantsObject.instance._assistantsAllList[i]._unitWork)
            {
                if (Active_unitSkills.ContainsKey(AssistantsObject.instance._assistantsAllList[i]._skill))
                {
                    Debug.Log("ContainsKey");
                    if (Active_unitSkills[AssistantsObject.instance._assistantsAllList[i]._skill] < AssistantsObject.instance._assistantsAllList[i]._rarityType)
                    {

                        Active_unitSkills[AssistantsObject.instance._assistantsAllList[i]._skill] = AssistantsObject.instance._assistantsAllList[i]._rarityType;
                        keyValuePairs[AssistantsObject.instance._assistantsAllList[i]._skill] = true;
                    }
                }
                else
                {
                    Debug.Log("add key");
                    Active_unitSkills.Add(AssistantsObject.instance._assistantsAllList[i]._skill, AssistantsObject.instance._assistantsAllList[i]._rarityType);
                    keyValuePairs[AssistantsObject.instance._assistantsAllList[i]._skill] = true;
                }
            }
            else if (AssistantsObject.instance._assistantsAllList[i]._unitWork && AssistantsObject.instance._assistantsAllList[i]._skill == UnitSkill.Sale_PriceGachaPlant)
            {
                if (Active_unitSkills.ContainsKey(AssistantsObject.instance._assistantsAllList[i]._skill))
                {
                    Debug.Log("ContainsKey");
                    if (Active_unitSkills[AssistantsObject.instance._assistantsAllList[i]._skill] < AssistantsObject.instance._assistantsAllList[i]._rarityType)
                    {
                
                        Active_unitSkills[AssistantsObject.instance._assistantsAllList[i]._skill] = AssistantsObject.instance._assistantsAllList[i]._rarityType;
                        keyValuePairs[AssistantsObject.instance._assistantsAllList[i]._skill] = true;
                    }
                }
                else
                {
                    Debug.Log("add key");
                    Active_unitSkills.Add(AssistantsObject.instance._assistantsAllList[i]._skill, AssistantsObject.instance._assistantsAllList[i]._rarityType);
                    keyValuePairs[AssistantsObject.instance._assistantsAllList[i]._skill] = true;
                }
            }
            else
            {
                if (keyValuePairs.ContainsKey(AssistantsObject.instance._assistantsAllList[i]._skill))
                {
                    continue;
                }
                inventory_unitSkills_dic[AssistantsObject.instance._assistantsAllList[i]._skill] = AssistantsObject.instance._assistantsAllList[i]._rarityType;
                keyValuePairs[AssistantsObject.instance._assistantsAllList[i]._skill] = false;
            }
        }

        foreach (var item in Active_unitSkills)
        {
            Debug.Log("this Skill: " + item.Key + " : " + item.Value);
            if (old_unitSkills_dic[item.Key] == Active_unitSkills[item.Key])
            {
                Debug.Log("Continue Skill: " + item.Key + " : " + item.Value);
                continue;
            }
            switch (item.Key)
            {
                case UnitSkill.none:
                    break;
                case UnitSkill.Increase_Coin:
                    check_Skill_increase_Coin = keyValuePairs[UnitSkill.Increase_Coin];
                    Skill_Increase_Coin(check_Skill_increase_Coin, item.Key, item.Value);
                    break;
                case UnitSkill.Increase_EXP:
                    check_Skill_increase_EXP = keyValuePairs[UnitSkill.Increase_EXP];
                    Skill_Increase_EXP(check_Skill_increase_EXP, item.Key, item.Value);
                    break;
                case UnitSkill.Extendperiod_PlantGold_GrowTime:
                    check_Skill_extendperiod_PlantGold_GrowTime = keyValuePairs[UnitSkill.Extendperiod_PlantGold_GrowTime];
                    Skill_Extendperiod_PlantGold_GrowTime(check_Skill_extendperiod_PlantGold_GrowTime, item.Key, item.Value);
                    break;
                case UnitSkill.Increase_PlantStack:
                    check_Skill_increase_PlantStack = keyValuePairs[UnitSkill.Increase_PlantStack];
                    Skill_Increase_PlantStack(check_Skill_increase_PlantStack, item.Key, item.Value);
                    break;
                case UnitSkill.Sale_PriceGachaPlant:
                    check_Skill_sale_PriceGachaPlant = keyValuePairs[UnitSkill.Sale_PriceGachaPlant];
                    Skill_Sale_PriceGachaPlant(check_Skill_sale_PriceGachaPlant, item.Key, item.Value);
                    break;
                case UnitSkill.Sale_PriceFunitrue:
                    check_Skill_sale_Sale_PriceFunitrue = keyValuePairs[UnitSkill.Sale_PriceFunitrue];
                    Skill_Sale_PriceFunitrue(check_Skill_sale_Sale_PriceFunitrue, item.Key, item.Value);
                    break;
                case UnitSkill.Sale_PriceBlock:
                    check_Skill_sale_Sale_PriceBlock = keyValuePairs[UnitSkill.Sale_PriceBlock];
                    Skill_Sale_PriceBlock(check_Skill_sale_Sale_PriceBlock, item.Key, item.Value);
                    break;
                case UnitSkill.Speed_GrowTime:
                    check_Skill_speed_GrowTime = keyValuePairs[UnitSkill.Speed_GrowTime];
                    Skill_Speed_GrowTime(check_Skill_speed_GrowTime, item.Key, item.Value);
                    break;
            }
        }

        foreach (var item in inventory_unitSkills_dic)
        {
            if (!Active_unitSkills.ContainsKey(item.Key))
            {
                switch (item.Key)
                {
                    case UnitSkill.none:
                        break;
                    case UnitSkill.Increase_Coin:
                        check_Skill_increase_Coin = false;
                        Skill_Increase_Coin(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Increase_EXP:
                        check_Skill_increase_EXP = false;
                        Skill_Increase_EXP(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Extendperiod_PlantGold_GrowTime:
                        check_Skill_extendperiod_PlantGold_GrowTime = false;
                        Skill_Extendperiod_PlantGold_GrowTime(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Increase_PlantStack:
                        check_Skill_increase_PlantStack = false;
                        Skill_Increase_PlantStack(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Sale_PriceGachaPlant:
                        check_Skill_sale_PriceGachaPlant = false;
                        Skill_Sale_PriceGachaPlant(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Sale_PriceFunitrue:
                        check_Skill_sale_Sale_PriceFunitrue = false;
                        Skill_Sale_PriceFunitrue(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Sale_PriceBlock:
                        check_Skill_sale_Sale_PriceBlock = false;
                        Skill_Sale_PriceFunitrue(false, item.Key, item.Value);
                        break;
                    case UnitSkill.Speed_GrowTime:
                        check_Skill_speed_GrowTime = false;
                        Skill_Speed_GrowTime(false, item.Key, item.Value);
                        break;
                }
            }
        }
    }

    #region AssistantDetail Skill
    public RarityType CheckRarityType(UnitSkill unitSkill)
    {
        // Initialize to the lowest value in the enum
        RarityType maxRarityTypeValue = RarityType.Common;
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill == unitSkill)
                    {
                        RarityType currentRarityTypeValue = ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._rarityType;
                        maxRarityTypeValue = (RarityType)Math.Max((int)maxRarityTypeValue, (int)currentRarityTypeValue);
                    }
                }
            }
        }
        return maxRarityTypeValue;
    }

    #region skill Increase coin
    public void Skill_Increase_Coin(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Increase_Coin;
        if (unitSkill == disSkill && actived)
        {
            Debug.Log("Test action function Coin");
            return;
        }
        else if (unitSkill == disSkill && !actived)
        {
            return;
        }
    }
    #endregion

    #region skill Increase EXP
    public void Skill_Increase_EXP(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Increase_EXP;
        if (unitSkill == disSkill && actived)
        {
            Debug.Log("Test action function: " + unitSkill.ToString() + "=>" + rarityType.ToString());
            return;
        }
        else if (unitSkill == disSkill && !actived)
        {
            Debug.Log("Test Unaction function: " + unitSkill.ToString() + "=>" + rarityType.ToString());
            return;
        }
    }
    #endregion

    #region skill extendperiod plant gold growtime
    public void Skill_Extendperiod_PlantGold_GrowTime(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Extendperiod_PlantGold_GrowTime;
        //Actived
        if (actived && disSkill == unitSkill)
        {
            List<CharacterData> GoldPlant = new List<CharacterData>();
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData goldData = new CharacterData();
                            goldData.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            goldData.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultGrowTimeGoldPlant(goldData);
                            GoldPlant.Add(goldData);
                        }
                    }
                }
            }
            for (int i = 0; i < GoldPlant.Count; i++)
            {
                switch (rarityType)
                {
                    case RarityType.Common:
                        GoldPlant[i].unitData.DecayTime += CalculateTimeGrowGoldPlant(110, GoldPlant[i].unitData.DecayTime);
                        break;
                    case RarityType.Rare:
                        GoldPlant[i].unitData.DecayTime += CalculateTimeGrowGoldPlant(120, GoldPlant[i].unitData.DecayTime);
                        break;
                    case RarityType.Epic:
                        GoldPlant[i].unitData.DecayTime += CalculateTimeGrowGoldPlant(130, GoldPlant[i].unitData.DecayTime);
                        break;
                    case RarityType.Legendary:
                        GoldPlant[i].unitData.DecayTime += CalculateTimeGrowGoldPlant(150, GoldPlant[i].unitData.DecayTime);
                        break;
                }
            }
        }
        //Unactived
        else if (!actived && disSkill == unitSkill)
        {
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData goldData = new CharacterData();
                            goldData.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            goldData.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultGrowTimeGoldPlant(goldData);
                        }
                    }
                }
            }
        }
    }
    public void setDefaultGrowTimeGoldPlant(CharacterData dataGoldList)
    {
        for (int i = 0; i < StakeUnitObject.instance._allPlantInfoDataList.Count; i++)
        {
            if (dataGoldList.unitData._unitTokenID == StakeUnitObject.instance._allPlantInfoDataList[i].plantID)
            {
                dataGoldList.unitData.DecayTime = StakeUnitObject.instance._allPlantInfoDataList[i].decayTime;
            }
        }
    }
    public int CalculateTimeGrowGoldPlant(int part, int whole)
    {
        int num = Mathf.FloorToInt((part * 1.0f / 100) * whole);
        return num;
    }
    #endregion

    #region skill Increase plant stack
    public void Skill_Increase_PlantStack(bool actived, UnitSkill unitSkill,RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Increase_PlantStack;
        //Actived
        if (unitSkill == disSkill && actived)
        {
            List<CharacterData> Plants = new List<CharacterData>();
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (!ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData Dataplant = new CharacterData();
                            Dataplant.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            Dataplant.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultPlantStack(Dataplant);
                            Plants.Add(Dataplant);
                        }
                    }
                }
            }
            for (int i = 0; i < Plants.Count; i++)
            {
                switch (rarityType)
                {
                    case RarityType.Common:
                        Plants[i].unitData._unitMaxStackCoine += 1;
                        break;
                    case RarityType.Rare:
                        Plants[i].unitData._unitMaxStackCoine += 1;
                        break;
                    case RarityType.Epic:
                        Plants[i].unitData._unitMaxStackCoine += 2;
                        break;
                    case RarityType.Legendary:
                        Plants[i].unitData._unitMaxStackCoine += 3;
                        break;
                }
            }
            StartCoroutine(get_unitCountTime());
        }
        else if (unitSkill == disSkill && !actived)
        {
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (!ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData Dataplant = new CharacterData();
                            Dataplant.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            Dataplant.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultPlantStack(Dataplant);
                        }
                    }
                }
            }
        }
    }
    public void setDefaultPlantStack(CharacterData dataplantList)
    {
        for (int i = 0; i < StakeUnitObject.instance._allPlantInfoDataList.Count; i++)
        {
            if (dataplantList.unitData._unitTokenID == StakeUnitObject.instance._allPlantInfoDataList[i].plantID)
            {
                dataplantList.unitData._unitMaxStackCoine = StakeUnitObject.instance._allPlantInfoDataList[i].growStack_1;
            }
        }
    }
    IEnumerator get_unitCountTime()
    {
        IWSResponse response = null;
        yield return UserPlant.GetAllPlantProgress(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        List<UserPlant> userPlantsList = UserPlant.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userPlantsList.Count; i++)
        {
            setUnitCountTimePlante(userPlantsList[i]);
        }
    }
    public void setUnitCountTimePlante(UserPlant userPlant)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == _thisZone)
            {
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; z++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCurrentPlant == userPlant.id)
                    {
                        ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitDateTimeStamp = userPlant.plantedTime;
                        ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine = setNowStack(userPlant.timeStamp, DateTime.Now, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z]);

                        if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine >= ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitMaxStackCoine)
                        {
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin * userPlant.pendingReward) + ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin;
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                        else
                        {
                            double x = setTimeNowStaking(userPlant.timeStamp, DateTime.Now);
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = x;//% ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin;
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                    }
                }
            }
        }
    }
    public int setNowStack(DateTime timeOld, DateTime timeNow, CharacterData data)
    {
        int nowStack = 0;
        TimeSpan timeSpan = timeNow - timeOld;
        float coinsEarned = (float)timeSpan.TotalSeconds / data.unitData.timePerCoin;
        nowStack = Mathf.FloorToInt(coinsEarned);
        return nowStack;
    }
    public void setupGrowTime(double timeGrowth, CharacterData myData, UserPlant userPlant)
    {
        myData.unitData.isLife = userPlant.isActive;
        if (timeGrowth > (myData.unitData.GrowTime * 0.3f) && (timeGrowth < (myData.unitData.GrowTime * 0.6f)))
        {
            myData.detail._growthPlante = GrowthPlante.Baby;
        }
        else if (timeGrowth > myData.unitData.GrowTime)
        {
            myData.detail._growthPlante = GrowthPlante.Growth;
        }
        else
        {
            myData.detail._growthPlante = GrowthPlante.Seed;
        }
    }
    public double setTimeNowStaking(DateTime timeOld, DateTime timeNow)
    {
        double numDouble;
        TimeSpan timeSpan = timeNow - timeOld;
        numDouble = timeSpan.TotalSeconds + PlayerObject.instance.dateTimeServer;
        Debug.Log("Pase to double: " + numDouble);
        return numDouble;
    }
    #endregion

    #region Skill Sale Price Gacha Plant
    public void Skill_Sale_PriceGachaPlant(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Sale_PriceGachaPlant;
        SpinLayerController.instance.coine_Price_text.text = "4,000";
        SpinLayerController.instance.coine10_Price_text.text = "36,000";
        if (unitSkill == disSkill && actived)
        {
            switch (rarityType)
            {
                case RarityType.Common:
                    SpinLayerController.instance.coine_Price_text.text = CalculateSale(4000, 1).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.text = CalculateSale(36000, 1).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.color = actived_color;
                    SpinLayerController.instance.coine_Price_text.color = actived_color;
                    break;
                case RarityType.Rare:
                    SpinLayerController.instance.coine_Price_text.text = CalculateSale(4000, 2).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.text = CalculateSale(36000, 2).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.color = actived_color;
                    SpinLayerController.instance.coine_Price_text.color = actived_color;
                    break;
                case RarityType.Epic:
                    SpinLayerController.instance.coine_Price_text.text = CalculateSale(4000, 3).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.text = CalculateSale(36000, 3).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.color = actived_color;
                    SpinLayerController.instance.coine_Price_text.color = actived_color;
                    break;
                case RarityType.Legendary:
                    SpinLayerController.instance.coine_Price_text.text = CalculateSale(4000, 5).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.text = CalculateSale(36000, 5).ToString("#,##0#");
                    SpinLayerController.instance.coine10_Price_text.color = actived_color;
                    SpinLayerController.instance.coine_Price_text.color = actived_color;
                    break;
            }
        }
        else if (unitSkill == disSkill && !actived)
        {
            SpinLayerController.instance.coine_Price_text.text = "4,000";
            SpinLayerController.instance.coine10_Price_text.text = "36,000";
            SpinLayerController.instance.coine10_Price_text.color = Color.white;
            SpinLayerController.instance.coine_Price_text.color = Color.white;
        }
    }
    public int CalculateSale(int price, int part)
    {
        int num = price - Mathf.FloorToInt((part * 1.0f / 100f) * price);
        return num;
    }
    #endregion

    #region skill sale price funiture
    public void Skill_Sale_PriceFunitrue(bool actived, UnitSkill unitSkill ,RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Sale_PriceFunitrue;
        for (int i = 0; i < FurnitureUnitObject.instance.all_furnitureDetails.Count; i++)
        {
            setPriceFunitrue(FurnitureUnitObject.instance.all_furnitureDetails[i]);
        }
        if (unitSkill == disSkill && actived)
        {
            switch (rarityType)
            {
                case RarityType.Common:
                    setUnitpriceSale(1);
                    break;
                case RarityType.Rare:
                    setUnitpriceSale(2);
                    break;
                case RarityType.Epic:
                    setUnitpriceSale(3);
                    break;
                case RarityType.Legendary:
                    setUnitpriceSale(5);
                    break;
            }
        }
        else if (unitSkill == disSkill && !actived)
        {
            for (int i = 0; i < FurnitureUnitObject.instance.all_furnitureDetails.Count; i++)
            {
                setPriceFunitrue(FurnitureUnitObject.instance.all_furnitureDetails[i]);
            }
        }
    }
    public void setPriceFunitrue(FurnitureDetail furnitureDetail)
    {
        for (int i = 0; i < FurnitureUnitObject.instance._allDecorationsDataList.Count; i++)
        {
            if (furnitureDetail.unitID == FurnitureUnitObject.instance._allDecorationsDataList[i].itemID)
            {
                furnitureDetail.unitPrice = FurnitureUnitObject.instance._allDecorationsDataList[i].price;
            }
        }
    }
    public void setUnitpriceSale(int part)
    {
        for (int i = 0; i < FurnitureUnitObject.instance.all_furnitureDetails.Count; i++)
        {
            FurnitureUnitObject.instance.all_furnitureDetails[i].unitPrice = CalculateSale(FurnitureUnitObject.instance.all_furnitureDetails[i].unitPrice, part);
        }
    }
    #endregion

    #region skill Sale PriceBlock
    public void Skill_Sale_PriceBlock(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Sale_PriceBlock;
        for (int i = 0; i < StakeLayerController.instance.tsc.slotsList.Count; i++)
        {
            if (StakeLayerController.instance.tsc.slotsList[i]._moneyType == moneyType.Coine)
            {
                resetPriceBlock(StakeLayerController.instance.tsc.slotsList[i]);
            }
        }
        if (unitSkill == disSkill && actived)
        {
            switch (rarityType)
            {
                case RarityType.Common:
                    setUnitPriceSaleBlock(1);
                    break;
                case RarityType.Rare:
                    setUnitPriceSaleBlock(2);
                    break;
                case RarityType.Epic:
                    setUnitPriceSaleBlock(3);
                    break;
                case RarityType.Legendary:
                    setUnitPriceSaleBlock(5);
                    break;
            }
        }
        else if (unitSkill == disSkill && !actived)
        {
            for (int i = 0; i < StakeLayerController.instance.tsc.slotsList.Count; i++)
            {
                if (StakeLayerController.instance.tsc.slotsList[i]._moneyType == moneyType.Coine)
                {
                    resetPriceBlock(StakeLayerController.instance.tsc.slotsList[i]);
                }
            }
        }
    }
    public void resetPriceBlock(TeamSlot slot)
    {
        for (int b = 0; b < StakeUnitObject.instance._allBlockInfoDataList.Count; b++)
        {
            if ((slot._blockID == StakeUnitObject.instance._allBlockInfoDataList[b].blockID) && (slot._areaID == StakeUnitObject.instance._allBlockInfoDataList[b].area))
            {
                slot._priceThisSlot = StakeUnitObject.instance._allBlockInfoDataList[b].price;
                slot.price_text.text = StakeUnitObject.instance._allBlockInfoDataList[b].price.ToString("#,##0") + " c";
                slot.price_text.color = Color.white;
            }
        }
    }
    public void setUnitPriceSaleBlock(int part)
    {
        for (int i = 0; i < StakeLayerController.instance.tsc.slotsList.Count; i++)
        {
            if (StakeLayerController.instance.tsc.slotsList[i]._moneyType == moneyType.Coine)
            {
                StakeLayerController.instance.tsc.slotsList[i]._priceThisSlot = CalculateSale(StakeLayerController.instance.tsc.slotsList[i]._priceThisSlot, part);
                StakeLayerController.instance.tsc.slotsList[i].price_text.text = CalculateSale(StakeLayerController.instance.tsc.slotsList[i]._priceThisSlot, part).ToString("#,##0") + " c";
                StakeLayerController.instance.tsc.slotsList[i].price_text.color = actived_color;
            }
        }
    }
    #endregion

    #region Skill Speed GrowTime
    public void Skill_Speed_GrowTime(bool actived, UnitSkill unitSkill, RarityType rarityType)
    {
        UnitSkill disSkill = UnitSkill.Speed_GrowTime;
        if (unitSkill == disSkill && actived)
        {
            List<CharacterData> Plants = new List<CharacterData>();
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (!ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData Dataplant = new CharacterData();
                            Dataplant.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            Dataplant.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultGrowTimePlant(Dataplant);
                            Plants.Add(Dataplant);
                        }
                    }
                }
            }
            for (int i = 0; i < Plants.Count; i++)
            {
                switch (rarityType)
                {
                    case RarityType.Common:
                        Plants[i].unitData.GrowTime -= CalculateTimeGrowGoldPlant(2, Plants[i].unitData.GrowTime);
                        Plants[i].unitData.timePerCoin -= CalculateTimeGrowGoldPlant(2, Plants[i].unitData.timePerCoin);
                        break;
                    case RarityType.Rare:
                        Plants[i].unitData.GrowTime -= CalculateTimeGrowGoldPlant(4, Plants[i].unitData.GrowTime);
                        Plants[i].unitData.timePerCoin -= CalculateTimeGrowGoldPlant(4, Plants[i].unitData.timePerCoin);
                        break;
                    case RarityType.Epic:
                        Plants[i].unitData.GrowTime -= CalculateTimeGrowGoldPlant(6, Plants[i].unitData.GrowTime);
                        Plants[i].unitData.timePerCoin -= CalculateTimeGrowGoldPlant(6, Plants[i].unitData.timePerCoin);
                        break;
                    case RarityType.Legendary:
                        Plants[i].unitData.GrowTime -= CalculateTimeGrowGoldPlant(10, Plants[i].unitData.GrowTime);
                        Plants[i].unitData.timePerCoin -= CalculateTimeGrowGoldPlant(10, Plants[i].unitData.timePerCoin);
                        break;
                }
            }
        }
        else if (unitSkill == disSkill && !actived)
        {
            for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
                {
                    for (int p = 0; p < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; p++)
                    {
                        if (!ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail._planeNFT)
                        {
                            CharacterData Dataplant = new CharacterData();
                            Dataplant.detail = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].detail;
                            Dataplant.unitData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[p].unitData;
                            setDefaultGrowTimePlant(Dataplant);
                        }
                    }
                }
            }
        }
    }
    public void setDefaultGrowTimePlant(CharacterData dataGoldList)
    {
        for (int i = 0; i < StakeUnitObject.instance._allPlantInfoDataList.Count; i++)
        {
            if (dataGoldList.unitData._unitTokenID == StakeUnitObject.instance._allPlantInfoDataList[i].plantID)
            {
                dataGoldList.unitData.GrowTime = StakeUnitObject.instance._allPlantInfoDataList[i].growTime;
                dataGoldList.unitData.timePerCoin = StakeUnitObject.instance._allPlantInfoDataList[i].timePerCoin;
            }
        }
    }
    #endregion

    #endregion
}
