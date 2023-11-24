using CannabisFarm.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

[System.Serializable]
public class CharacterData
{
    public UnitDetail detail;
    public UnitData unitData;
}
public class StakeUnitObject : MonoBehaviour
{
    public static StakeUnitObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Staking")]
    [SerializeField] public ZoneType zone;
    [Header("All unitInventory")]
    [SerializeField] public List<UnitDetail> _allUnitDetailList;
    [SerializeField] public List<UnitData> _allUnitDataList;
    [Header("All Sellunit")]
    [SerializeField] public List<CharacterData> _allSellUnitDataList;
    [Header("Plant info Data")]
    [SerializeField] public List<PlantInfo> _allPlantInfoDataList;
    [Header("Block info Data")]
    [SerializeField] public List<BlockInfo> _allBlockInfoDataList;
    //get count species
    public int getCountBySperity(string plantID)
    {
        int count = 0;
        for (int i = 0; i < _allUnitDetailList.Count; i++)
        {
            if (_allUnitDetailList[i]._unitTokenID == plantID)
            {
                count++;
            }
        }
        return count;
    }
    //get all unit species
    public List<string> getAllUnitSperity()
    {
        List<string> temp = new List<string>();
        for (int i = 0; i < _allUnitDetailList.Count; i++)
        {
            if (!temp.Exists(o => o == _allUnitDetailList[i]._unitTokenID))
            {
                temp.Add(_allUnitDetailList[i]._unitTokenID);
            }
        }
        return temp;
    }
    //set up plant in fo
    public void SetUpDataPlantinfo(PlantInfo plantInfos, UnitDetail unitDetail, UnitData unitData)
    {
        if (plantInfos.rewardAmountNFT > 0)
        {
            unitDetail._planeNFT = true;
            unitDetail._plantType = PlantType.Golden;
        }
        else
        {
            unitDetail._planeNFT = false;
            unitDetail._plantType = PlantType.Normal;
        }
        unitDetail.name = plantInfos.name;
        unitData.name = plantInfos.name;
        unitDetail._unitName = plantInfos.name;
        unitDetail._speciesType = plantInfos.species;
        //--------------------------------------------------------------------------------------------
        unitDetail._unitLocalImage = UnitDataLoader.Instance.GetLocalImages(unitDetail._unitTokenID);
        unitDetail._plant_Image = UnitDataLoader.Instance.GetLocalPlant(unitDetail._unitTokenID);
        unitDetail._unitPrefab = UnitDataLoader.Instance.GetLocalObjPlant(unitDetail._unitTokenID);
        if (unitDetail._planeNFT)
        {
            unitDetail._unitParticle = UnitDataLoader.Instance.GetLocalParticlePlant("plant_21");
        }
        else
        {
            unitDetail._unitParticle = UnitDataLoader.Instance.GetLocalParticlePlant("plant_1");
        }
        //--------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------
        unitData._maxCountPlantGold = plantInfos.buyLimitPerMonth;
        //unitData._currentCountPlantGold = plantInfos.
        //--------------------------------------------------------------------------------------------
        unitData._unitCanBuy = plantInfos.canBuy;
        unitData._unitName = plantInfos.name;
        unitData._priceBuyPlane = plantInfos.buyPrice;
        unitData._priceSellPlane = plantInfos.sellPrice;
        unitData._unitCountPlane = 1;
        unitData._unitCoineMax = plantInfos.rewardAmount;
        unitData._unitTokenNFTMax = plantInfos.rewardAmountNFT;
        unitData._unitMaxStackCoine = plantInfos.growStack_1;
        unitData.GrowTime = plantInfos.growTime;
        unitData.DecayTime = plantInfos.decayTime;//unitData.setDataCountTime(plantInfos.decayTime);
        unitData.timePerCoin = plantInfos.timePerCoin;
        unitData._unitInfo = plantInfos.documentPlant;
        unitDetail._unitData = unitData;
    }
    //How to use code 
    // List<SpeciesType> test = StakeUnitObject.instance.getAllUnitSperity();
    // Debug.Log("SpeciesType: " + test);
    // for (int i = 0; i<test.Count; i++)
    // {
    //     Debug.Log(test[i] +" count: " + StakeUnitObject.instance.getCountBySperity(test[i]));
    // }
}
