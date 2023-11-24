using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using System.Reflection;
using System.Linq;

public enum TeamSlotState
{
    idle = 0,
    select = 2
}

public class TeamSlotController : MonoBehaviour
{

    public bool isTest = false;
    public List<CharacterData> testingDatas;
    public ZoneType zones;
    public bool isStakingLayer;
    public ZoneType currentZone;
    public int currentUnlockSlot;
    public TeamSlotState state = TeamSlotState.idle;
    public List<TeamSlot> slotsList;

    public int slotSelectedIndex;
    [Header("Stake Zone")]
    public bool isLockSlot;
    
    [Header("Mouse Event Check")]
    [SerializeField]  GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;

    private void Awake() {
        if(isTest)
        {
            Setup(testingDatas);
        }
        zones = PlayerObject.instance._zone;
    }

    public void Setup(List<CharacterData> datas) 
    {
        for (int i = 0; i < slotsList.Count; i++)
        {
            int t = i;
            slotsList[i].Setup(t, true, this);
            /*if (i <= currentUnlockSlot)
            {
                slotsList[i].Setup(t, false, this);
            }
            else 
            {
               slotsList[i].Setup(t, true, this);
            }*/
            /*if (datas == null || datas.Count == 0)
            {
                continue;
            }
            int index = datas.FindIndex(o => o.detail._unitPos == i);
            if (index != -1)
            {
                slotsList[i].AddUnitToSlot(datas[index]);
            }*/
        }
        AddMutipleSlots(datas);
        HideAllSlotBtn();
    }
    
    private void Update() 
    {
       //if(Input.GetMouseButtonUp(0))
       //{
       //    m_PointerEventData = new PointerEventData(m_EventSystem);
       //    m_PointerEventData.position = Input.mousePosition;
       //    List<RaycastResult> results = new List<RaycastResult>();
       //    m_Raycaster.Raycast(m_PointerEventData, results);
       //
       //    if(results.Count > 0)
       //    {
       //        if(results[0].gameObject.tag != "UnitSlot")
       //        {
       //            HideAllSlotBtn();
       //        }
       //    }
       //    else
       //    {
       //        HideAllSlotBtn();
       //    }
       //}
    }

    public bool HasLeader()
    {
        return slotsList[0].hasDataInSlot;
    }

