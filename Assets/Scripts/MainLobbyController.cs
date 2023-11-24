using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XSystem;

public class MainLobbyController : MonoBehaviour
{
    public static MainLobbyController instance;
    [Header("Zone")]
    [SerializeField] public ZoneType zone;
    [Header("UI Layer Object")]
    [SerializeField] public GameObject m_shopping;
    [SerializeField] public GameObject m_inventory;
    [SerializeField] public GameObject m_assistants;
    [SerializeField] public GameObject m_friend;
    [SerializeField] public GameObject m_zone;
    [SerializeField] public GameObject m_spin;
    [SerializeField] public GameObject m_recruitAssistant;

    [Header("Load Object")]
    [SerializeField] public GameObject m_profile;
    [SerializeField] public GameObject m_setting;
    [SerializeField] public GameObject m_CalculateTimeObject;

    [Header("PanelSystemcall")]
    [SerializeField] public GameObject PanelSystemcall;
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        m_setting = Resources.Load("UILayer/SettingLayer", typeof(GameObject)) as GameObject;
        m_profile = Resources.Load("UILayer/ProfileLayer", typeof(GameObject)) as GameObject;
        m_CalculateTimeObject = Resources.Load("UILayer/CalculateTimeGameplay", typeof(GameObject)) as GameObject;

