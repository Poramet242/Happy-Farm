using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class UnlockSlotReward : MonoBehaviour
{
    public static UnlockSlotReward instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] GameObject thisObject;
    [SerializeField] GameObject Content_rewardToUnlockSlot;
    [SerializeField] GameObject tempPlantReward_unit;
    [SerializeField] GameObject slotReward_img;

    [SerializeField] int CountPlanteList;
    [SerializeField] List<GameObject> unitRewardList;
    [SerializeField] List<CharacterData> characterDatasReward;

    private void OnEnable()
    {
        if (unitRewardList.Count > 0)
        {
            for (int i = 0; i < unitRewardList.Count; i++)
            {
                Destroy(unitRewardList[i]);
            }
            unitRewardList.Clear();
            characterDatasReward.Clear();
        }
    }
    public void getRewardInData(List<UnlockBlockReward> blockRewards)
    {
        for (int i = 0; i < blockRewards.Count; i++)
        {
            CountPlanteList = blockRewards[i].amount;
            StartCoroutine(setRewardPlanteData(blockRewards[i].plantID, blockRewards[i].amount));
        }
    }
    IEnumerator setRewardPlanteData(string plantID,int count)
    {
        IWSResponse response = null;
        yield return PlantInfo.GetPlantInfo(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<PlantInfo> plantInfos = PlantInfo.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < plantInfos.Count; i++)
        {
            if (plantInfos[i].plantID == plantID)
            {
                for (int c = 0; c < count; c++)
                {
                    UnitDetail unitDetail = ScriptableObject.CreateInstance<UnitDetail>();
                    UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
                    unitDetail._unitTokenID = plantInfos[i].plantID;
                    unitData._unitTokenID = plantInfos[i].plantID;
                    StakeUnitObject.instance.SetUpDataPlantinfo(plantInfos[i], unitDetail, unitData);
                    CharacterData Selldata = new CharacterData();
                    Selldata.detail = unitDetail;
                    Selldata.unitData = unitData;
                    characterDatasReward.Add(Selldata);
                    //StakeUnitObject.instance._allSellUnitDataList.Add(Selldata);
                }
            }
        }
        //other reward to data
        for (int i = 0; i < characterDatasReward.Count; i++)
        {
            if (characterDatasReward[i].unitData._unitTokenID == plantID)
            {
                setUnitDisplay(characterDatasReward[i]);
                break;
            }
        }

    }
    public void setUnitDisplay(CharacterData characterData)
    {
        for (int i = 0; i < 1; i++)
        {
            GameObject unitPlant = Instantiate(tempPlantReward_unit, Content_rewardToUnlockSlot.transform);
            unitPlant.SetActive(true);
            unitPlant.GetComponent<RewardPlanteDisplay>().setupDataRewardDisplay(characterData, CountPlanteList);
            unitRewardList.Add(unitPlant);
        }
    }
    public void onClickConfirm()
    {
        StartCoroutine(resetUsePlant());
    }
    IEnumerator resetUsePlant()
    {
        //Clear Data All plant
        StakeUnitObject.instance._allUnitDetailList.Clear();
        StakeUnitObject.instance._allUnitDataList.Clear();
        IWSResponse response = null;
        yield return UserSeed.GetUserSeed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<UserSeed> seeds = UserSeed.ParseToList(response.RawResult().ToString());
        Debug.Log("plantInfos: " + seeds.Count);
        for (int i = 0; i < seeds.Count; i++)
        {
            UnitDetail unitDetail = ScriptableObject.CreateInstance<UnitDetail>();
            UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
            //set plant ID
            unitDetail._unitTokenID = seeds[i].plantID;
            unitData._unitTokenID = seeds[i].plantID;
            unitData._unitCurrentPlant = seeds[i].id;
            unitDetail._unitCurrentPlant = seeds[i].id;
            var plant = GameData.instance.GetPlantInfoByPlantID(unitDetail._unitTokenID);
            StakeUnitObject.instance.SetUpDataPlantinfo(plant, unitDetail, unitData);
            StakeUnitObject.instance._allUnitDetailList.Add(unitDetail);
            StakeUnitObject.instance._allUnitDataList.Add(unitData);
        }
        if (!PlayerObject.instance._checkplayTutorial)
        {
            StakeLayerController.instance.oncloseTutorial();
        }
        thisObject.SetActive(false);
    }
}
