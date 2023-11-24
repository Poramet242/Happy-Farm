using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using XSystem;

public class Login : MonoBehaviour
{
    private string secen = "Garage_zone";
    private string secenTutorialGameplay = "TutorialGameplay";
    [SerializeField] private bool isDemo;
    private bool checkDataDemo;
    [SerializeField] private LoginObject_Ctr loginObject_Ctr;
    [Header("CheckData")]
    [SerializeField] private bool checkGetAssistants;
    [SerializeField] private GameObject _facebook_btn;
    [SerializeField] private GameObject _google_btn;
    [SerializeField] private GameObject _apple_btn;
    [SerializeField] private GameObject _email_btn;
    private void Awake()
    {
        if (isDemo)
        {
            loginObject_Ctr.showButtonLogin();
            //StartCoroutine(LoadDataLocalFile());
        }
        else
        {
            StartCoroutine(XLogin());
            StartCoroutine(setDateTime());
        }
    }
    private void Start()
    {
        StartCoroutine(setsoung());
#if UNITY_ANDROID && !UNTYI_EDITOR
        _apple_btn.SetActive(false);
#endif
    }
    IEnumerator setsoung()
    {
        SoundManager.instance.PlayBGM(SoundListObject.instance._BGMALL[0]);
        yield break;
    }
    public void OnClickLogin(LoginTypes loginTypes)
    {
        switch (loginTypes)
        {
            case LoginTypes.Facebook:
                //TODO: get token id in facebook
                loginObject_Ctr.onClickFacebook();
                break;
            case LoginTypes.Google:
                //TODO: get token id in Google
                loginObject_Ctr.onclickLoginGoole();
                break;
            case LoginTypes.AppleID:
                //TODO: get token id in AppleID
                loginObject_Ctr.onClickAppleLogin();
                break;
            case LoginTypes.Email:
                loginObject_Ctr.showLogin(true);
                break;
        }
        /*if (isDemo)
        {
            StartCoroutine(LoadScene());
        }*/
    }
    IEnumerator XLogin()
    {
        Debug.Log("Start XLogin");
        IWSResponse response = null;
        if (PlayerPrefs.HasKey("sessionToken"))
        {
            string sessionToken = PlayerPrefs.GetString("sessionToken");
            Debug.Log("Token: " + sessionToken);
            yield return XUser.RestoreSession(XCoreManager.instance.mXCoreInstance, sessionToken,
                (r) =>
                {
                    response = r;
                });
            if (response.Success() == false)
            {
                if (!response.Success())
                {
                    Debug.LogError(response.ErrorsString());
                    yield break;
                }
                Debug.Log("XLogin Success");
                loginObject_Ctr.showButtonLogin();
            }
            loginObject_Ctr.showPlayGame();
        }
        else
        {
            Debug.Log("XLogin Success");
            loginObject_Ctr.showButtonLogin();
        }
    }
    public void onClickGameplay()
    {
        StartCoroutine(CheckChangeScene());
    }
    IEnumerator CheckChangeScene()
    {
        loginObject_Ctr._callplanelSysyem();
        yield return LoadDataGameplayProfile();
        yield return LoadDataPlant();
        yield return LoadFriendData();
        yield return LoadDataAssistants();
        yield return LoadDataFurniture();
        //Assisstents();
        loginObject_Ctr._panelSystem.SetActive(false);
        SoundManager.instance.StopBGM();
        //TODO: go to secen toturialSecen
        if (PlayerObject.instance._checkplayTutorial)
        {
            SceneManager.LoadScene(secen);
        }
        else
        {
            SceneManager.LoadScene(secenTutorialGameplay);
        }
        //SceneManager.LoadScene(secen);
    }
#region load Data player profile
    IEnumerator LoadDataGameplayProfile()
    {
        Debug.Log("Load Data Gameplay...");
        IWSResponse response = null;
        yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        var user = response as Account;
        //Info profile
        PlayerObject.instance._playerName = user.displayName;
        PlayerObject.instance._imagesID = user.displayImageID;
        PlayerObject.instance._checkplayTutorial = user.tutorialPlayed;

        if (PlayerObject.instance._imagesID == null || PlayerObject.instance._imagesID == string.Empty || PlayerObject.instance._imagesID == "000000000000000000000000") 
        {
            yield return setAvatardefault("Avatardefault");
            yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                Debug.Log("Error GetUserProfile");
                yield break;
            }
            var userTemp = response as Account;
            PlayerObject.instance._imagesID = userTemp.displayImageID;
            PlayerObject.instance._imagesProfile_spr = UnitDataLoader.Instance.GetLocalIcon(userTemp.displayImageID);
            setUpAccount(userTemp);
        }
        else
        {
            PlayerObject.instance._imagesProfile_spr = UnitDataLoader.Instance.GetLocalIcon(user.displayImageID);
            setUpAccount(user);
        }
        yield return NextLevelExp.GetNextLevelExp(XCoreManager.instance.mXCoreInstance, user.level, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var levelData = response as NextLevelExp;
        PlayerObject.instance.max_levelPlayer = levelData.nextLevelExp;
        PlayerObject.instance.min_levelPlayer = levelData.currentLevelExp;

        foreach (Sprite item in Resources.LoadAll<Sprite>("Icon"))
        {
            PlayerObject.instance._allLocalImages.Add(item);
        }
        //---------------------------TODO: Deleted this to play-------------------------------------------------------
        /*yield return Account.SetTutorialPlayed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        var usered = response as Account;
        PlayerObject.instance._checkplayTutorial = usered.tutorialPlayed;*/
        //------------------------------------------------------------------------------------------------------------
    }
    public void setUpAccount(Account user)
    {
        PlayerObject.instance._address = user.accountAddress;
        PlayerObject.instance._accNo = user.userID;
        PlayerObject.instance._accType = user.accountType;
        //---------------------------------------------------
        PlayerObject.instance._zone = ZoneType.Garage;
        PlayerObject.instance._checkEditName = user.nameLocked;
        //exp
        PlayerObject.instance._levelPlayer = user.level;
        PlayerObject.instance.current_levelPlayer = user.exp;
        //Coine
        PlayerObject.instance._tokenNFTReward = user.gem;
        PlayerObject.instance._coineReward = user.coin;
    }
    IEnumerator setAvatardefault(string ImagesID)
    {
        IWSResponse response = null;
        yield return Account.setProfileImage(XCoreManager.instance.mXCoreInstance, ImagesID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }

    }
#endregion

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
                    Selldata.unitData._currentCountPlantGold = (Selldata.unitData._maxCountPlantGold - SeedCurrendcount.count);
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
    public int SplitingToint(string str,string fixer)
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
        PlayerObject.instance.IsNowServerDateTime = time.timeNow;
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

#region load Data friend
    IEnumerator LoadFriendData()
    {
        Debug.Log("Load Friend Data...");
        IWSResponse response = null;
        yield return FriendAPI.GetFriendList(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        List<Account> accountsFriend = Account.ParseToList(response.RawResult().ToString());
        if (accountsFriend.Count == 0)
        {
            yield break;
        }
        for (int i = 0; i < accountsFriend.Count; i++)
        {
            FriendDetail friendDetail = ScriptableObject.CreateInstance<FriendDetail>();
            friendDetail.name = accountsFriend[i].displayName;
            friendDetail.playerTokenID = accountsFriend[i].userID;
            friendDetail.playerName = accountsFriend[i].displayName;
            friendDetail.playerURLImage = accountsFriend[i].displayImageID;
            if (friendDetail.playerURLImage == null || friendDetail.playerURLImage == string.Empty)
            {
                friendDetail.playerLocalImage = UnitDataLoader.Instance.GetLocalIcon("Avatardefault");
                setupFriendAccount(accountsFriend[i], friendDetail);
            }
            else
            {
                friendDetail.playerLocalImage = UnitDataLoader.Instance.GetLocalIcon(accountsFriend[i].displayImageID);
                setupFriendAccount(accountsFriend[i], friendDetail);
            }
            FriendObject.instance._allFriendslist.Add(friendDetail);
        }

    }
    public void setupFriendAccount(Account accountsFriend,FriendDetail friendDetail)
    {
        friendDetail.playerLevel = accountsFriend.level;
        friendDetail.countCoin = accountsFriend.coin;
        friendDetail.countCoinNFT = accountsFriend.gem;
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
    public void Assisstents()
    {
        foreach (AssisstantDetail item in Resources.LoadAll<AssisstantDetail>("Data/CharacterFarmer"))
        {
            item._unitTimeAssis = 0f;
            //item._unitDisplay = item._unitLocalImage.name;
            AssistantsObject.instance._assistantsAllList.Add(item);
        }
        /*foreach (FriendDetail item in Resources.LoadAll<FriendDetail>("Data/Friend"))
        {
            AlphaGamePlayDemo._friendDetails.Add(item);
        }
        foreach (FriendDetail item in AlphaGamePlayDemo._friendDetails)
        {
            FriendObject.instance._allFriendslist.Add(item);
        }*/
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

#region load Data Furniture
    IEnumerator LoadDataFurniture()
    {
        IWSResponse response = null;
        yield return Decoration.GetDecorationItems(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }

        List<Decoration> Decoreationlist = Decoration.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < Decoreationlist.Count; i++)
        {
            FurnitureDetail furnitureDetails = ScriptableObject.CreateInstance<FurnitureDetail>();
            furnitureDetails.isBuyFurniture = Decoreationlist[i].isUnlocked;
            furnitureDetails.isUseFurniture = Decoreationlist[i].isUsed;
            furnitureDetails.name = Decoreationlist[i].itemID;
            furnitureDetails.unitID = Decoreationlist[i].itemID;
            furnitureDetails.unitImagesID = Decoreationlist[i].imageID;
            furnitureDetails.unitPrice = Decoreationlist[i].price;
            //furnitureDetails.unitPrice_NFT = int.Parse(Decoreationlist[i].priceCurrency);
            furnitureDetails.rarityType = Decoreationlist[i].rarityType;
            furnitureDetails.furnitureType = Decoreationlist[i].itemType;
            furnitureDetails.localImages = UnitDataLoader.Instance.GetLocalImagesFurniture(Decoreationlist[i].itemType, Decoreationlist[i].itemID);

            FurnitureUnitObject.instance.all_furnitureDetails.Add(furnitureDetails);
            FurnitureUnitObject.instance._allDecorationsDataList.Add(Decoreationlist[i]);
        }
    }
#endregion

#region alpha gameplay data demo dont callBackend
    /*IEnumerator LoadScene()
    {
        loginObject_Ctr._panelSystem.SetActive(true);
        while (!checkDataDemo)
        {
            yield return null;
        }
        loginObject_Ctr._panelSystem.SetActive(false);
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene(secen);
    }
    IEnumerator LoadDataLocalFile()
    {
        if (AlphaGamePlayDemo.CheckData)
        {
            LoadUnitLocalFile();
            yield break;
        }
        foreach (UnitDetail item in Resources.LoadAll<UnitDetail>("Data/Cannabis/Detail"))
        {
            AlphaGamePlayDemo._allUnitDetailList.Add(item);
        }
        foreach (UnitData item in Resources.LoadAll<UnitData>("Data/Cannabis/Data"))
        {
            AlphaGamePlayDemo._allUnitDataList.Add(item);
        }
        foreach (AssisstantDetail item in Resources.LoadAll<AssisstantDetail>("Data/CharacterFarmer"))
        {
            AlphaGamePlayDemo._assistantsAllList.Add(item);
        }
        foreach (FriendDetail item in Resources.LoadAll<FriendDetail>("Data/Friend"))
        {
            AlphaGamePlayDemo._friendDetails.Add(item);
        }
        AlphaGamePlayDemo.CheckData = true;
        LoadUnitLocalFile();
        yield break;
    }
    public void LoadUnitLocalFile()
    {
        foreach (UnitDetail item in AlphaGamePlayDemo._allUnitDetailList)
        {
            StakeUnitObject.instance._allUnitDetailList.Add(item);
        }
        foreach (UnitData item in AlphaGamePlayDemo._allUnitDataList)
        {
            StakeUnitObject.instance._allUnitDataList.Add(item);
        }
        foreach (AssisstantDetail item in AlphaGamePlayDemo._assistantsAllList)
        {
            item._unitTimeAssis = 0f;
            AssistantsObject.instance._assistantsAllList.Add(item);
        }
        foreach (FriendDetail item in AlphaGamePlayDemo._friendDetails)
        {
            PlayerObject.instance._allFriendslist.Add(item);
        }
        setAssisstantInZone(AssistantsObject.instance._assistantsAllList);
        setCannabisInZone(setCharacterDataList(StakeUnitObject.instance._allUnitDetailList, StakeUnitObject.instance._allUnitDataList));
        setCountSlotInZone(0);
        checkDataDemo = true;
    }*/
    public List<CharacterData> setCharacterDataList(List<UnitDetail> unitDetails, List<UnitData> unitDatas)
    {
        List<CharacterData> data = new List<CharacterData>();
        for (int i = 0; i < unitDetails.Count; i++)
        {
            for (int j = 0; j < unitDatas.Count; j++)
            {
                if (unitDetails[i]._unitTokenID == unitDatas[j]._unitTokenID) 
                {
                    if (isDemo)
                    {
                        CharacterData newdata = new CharacterData();
                        newdata.detail = unitDetails[i];
                        newdata.unitData = unitDatas[j];
                        if (newdata.detail._unitStaking)
                        {
                            data.Add(newdata);
                        }
                        else
                        {
                            newdata.detail._growthPlante = GrowthPlante.None;
                            newdata.detail._zonePos = ZoneType.None;
                            newdata.unitData._unitStackCoine = 0;
                            newdata.unitData._unitCountTime = 0;
                            newdata.unitData._unitDateTimeStamp = new DateTime();
                            data.Add(newdata);
                        }
                    }
                    else
                    {
                        CharacterData newdata = new CharacterData();
                        newdata.detail = unitDetails[i];
                        newdata.unitData = unitDatas[j];
                        data.Add(newdata);
                    }
                    break;
                }
            }
        }
        return data;
    }
    public void setCannabisInZone(List<CharacterData> characterDatas)
    {
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (characterDatas[i].detail._unitStaking)
            {
                for (int j = 0; j < ZoneUnitObject.instance.unitDataZones.Count; j++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[j].ZoneType == characterDatas[i].detail._zonePos)
                    {
                        ZoneUnitObject.instance.unitDataZones[j]._cannaBisDatasThisZone.Add(characterDatas[i]);
                    }
                }
            }
        }
    }
    public void setCountSlotInZone(int count)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot = count;
        }
    }
#endregion
}
