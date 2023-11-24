using CannabisFarm.Models;
using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class AssistantsDisplay : MonoBehaviour
{
   public static AssistantsDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public AssisstantDetail _assistantsDataDetail;
    [SerializeField] private bool isCheckWork;
    [Header("Detail Display")]
    [SerializeField] private Text _nameAssistants;
    [SerializeField] private Image _imageAssistants;
    [SerializeField] private Image _frameAssistants;
    [SerializeField] private GameObject _chooseToWork;
    [Header("Assistants Work")]
    [SerializeField] private GameObject _workObj;
    [SerializeField] private GameObject _callBack;
    [SerializeField] private Text _timeWork;
    [SerializeField] private Text _zoneWork;

    public void setDataAssistantsDisplay(GameObject temp, AssisstantDetail data)
    {
        _assistantsDataDetail = data;
        setUpDisplayAsssistants(_assistantsDataDetail);
    }
    public void setUpDisplayAsssistants(AssisstantDetail detail)
    {
        _nameAssistants.text = detail._unitName;
        _imageAssistants.sprite = detail._unitLocalImage;
        if (detail._unitWork)
        {
            ShowChooseToWork(detail._unitWork);
            _zoneWork.text = "Zone: " + detail._zonePos;
            //detail._unitDateTimeStampAssis = DateTime.Parse(PlayerPrefs.GetString("date time" + detail._unitName, detail._unitDateTimeStampAssis.ToString("yyyy/MM/dd HH:mm:ss")));
            //setTimeNowStakingLand(detail._unitDateTimeStampAssis, DateTime.Now, _assistantsDataDetail);
        }
    }
    private void Update()
    {
        /*if (isCheckWork)
        {
            //_assistantsDataDetail._unitTimeAssis += Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(_assistantsDataDetail._unitTimeAssis);
            _timeWork.text = ((timeSpan.Days * 24) + timeSpan.Hours).ToString() + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        }
        else
        {
            //_assistantsDataDetail._unitTimeAssis = 0f;
            _timeWork.text = "00:00:00";
        }*/
    }
    public void onClickChooseToWork()
    {
        if (MovementController.instance._assisObj_agent.Count == 4)
        {
            AssistantsLayerController.instance.warningPanel_obj.gameObject.SetActive(true);
            AssistantsLayerController.instance.warningPanel_obj._innfo_txt.text = "This zone is Maximum 4/4 Assistants work!";
            return;
        }
        SoundListObject.instance.OnclickSFX(0);
        setTimeStartStakingLand(DateTime.Now, _assistantsDataDetail);
        setAssissDetailList(true, _assistantsDataDetail, PlayerObject.instance._zone);
        StartCoroutine(MovementController.instance.setupInstanceAssistants(_assistantsDataDetail));
        if (_assistantsDataDetail._skill != UnitSkill.none)
        {
            IconSkillController.instance.setupActivedSkillIcon(_assistantsDataDetail);
        }
        //TODO: call back to api Choose work
        StartCoroutine(setAssistantsToWork(_assistantsDataDetail._unitTokenID, ZoneTypeEnumToString(PlayerObject.instance._zone)));
        AssistantsSkillActived.instance.AssistantChoseWorkThisZone_ac?.Invoke();
    }
    public void onClickCallBack()
    {
        SoundListObject.instance.OnclickSFX(0);
        if (_assistantsDataDetail._zonePos != PlayerObject.instance._zone)
        {
            AssistantsLayerController.instance.warningPanel_obj.gameObject.SetActive(true);
            AssistantsLayerController.instance.warningPanel_obj._innfo_txt.text = "Please go to that zone " + _assistantsDataDetail._zonePos.ToString() + " to callback !";
            return;
        }
        setAssissDetailList(false, _assistantsDataDetail, PlayerObject.instance._zone);
        MovementController.instance.clearDataAssistant(_assistantsDataDetail);
        if (_assistantsDataDetail._skill != UnitSkill.none)
        {
            IconSkillController.instance.setupUnActivedSkillIcon(_assistantsDataDetail);
        }
        //TODO: call back to api call back work
        StartCoroutine(setAssistantsToWork(_assistantsDataDetail._unitTokenID, ""));
        AssistantsSkillActived.instance.AssistantChoseWorkThisZone_ac?.Invoke();

    }
    IEnumerator setAssistantsToWork(int tokenID, string ZoneID)
    {
        IWSResponse response = null;
        yield return Assistant.SetAssistantArea(XCoreManager.instance.mXCoreInstance, tokenID, ZoneID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
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
    public void setAssissDetailList(bool checkCharacterWork, AssisstantDetail unitDetail, ZoneType zone)
    {
        isCheckWork = checkCharacterWork;
        if (checkCharacterWork)
        {
            ShowChooseToWork(checkCharacterWork);
            unitDetail._unitWork = checkCharacterWork;
            unitDetail._zonePos = PlayerObject.instance._zone;
            _zoneWork.text = "Zone: " + zone.ToString();
            AssistantsLayerController.instance._assisDiapalyList.Add(unitDetail);
            for (int z = 0; z < ZoneUnitObject.instance.unitDataZones.Count; z++)
            {
                if (ZoneUnitObject.instance.unitDataZones[z].ZoneType == zone)
                {
                    ZoneUnitObject.instance.unitDataZones[z]._assisstantDetailThisZone.Add(unitDetail);
                    AutoCalculateHaver.instance.setupAssistantAutoDispalyList(unitDetail);
                }
            }

        }
        else 
        {
            ShowChooseToWork(checkCharacterWork);
            unitDetail._unitWork = checkCharacterWork;
            unitDetail._zonePos = ZoneType.None;
            unitDetail.resetDateTime();
            unitDetail.DeletePlayerprefsAss("date time" + unitDetail._unitName);
            for (int i = 0; i < AssistantsLayerController.instance._assisDiapalyList.Count; i++)
            {
                if (AssistantsLayerController.instance._assisDiapalyList[i]._unitTokenID == unitDetail._unitTokenID)
                {
                    AssistantsLayerController.instance._assisDiapalyList.Remove(unitDetail);
                }
            }
            for (int z = 0; z < ZoneUnitObject.instance.unitDataZones.Count; z++)
            {
                if (ZoneUnitObject.instance.unitDataZones[z].ZoneType == zone)
                {
                    ZoneUnitObject.instance.unitDataZones[z]._assisstantDetailThisZone.Remove(unitDetail);
                    AutoCalculateHaver.instance.DeleteAssistantAutoDispalyList(unitDetail);
                }
            }
            for (int i = 0; i < MovementController.instance._assisObj_agent.Count; i++)
            {
                if (MovementController.instance._assisObj_agent[i].agentDetail._unitTokenID == unitDetail._unitTokenID)
                {
                    MovementController.instance._assisObj_agent.Remove(MovementController.instance._assisObj_agent[i]);
                }
            }
        }
    }
    public void ShowChooseToWork(bool check)
    {
        isCheckWork = check;
        _chooseToWork.SetActive(!check);
        _workObj.SetActive(check);
        _callBack.SetActive(check);
    }
    public void onclickClose()
    {
        SoundListObject.instance.OnclickSFX(0);
        this.gameObject.SetActive(false);
    }
    #region DATA TIME
    public void setTimeNowStakingLand(DateTime timeOld, DateTime timeNow, AssisstantDetail detail)
    {
        TimeSpan timeSpan = timeNow - timeOld;
        detail._unitTimeAssis = timeSpan.TotalSeconds;
        //Debug.Log("Pase to double: " + detail._unitTimeAssis);
    }
    //Use to set time in start plante
    public void setTimeStartStakingLand(DateTime timeNow, AssisstantDetail detail)
    {
        TimeSpan timeSpan = TimeSpan.Zero;
        detail._unitTimeAssis = timeSpan.TotalSeconds;
        detail._unitDateTimeStampAssis = timeNow;
        //set playerprefs to date time assisstant
        PlayerPrefs.SetString("date time" + detail._unitName, detail._unitDateTimeStampAssis.ToString("yyyy/MM/dd HH:mm:ss"));
        //Debug.Log(PlayerPrefs.GetString("date time"));
        //Debug.Log("Stamp time: " + detail._unitDateTimeStampAssis);
    }
    #endregion
}
