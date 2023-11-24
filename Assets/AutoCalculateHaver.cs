using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XSystem;

public class AutoCalculateHaver : MonoBehaviour
{
    public static AutoCalculateHaver instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Auto Harvest")]
    [SerializeField] public bool isAuto;
    [SerializeField] public ZoneType thisZone;
    [SerializeField] public List<AssisstantDetail> assistantDispaly = new List<AssisstantDetail>();
    [Header("Plant Data")]
    [SerializeField] public CharacterData characterData;
    private void Start()
    {
        StartCoroutine(AutoHaverSkill());
    }
    public IEnumerator AutoHaverSkill()
    {
        while (true)
        {
            for (int i = 0; i < assistantDispaly.Count; i++)
            {
                isAuto = true;
                assistantDispaly[i].UpdateAutoHaverSkill();
                if (assistantDispaly[i].startAuto)
                {
                    StartCoroutine(AutoSkill());
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator setUnitTimeAuto(AssisstantDetail detail)
    {
        IWSResponse response = null;
        yield return Assistant.GetAssistants(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<Assistant> assistant_data = Assistant.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < assistant_data.Count; i++)
        {
            if (assistant_data[i].tokenID == detail._unitTokenID)
            {
                detail._unitTimeWorkAuto = DateTime.Now;//assistant_data[i].autoHarvestTimeStamp;
                detail.stampTime = true;
                detail.currentHours = CountTimeRarityConvert(detail._rarityType);
                break;
            }
        }
        detail.isAutoActive = true;
    }
    public float CountTimeRarityConvert(RarityType rarityType)
    {
        float count = 0f;
        switch (rarityType)
        {
            case RarityType.Common:
                count = 24f;
                break;
            case RarityType.Rare:
                count = 12f;
                break;
            case RarityType.Epic:
                count = 8f;
                break;
            case RarityType.Legendary:
                count = 4f;
                break;
        }
        return count;
    }
    public void setupAssistantAutoDispalyList(AssisstantDetail assisstant)
    {
        assistantDispaly.Add(assisstant);
        StartCoroutine(setUnitTimeAuto(assisstant));
    }
    IEnumerator AutoSkill()
    {
        IWSResponse response = null;
        yield return AutoHarvestAPI.AutoHarvest(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        Debug.Log(response.RawResult().ToString());
        var autoSkill = response as AutoHarvestAPI;
        characterData = new CharacterData();
        for (int i = 0; i < autoSkill.autoHarvestDatas.Count; i++)
        {
            getPlantDataInzone(autoSkill.autoHarvestDatas[i].id, autoSkill.autoHarvestDatas[i].areaID, autoSkill.autoHarvestDatas[i].blockID);
        }
    }
    public void getPlantDataInzone(string tokenId, string areaId, string blockID)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == thisZone)
            {
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; z++)
                {
                    if ((ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].detail._unitTokenID == tokenId)
                        && ((int)ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].detail._zonePos == int.Parse(areaId))
                        && ((ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].detail._unitPos - 1) == int.Parse(blockID)))
                    {
                        characterData = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z];
                    }
                }
            }
        }
        StakeLayerController.instance.AutoSetEffectHarvest(characterData);
    }
    public void DeleteAssistantAutoDispalyList(AssisstantDetail assisstant)
    {
        assistantDispaly.Remove(assisstant);
        assisstant.isAutoActive = false;
    }
}