        //StartCoroutine(LoadDataLocalFile());
    }
    private void Start()
    {
        HideLayerOnload(true);
        PanelSystemcall.SetActive(true);
        StartMainLobby();
        StartCoroutine(AlphaInitialize());
    }
    public void StartMainLobby()
    {
        DataSoundHolder.SFX_Volume = PlayerPrefs.GetFloat("SFX_Volume", 1.0f);
        DataSoundHolder.BGM_Volume = PlayerPrefs.GetFloat("BGM_Volume", 1.0f);
        DataSoundHolder.LanguageSetting = PlayerPrefs.GetInt("LanguageSetting", 0);
        DataSoundHolder.animationSetting = PlayerPrefs.GetInt("AnimationSetting", 0);

        SoundManager.instance.PlayBGM(SoundListObject.instance._BGMALL[1]);
        //Setting----------------------------------------------------------------------
        if (GameObject.Find("SettingLayer(Clone)") == null)
        {
            m_setting = Instantiate(m_setting, Vector3.zero, Quaternion.identity);
        }
        else
        {
            m_setting = GameObject.Find("SettingLayer(Clone)");
        }
        //-----------------------------------------------------------------------------
        //Profile----------------------------------------------------------------------
        if (GameObject.Find("ProfileLayer(Clone)") == null)
        {
            m_profile = Instantiate(m_profile, Vector3.zero, Quaternion.identity);
        }
        else
        {
            m_profile = GameObject.Find("ProfileLayer(Clone)");
        }
        //-----------------------------------------------------------------------------
        //CalculateTimeGameplay--------------------------------------------------------
        if (GameObject.Find("CalculateTimeGameplay(Clone)") == null)
        {
            m_CalculateTimeObject = Instantiate(m_CalculateTimeObject, Vector3.zero, Quaternion.identity);
        }
        else
        {
            m_CalculateTimeObject = GameObject.Find("CalculateTimeGameplay(Clone)");
        }
        //-----------------------------------------------------------------------------
    }

    IEnumerator AlphaInitialize()
    {
        if (ZoneUnitObject.instance._checkNoneCalBackData)
        {
            yield return setDateTime();
            yield return LoadDataPlant();
            yield return LoadDataAssistants();
            PanelSystemcall.SetActive(false);
        }
        else
        {
            PanelSystemcall.SetActive(false);
        }
        IconSkillController.instance.Initialize();
        AssistantsLayerController.instance.Initialize();
        StakeLayerController.instance.Initialize();
        ProfileLayerController.instance.Initialize();
        AssistantsSkillActived.instance.Initialize();
        SettingController.instance.facetoPlayGame(true, true);
        HideLayerOnload(false);
        m_profile.SetActive(true);
        m_setting.SetActive(true);
        m_setting.SetActive(false);
        yield break;
    }

    public void HideLayerOnload(bool check)
    {
        m_shopping.SetActive(check);
        m_inventory.SetActive(check);
        m_assistants.SetActive(check);
        m_friend.SetActive(check);
        m_zone.SetActive(check);
        m_spin.SetActive(check);
        m_profile.SetActive(check);
        //m_recruitAssistant.SetActive(check);
        if (!check)
        {
            m_setting.SetActive(check);
        }
    }
    //TODO: call in end play tutoraialplayed
    IEnumerator setTutorialplayed()
    {
       IWSResponse response = null;
       yield return Account.SetTutorialPlayed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
       if (!response.Success())
       {
           Debug.LogError(response.ErrorsString());
           yield break;
       }
    }
    #region load Data plant
    IEnumerator LoadDataPlant()
    {
        Debug.Log("Load Data plant....");
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
            if (plantInfos[i].canBuy)
            {
                UnitDetail unitDetail = ScriptableObject.CreateInstance<UnitDetail>();
                UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
                unitDetail._unitTokenID = plantInfos[i].plantID;
                unitData._unitTokenID = plantInfos[i].plantID;
                //unitData._currentCountPlant = plantInfos[i]
                //unitData._maxCountPlant = plantInfos[i]
                StakeUnitObject.instance.SetUpDataPlantinfo(plantInfos[i], unitDetail, unitData);
                CharacterData Selldata = new CharacterData();
                Selldata.detail = unitDetail;
                Selldata.unitData = unitData;
                StakeUnitObject.instance._allSellUnitDataList.Add(Selldata);
                yield return SeedBuyCount.GetSeedBuyCount(XCoreManager.instance.mXCoreInstance, Selldata.detail._unitTokenID, (r) => response = r);
                if (!response.Success())
                {
                    Debug.LogError(response.ErrorsString());
                    yield break;
                }
                var SeedCurrendcount = response as SeedBuyCount;
                if (Selldata.unitData._maxCountPlantGold == -1)
                {
                    Selldata.unitData._currentCountPlantGold = 999999;
                }
                else
                {
                    Selldata.unitData._currentCountPlantGold = SeedCurrendcount.count;
                }
                Debug.Log(unitData.name + unitData._unitCanBuy);
            }
            StakeUnitObject.instance._allPlantInfoDataList.Add(plantInfos[i]);
        }
        Debug.Log("plantInfos: " + plantInfos.Count);
        GameData.instance.InitPlantInfo(plantInfos);

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
            //Debug.Log(unitData.name + unitData._unitCanBuy);
        }
        //Get block and pos plant
        yield return UserBlock.GetUserBlock(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        List<UserBlock> userBlocksList = UserBlock.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userBlocksList.Count; i++)
        {
            //set block and plant in zone
            setPlantandBlockInZone(userBlocksList[i], SplitingToint(userBlocksList[i].blockID, "block"));
        }
        //Get zone in gameplay
        yield return UserArea.GetUserArea(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        List<UserArea> userAreasList = UserArea.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userAreasList.Count; i++)
        {
            setZoneGameplay(userAreasList[i], SplitingToint(userAreasList[i].areaID, "zone"));
        }
        //get unit time plant
        yield return UserPlant.GetAllPlantProgress(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        List<UserPlant> userPlantsList = UserPlant.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < userPlantsList.Count; i++)
        {
            setDatatimePlantes(userPlantsList[i]);
        }
        yield return BlockInfo.GetBlockInfo(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        List<BlockInfo> BlockInfos = BlockInfo.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < BlockInfos.Count; i++)
        {
            StakeUnitObject.instance._allBlockInfoDataList.Add(BlockInfos[i]);
        }

    }
    public int SplitingToint(string str, string fixer)
    {
        string input = str;
        string prefix = fixer;
        string numPart = input.Substring(prefix.Length);
        int num;
        int.TryParse(numPart, out num);
        return num;
    }
    public void setDatatimePlantes(UserPlant userPlant)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; z++)
            {
                if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCurrentPlant == userPlant.id)
                {
                    ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitDateTimeStamp = userPlant.plantedTime;
                    ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime = setTimeNowStaking(userPlant.plantedTime, DateTime.Now);
                    Debug.Log("Stack Coine: " + userPlant.pendingReward);
                    if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].detail._planeNFT)
                    {
                        ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine = setNowStack(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant.pendingRewardNFT);
                        if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine >= ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitMaxStackCoine)
                        {
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine = ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitMaxStackCoine;
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin * userPlant.pendingReward) + ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin;
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                        else
                        {
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = setTimeNowStaking(userPlant.timeStamp, DateTime.Now);
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                    }
                    else
                    {
                        ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine = setNowStack(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant.pendingReward);
                        if (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitStackCoine >= ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitMaxStackCoine)
                        {
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = (ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin * userPlant.pendingReward) + ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin;
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                        else
                        {
                            double x = setTimeNowStaking(userPlant.timeStamp, DateTime.Now);
                            ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTimeHaver = x % ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData.timePerCoin;
                            setupGrowTime(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z].unitData._unitCountTime, ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[z], userPlant);
                        }
                    }
                }
            }
        }
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
    public int setNowStack(CharacterData data, int pandingRew)
    {
        int nowStack = pandingRew / data.unitData._unitCoineMax;
        return nowStack;
    }
    public double setTimeNowStaking(DateTime timeOld, DateTime timeNow)
    {
        double numDouble = 0;
        TimeSpan timeSpan = timeNow - timeOld;
        numDouble = timeSpan.TotalSeconds + PlayerObject.instance.dateTimeServer;
        Debug.Log("Pase to double: " + numDouble);
        return numDouble;
    }
    IEnumerator setDateTime()
    {
        IWSResponse response = null;
        yield return TimeNow.GetTimeNow(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var time = response as TimeNow;
        PlayerObject.instance.dateTimeServer = (int)(time.timeNow - DateTime.Now).TotalSeconds;
        Debug.Log("Pase to double: " + PlayerObject.instance.dateTimeServer);
    }
    public void setZoneGameplay(UserArea userAream, int num)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if ((int)ZoneUnitObject.instance.unitDataZones[i].ZoneType == num)
            {
                ZoneUnitObject.instance.unitDataZones[i]._checkUnlock = true;
            }
        }
    }
    public void setPlantandBlockInZone(UserBlock userBlock, int num)
    {
        Debug.Log(userBlock.area);
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (userBlock.area == ZoneUnitObject.instance.unitDataZones[i].ZoneType)
            {
                Debug.Log(userBlock.area);
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone.Count; z++)
                {
                    if (z == (num - 1))
                    {
                        Debug.Log(num);
                        ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone[z] = true;
                        ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot += 1;
                    }
                }
            }
        }
        if (userBlock.isPlanted)
        {
            UnitDetail blockDetail = ScriptableObject.CreateInstance<UnitDetail>();
            UnitData blockData = ScriptableObject.CreateInstance<UnitData>();
            //set plant ID
            blockDetail._unitTokenID = userBlock.currentPlantID;
            blockData._unitTokenID = userBlock.currentPlantID;
            blockData._unitCurrentPlant = userBlock.currentPlant;
            blockDetail._unitCurrentPlant = userBlock.currentPlant;
            var plant = GameData.instance.GetPlantInfoByPlantID(userBlock.currentPlantID);
            StakeUnitObject.instance.SetUpDataPlantinfo(plant, blockDetail, blockData);
            blockDetail._unitPos = num - 1;
            blockDetail._unitStaking = true;
            blockDetail._zonePos = userBlock.area;
            CharacterData data = new CharacterData();
            data.detail = blockDetail;
            data.unitData = blockData;
            for (int j = 0; j < ZoneUnitObject.instance.unitDataZones.Count; j++)
            {
                if (ZoneUnitObject.instance.unitDataZones[j].ZoneType == blockDetail._zonePos)
                {
                    ZoneUnitObject.instance.unitDataZones[j]._cannaBisDatasThisZone.Add(data);
                }
            }
        }
    }
    #endregion

    #region load Data Assistants
    IEnumerator LoadDataAssistants()
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
            AssisstantDetail assisDetail = ScriptableObject.CreateInstance<AssisstantDetail>();
            assisDetail._unitTokenID = assistant_data[i].tokenID;
            assisDetail._unitName = assistant_data[i].name;
            assisDetail.name = assistant_data[i].name;
            assisDetail._skill = assistant_data[i].skillType;
            //assisDetail._unitDisplay = "";
            assisDetail._unitImageID = assistant_data[i].imageID;
            //assisDetail._unitDateTimeStampAssis == assistant_data[i].timeStamp;
            assisDetail._zonePos = assistant_data[i].area;
            if (assisDetail._zonePos == ZoneType.None)
            {
                assisDetail._unitWork = false;
            }
            else
            {
                assisDetail._unitWork = true;
            }
            assisDetail._unitLocalImage = UnitDataLoader.Instance.GetLocalImagesAssistan(assistant_data[i].imageID);
            assisDetail._rarityType = assistant_data[i].rarity;
            assisDetail._unitPrefab = UnitDataLoader.Instance.GetLocalGameObjectAssistan(assistant_data[i].name);

            AssistantsObject.instance._assistantsAllList.Add(assisDetail);
        }
        setAssisstantInZone(AssistantsObject.instance._assistantsAllList);
    }
    public void setAssisstantInZone(List<AssisstantDetail> assisstantDetails)
    {
        for (int i = 0; i < assisstantDetails.Count; i++)
        {
            if (assisstantDetails[i]._unitWork)
            {
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones.Count; z++)
                {
                    if (assisstantDetails[i]._zonePos == ZoneUnitObject.instance.unitDataZones[z].ZoneType)
                    {
                        ZoneUnitObject.instance.unitDataZones[z]._assisstantDetailThisZone.Add(assisstantDetails[i]);
                    }
                }
            }
        }
    }
    #endregion
}
