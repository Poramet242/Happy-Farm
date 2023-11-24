using System;
using System.Collections.Generic;
using UnityEngine;


public class ZoneUnitObject : MonoBehaviour
{
    public static ZoneUnitObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("check none call data")]
    [SerializeField] public bool _checkNoneCalBackData;
    [Header("Zone Data")]
    [SerializeField] public List<UnitDataZone> unitDataZones = new List<UnitDataZone>();

    public int countAssisstantDetailThiszone(ZoneType zone)
    {
        int count = 0;
        for (int i = 0; i < unitDataZones.Count; i++)
        {
            if (unitDataZones[i].ZoneType == zone)
            {
                for (int z = 0; z < unitDataZones[i]._assisstantDetailThisZone.Count; z++)
                {
                    count = z + 1;
                }
            }
            else
            {
                break;
            }
        }
        return count;
    }
    /// <summary>
    /// this function is reset zone data to get new zone friend data 
    /// </summary>
    /// <param name="checkClear">check to reset data</param>
    public void resetDatathisZone(bool checkClear)
    {
        if (checkClear)
        {
            for (int i = 0; i < unitDataZones.Count; i++)
            {
                unitDataZones[i]._checkUnlock = false;
                unitDataZones[i]._countUnlockSlot = 0;
                unitDataZones[i]._cannaBisDatasThisZone.Clear();
                unitDataZones[i]._assisstantDetailThisZone.Clear();
                for (int j = 0; j < unitDataZones[i]._slotDataThisZone.Count; j++)
                {
                    unitDataZones[i]._slotDataThisZone[j] = false;
                }
            }
            StakeUnitObject.instance._allUnitDataList.Clear();
            StakeUnitObject.instance._allUnitDetailList.Clear();
            StakeUnitObject.instance._allBlockInfoDataList.Clear();
            StakeUnitObject.instance._allSellUnitDataList.Clear();
            AssistantsObject.instance._assistantsAllList.Clear();
            _checkNoneCalBackData = true;
        }
    }

}
[System.Serializable]
public  class UnitDataZone
{
    [Header("Data")]
    public ZoneType ZoneType;
    public bool _checkUnlock;
    public int _countUnlockSlot;
    public List<CharacterData> _cannaBisDatasThisZone = new List<CharacterData>();
    public List<AssisstantDetail> _assisstantDetailThisZone = new List<AssisstantDetail>();
    public List<bool> _slotDataThisZone = new List<bool>();
}
