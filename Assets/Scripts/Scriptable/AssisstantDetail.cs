using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using XSystem;

[System.Serializable]
[CreateAssetMenu(fileName = "AssisstantDeatil", menuName = "ScriptableObjects/AssisstantDeatil", order = 3)]
public class AssisstantDetail : ScriptableObject
{
    public bool _unitWork;
    public int _unitTokenID;
    public string _unitName;
    public string _unitImageID;
    public int _unitPos;
    [Header("Skill")]
    public UnitSkill _skill;
    [Header("Data Assistant")]
    public double _unitTimeAssis;
    public DateTime _unitDateTimeStampAssis;
    [Header("Zone")]
    public ZoneType _zonePos;
    [Header("Image Type")]
    public Sprite _unitLocalImage;
    public RarityType _rarityType;
    [Header("Animator")]
    public GameObject _unitPrefab;
    [Header("Skill Auto")]
    public DateTime _unitTimeWorkAuto;
    public bool isAutoActive;
    public bool stampTime = false;
    public bool startAuto = false;
    public float currentHours = 0;
    public int lastUpdateHours = -1;
    public void resetDateTime()
    {
        _unitTimeAssis = 0;
        _unitDateTimeStampAssis = DateTime.Now;
    }
    public void DeletePlayerprefsAss(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }
    public void UpdateAutoHaverSkill()
    {
        if (isAutoActive)
        {
            TimeSpan elapsedTime = DateTime.Now - _unitTimeWorkAuto;
            string timeText = string.Format("{0:00}:{1:00}:{2:00}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
            //currentSecond = one update to instantiate
            //TODO: use elapsedTime.Hours to playgame
            int currentSecond = elapsedTime.Hours;
            if (currentSecond != lastUpdateHours && currentSecond % currentHours == 0)
            {
                if (currentSecond != 0)
                {
                    startAuto = true;
                    //Debug.Log(_unitName + " => " + timeText);
                    lastUpdateHours = currentSecond;
                }
            }
        }
        else
        {
            stampTime = false;
            startAuto = false;
            lastUpdateHours = -1; // Reset the lastInstantiatedSecond when auto update is deactivated
        }
    }
}
public enum UnitSkill
{
    none = 0,
    Increase_Coin = 1,
    Increase_EXP = 2,
    Extendperiod_PlantGold_GrowTime = 3,
    Increase_PlantStack = 4,
    Sale_PriceGachaPlant = 5,
    Sale_PriceFunitrue = 6,
    Sale_PriceBlock = 7,
    Speed_GrowTime = 8,
}