    public void OnSlotClick(int index) 
    {
        if(isLockSlot) return;
        state = TeamSlotState.select;
        slotSelectedIndex = index;
        for (int i = 0; i < slotsList.Count; i++)
        {
            if(index == i)
            {
                slotsList[i].ShowOptionBtn();
            }
            else
            {
                slotsList[i].HideAllBtn();
            }
        }
    }
    #region unlock MutipleSlots by Graun
    public void AddMutipleSlots(List<CharacterData> datas)
    {
        if (datas != null && datas.Count > 0)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                int slotIndex = datas[i].detail._unitPos;
                if (slotIndex >= 0 && slotIndex < slotsList.Count)
                {
                    slotsList[slotIndex].Setup(slotIndex, false, this);
                    slotsList[slotIndex].AddUnitToSlot(datas[i]);
                }
            }
        }
        //var zoneUnitObject it's call function ZoneUnitObject.instance
        var zoneUnitObject = ZoneUnitObject.instance;
        //var validZones it's check value in unitDataZones where zone = this zone pase to list
        var validZones = zoneUnitObject.unitDataZones
            .Where(zone => zone.ZoneType == zones)
            .ToList();
        if (validZones.Count == 0)
        {
            return;
        }
        //var zoneslots it's check value in list validZones
        var zoneSlots = validZones[0]._slotDataThisZone;
        for (int z = 0; z < Math.Min(zoneSlots.Count, slotsList.Count); z++)
        {
            if (zoneSlots[z])
            {
                slotsList[z].Setup(z, false, this);
            }
        }
    }
    public void unlockMutipleSlots(List<CharacterData> characterDatas, int countUnlock)
    {
        if (characterDatas != null)
        {
            for (int i = 0; i < characterDatas.Count; i++)
            {
                int slotIndex = characterDatas[i].detail._unitPos;
                if (slotIndex < 0 || slotIndex > slotsList.Count) continue;
                if (slotsList[slotIndex].isLock)
                {
                    OnSlotUnlockBtnClick(slotIndex);
                }
            }
        }
        else
        {
            setupNoneDataMutipleSlots();
        }
    }
    public void setupNoneDataMutipleSlots()
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
            {
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone.Count; z++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone[z] == true)
                    {
                        if (slotsList[z].isLock)
                        {
                            OnSlotUnlockBtnClick(z);
                        }
                    }
                }
            }
        }
    }
    #endregion
    public void OnSlotUnlockBtnClick(int index)
    {
        if(isLockSlot) return;
        slotSelectedIndex = index;
        state = TeamSlotState.select;
        HideAllSlotBtn();
        // to do open unlock UI
        if (!StakeLayerController.instance.isCheckStartZone)
        {
            StakeLayerController.instance.onClickBuySlot(index,slotsList[index]._priceThisSlot, slotsList[index]._moneyType);
        }
        else
        {
            OnUnlockSlotComplete();
        }
    }
    public void OnUnlockSlotComplete()
    {
        if(slotSelectedIndex == -1) return;
        slotsList[slotSelectedIndex].isLock = false;
        state = TeamSlotState.idle;
        slotSelectedIndex = -1;
        HideAllSlotBtn();
    }
    public void OnSlotPlantBtnClick(int index)
    {
        if(isLockSlot) return;
        slotSelectedIndex = index;
        state = TeamSlotState.select;
        HideAllSlotBtn();
        // to do open invectory UI
        StakeLayerController.instance.plant_btn(true);
    }
    public void OnSelectCharacterToPlant(CharacterData data)
    {
        if(slotSelectedIndex == -1) return;
        slotsList[slotSelectedIndex].AddUnitToSlot(data);
        data.detail._unitPos = slotSelectedIndex;
        state = TeamSlotState.idle;
        slotSelectedIndex = -1;
        HideAllSlotBtn();
    }
    public void OnSlotHaverseBtnClick(int index)
    {
        Debug.Log("1");
        // to do calculate Amount of currency
        slotsList[index].GetComponent<RewardAnimation>().isHaver = true;
        StakeLayerController.instance.onClickHaver(slotsList[index].myData,index);
        HideAllSlotBtn();
        slotSelectedIndex = -1;
    }
    public void OnSlotDeleteBtnClick(int index)
    {
        if(isLockSlot || !PlayerObject.instance._checkplayTutorial) return;
        if(index != slotSelectedIndex)
        {
            HideAllSlotBtn();
            return;
        }
        //Graun: Chararcter Selected Display
        string tokenID = slotsList[index].myData.unitData._unitTokenID;
        StakeLayerController.instance.setupDisplayDelete(index);
    }

    public void HideAllSlotBtn()
    {
        if(isLockSlot) return;
        state = TeamSlotState.idle;
        for (int i = 0; i < slotsList.Count; i++)
        {
            slotsList[i].HideAllBtn();   
        }
        /*for (int i = 0; i < TeamLayerController.instance.CharacterInventoryDisplaylist.Count; i++)
        {
            TeamLayerController.instance.CharacterInventoryDisplaylist[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        for (int i = 0; i < StakeLayerController.instance.CharacterInventoryDisplaylist.Count; i++)
        {
            StakeLayerController.instance.CharacterInventoryDisplaylist[i].transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }*/
    }

    public List<CharacterData> GetListCharacterData()
    {
        List<CharacterData> result = new List<CharacterData>();
        for (int i = 0; i < slotsList.Count; i++)
        {
            if (isStakingLayer)
            {
                if (slotsList[i].hasDataInSlot)
                {
                    result.Add(slotsList[i].myData);
                }
                continue;
            }
            else
            {
                if (slotsList[i].hasDataInSlot)
                {
                    result.Add(slotsList[i].myData);
                }
                else
                {
                    result.Add(new CharacterData());
                }
            }
        }
        //Graun:Update show teamCp
        /*if (isStakingLayer)
        {
            TeamCP_stk.instance.setUadateTeamCP_stk(true);
            return result;
        }
        TeamCP.instance.setUpdateTeamCp(true);*/
        return result;
    }

    public void StartStaking()
    {
        isLockSlot = true;
        for (int i = 0; i < slotsList.Count; i++)
        {
            slotsList[i].StakeFxSet(slotsList[i].hasDataInSlot);
        }
    }

    public void StopStaking()
    {
        isLockSlot = false;
        for (int i = 0; i < slotsList.Count; i++)
        {
            slotsList[i].StakeFxSet(false);
        }
    }

    public void cleaData()
    {
        for (int i = 0; i < slotsList.Count; i++)
        {
            slotsList[i].deleteUnitDataInSlot();
        }
    }
}