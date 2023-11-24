using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XSystem;

public class CalculateTimeObject : MonoBehaviour
{
    public static CalculateTimeObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

    }
    private void Start()
    {
        StartCoroutine(setDateTime());
    }
    IEnumerator setDateTime()
    {
        IWSResponse response = null;
        yield return TimeNow.GetTimeNow(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var time = response as TimeNow;
        PlayerObject.instance.dateTimecooldown = (int)(time.timeNow - DateTime.Now).TotalSeconds;
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            settTimeCooldownCannabis(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone, PlayerObject.instance.dateTimecooldown);
        }
        //Debug.Log("Pase to double: " + PlayerObject.instance.dateTimecooldown);
    }
    void Update()
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            //updateTimeToAssisstant(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone);
            updateTimeToCannbis(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone);
        }
    }
    public void updateTimeToAssisstant(List<AssisstantDetail> assisstantDetail)
    {
        if (assisstantDetail == null || assisstantDetail.Count ==0)
        {
            return;
        }
        for (int i = 0; i < assisstantDetail.Count; i++)
        {
            if (assisstantDetail[i] != null)
            {
                assisstantDetail[i]._unitTimeAssis += Time.deltaTime;
            }
        }
    }

    #region plant
    public void updateTimeToCannbis(List<CharacterData> characterDatas)
    {
        if (characterDatas == null || characterDatas.Count == 0)
        {
            return;
        }
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i] != null)
            {
                switch (characterDatas[i].detail._growthPlante)
                {
                    case GrowthPlante.Seed:
                        characterDatas[i].unitData._unitCountTime += Time.deltaTime;
                        break;
                    case GrowthPlante.Baby:
                        characterDatas[i].unitData._unitCountTime += Time.deltaTime;
                        break;
                    case GrowthPlante.Growth:
                        if (characterDatas[i].unitData._unitStackCoine >= characterDatas[i].unitData._unitMaxStackCoine)
                        {
                            continue;
                        }
                        else
                        {
                            characterDatas[i].unitData._unitCountTimeHaver += Time.deltaTime;
                            characterDatas[i].unitData._unitCountTime += Time.deltaTime;
                        }
                        break;
                    case GrowthPlante.Rotted:
                        if (!characterDatas[i].unitData.isLife)
                        {
                            continue;
                        }
                        break;
                }

                /*
                if ((characterDatas[i].detail._growthPlante == GrowthPlante.Seed)||(characterDatas[i].detail._growthPlante == GrowthPlante.Baby))
                {
                    characterDatas[i].unitData._unitCountTime += Time.deltaTime;
                }
                else if(characterDatas[i].detail._growthPlante == GrowthPlante.Growth)
                {
                    if (characterDatas[i].unitData._unitStackCoine >= characterDatas[i].unitData._unitMaxStackCoine)
                    {
                        continue;
                    }
                    characterDatas[i].unitData._unitCountTimeHaver += Time.deltaTime;
                    characterDatas[i].unitData._unitCountTime += Time.deltaTime;
                }
                else if ((characterDatas[i].detail._growthPlante == GrowthPlante.Rotted) || (!characterDatas[i].unitData.isLife))
                {
                    continue;
                }
                else
                {
                    return;
                }
                */
            }
        }
    }
    public void settTimeCooldownCannabis(List<CharacterData> characterDatas, double num)
    {
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i] != null)
            {
                if ((characterDatas[i].detail._growthPlante == GrowthPlante.Seed) || (characterDatas[i].detail._growthPlante == GrowthPlante.Baby))
                {
                    characterDatas[i].unitData._unitDateTimeStamp.AddSeconds(num);
                    //characterDatas[i].unitData._unitCountTime += num;
                    
                }
                else if (characterDatas[i].detail._growthPlante == GrowthPlante.Growth)
                {
                    characterDatas[i].unitData._unitDateTimeStamp.AddSeconds(num);
                    //characterDatas[i].unitData._unitCountTimeHaver += num;
                }
                else if ((characterDatas[i].detail._growthPlante == GrowthPlante.Rotted) || (!characterDatas[i].unitData.isLife))
                {
                    continue;
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion
}
