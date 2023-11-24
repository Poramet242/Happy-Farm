using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XSystem;

public class StakeLayerController : MonoBehaviour
{
    public static StakeLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public bool isCheckStartZone;
    [SerializeField] public TeamSlotController tsc;
    [SerializeField] public ZoneType zone;
    [Header("Object")]
    [SerializeField] public GameObject _inventoryObj;
    [SerializeField] public SpriteRenderer _background;
    [SerializeField] public List<CharacterData> _plantDisplayStake = new List<CharacterData>();
    [Header("Adtibuild")]
    [SerializeField] private int indexSlot;
    [SerializeField] public GameObject infoBuyslot;
    [SerializeField] public GameObject infoBuyslotIAP;
    [SerializeField] public GameObject infoBuyslot_tutorial;
    [SerializeField] public UnlockSlotReward RewardUi;
    [SerializeField] public GameObject DeleteUi;
    [SerializeField] private Slot_IAPManager slot;
    [Header("Poppup")]
    [SerializeField] public List<GameObject> poppup_gameList = new List<GameObject>();
    [Header("Auto Display")]
    [SerializeField] public int countIsOpenAuto;
    private bool hasOpenAuto = false;
    [SerializeField] public List<GameObject> unitslot_gameObjec = new List<GameObject>();
    public void Initialize()
    {
        isCheckStartZone = true;
        PlayerObject.instance._zone = zone;
        StakeUnitObject.instance.zone = zone;
        updateCountZoneunlock();
        setPlaneZoneDisplay(getDataPlanetodisplay(ZoneUnitObject.instance.unitDataZones));
        //setUpZonePoppupDisplay(FurnitureUnitObject.instance.all_furnitureDetails);
    }
    public void setUpZonePoppupDisplay(List<FurnitureDetail> furnitureDetails)
    {
        for (int i = 0; i < furnitureDetails.Count; i++)
        {
            if (furnitureDetails[i].isUseFurniture)
            {
                setfurnitureInGamePlay(FurnitureUnitObject.instance.all_furnitureDetails[i]);
            }
        }
    }
    public void setfurnitureInGamePlay(FurnitureDetail detail)
    {
        for (int i = 0; i < poppup_gameList.Count; i++)
        {
            if (poppup_gameList[i].GetComponent<FurnitureController>().furniture == detail.furnitureType)
            {
                poppup_gameList[i].GetComponent<SpriteRenderer>().sprite = detail.localImages;
            }
        }
    }
    public void setPlaneZoneDisplay(List<CharacterData> characterDatas)
    {
        if (characterDatas != null && characterDatas.Count != 0)
        {
            tsc.Setup(characterDatas);
            tsc.unlockMutipleSlots(characterDatas, setCountUnlockZone());
        }
        else
        {
            tsc.Setup(null);
            tsc.unlockMutipleSlots(null, setCountUnlockZone());
        }
        isCheckStartZone = false;
    }
    public int setCountUnlockZone()
    {
        int count = 0;
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == zone)
            {
                count = ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot;
            }
        }
        return count;
    }
    private void Update()
    {
        if (!hasOpenAuto)
        {
            setupBlockOpenAuto(AssistantsLayerController.instance.countAuto);
            hasOpenAuto = true;
        }
    }
    public void setupBlockOpenAuto(int countAuto)
    {
        countIsOpenAuto = countAuto;
        for (int i = 0; i < countAuto; i++)
        {
            for (int a = 0; a < unitslot_gameObjec.Count; a++)
            {
                if (unitslot_gameObjec[a].GetComponent<PlantCalculate>().isAutoHaver)
                {
                    continue;
                }
                else
                {
                    if (unitslot_gameObjec[a].GetComponent<PlantCalculate>().myData.unitData != null)
                    {
                        unitslot_gameObjec[a].GetComponent<PlantCalculate>().isAutoHaver = true;
                        break;
                    }
                }
            }
        }
    }
    public List<CharacterData> getDataPlanetodisplay(List<UnitDataZone> unitDataZones)
    {
        for (int i = 0; i < unitDataZones.Count; i++)
        {
            if (unitDataZones[i].ZoneType == zone)
            {
                _plantDisplayStake = unitDataZones[i]._cannaBisDatasThisZone;
            }
        }
        return _plantDisplayStake;
    }
    public void DeleteDataThisZone(CharacterData Data)
    {
        //ClearDataPlaneLocalFile(Data);
        for (int z = 0; z < ZoneUnitObject.instance.unitDataZones.Count; z++)
        {
            if (ZoneUnitObject.instance.unitDataZones[z].ZoneType == PlayerObject.instance._zone)
            {
                ZoneUnitObject.instance.unitDataZones[z]._cannaBisDatasThisZone.Remove(Data);
            }
        }
    }
    public IEnumerator setDeletePlant(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.RemovePlant(XCoreManager.instance.mXCoreInstance, "zone" + (int)data.detail._zonePos, "block" + (data.detail._unitPos + 1), (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
    }
    public void ClearDataPlaneLocalFile(CharacterData characterData)
    {
        characterData.detail._unitPos = 0;
        characterData.detail._zonePos = ZoneType.None;
        characterData.detail._growthPlante = GrowthPlante.None;
        characterData.detail._unitStaking = false;

        characterData.unitData._unitStackCoine = 0;
        characterData.unitData._unitCountTime = 0f;
        characterData.unitData._unitDateTimeStamp = DateTime.Now;
    }
    public void onclickConfirmDeleteSlot()
    {
        setPlantEffectedToGameplay(tsc.slotsList[indexDeleted].myData, "cut");
        StartCoroutine(setDeletePlant(tsc.slotsList[indexDeleted].myData));
        DeleteDataThisZone(tsc.slotsList[indexDeleted].myData);
        tsc.slotsList[indexDeleted].deleteUnitDataInSlot();
        //reset DateTime in unitData
        tsc.slotSelectedIndex = -1;
        CloseUiLayerGameplay();
        DeleteUi.SetActive(false);
    }
    [SerializeField] private int indexDeleted;
    public void setupDisplayDelete(int indexDelete)
    {
        indexDeleted = indexDelete;
        DeleteUi.SetActive(true);
    }
    public void plant_btn(bool check)
    {
        if (check)
        {
            OpenUiLayerGameplay();
            if (!PlayerObject.instance._checkplayTutorial)
            {
                TutorialGameplay.instance._face_img.gameObject.SetActive(false);
                //TutorialGameplay.instance._Cursor_obj.gameObject.SetActive(false);
            }
        }
        else
        {
            CloseUiLayerGameplay();
        }
        InventoryLayerController.instance.gameObject.SetActive(check);
        InventoryLayerController.instance._sell_btn.SetActive(!check);
        InventoryLayerController.instance._plant_btn.SetActive(check);
        PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(InventoryLayerController.instance._unitinventoryDisplayList[0].GetComponent<PlantsDisplay>().characterDataList);
        InventoryLayerController.instance.SortIconImgByLockStatus();
    }
    public void plantEff(CharacterData data, bool check)
    {
        if (check)
        {
            for (int i = 0; i < tsc.slotsList.Count; i++)
            {
                if (tsc?.slotsList != null)
                {
                    if (tsc.slotsList[i].myData == null || tsc.slotsList[i].myData.unitData == null || tsc.slotsList[i].myData.detail == null)
                    {
                        continue;
                    }
                    // do something with tsc.slotsList
                    if (tsc.slotsList[i].myData.unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
                    {
                        tsc.slotsList[i].GetComponent<RewardAnimation>()._plant_Eff.gameObject.SetActive(true);
                        tsc.slotsList[i].GetComponent<RewardAnimation>()._plant_Eff.Play();
                        break;
                    }
                }
            }

        }
        else
        {
            for (int i = 0; i < tsc.slotsList.Count; i++)
            {
                if (tsc?.slotsList != null)
                {
                    if (tsc.slotsList[i].myData == null || tsc.slotsList[i].myData.unitData == null || tsc.slotsList[i].myData.detail == null)
                    {
                        continue;
                    }
                    if (tsc.slotsList[i].myData.unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
                    {
                        tsc.slotsList[i].GetComponent<RewardAnimation>().setUpParticle(data);
                        break;
                    }
                }
            }
        }
    }
    public void setPlantEffectedToGameplay(CharacterData data,string nameAnim)
    {
        for (int i = 0; i < tsc.slotsList.Count; i++)
        {
            if (tsc?.slotsList != null)
            {
                if (tsc.slotsList[i].myData == null || tsc.slotsList[i].myData.unitData == null || tsc.slotsList[i].myData.detail == null)
                {
                    continue;
                }
                // do something with tsc.slotsList
                if (tsc.slotsList[i].myData.unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
                {
                    tsc.slotsList[i].GetComponent<RewardAnimation>()._plant_Eff.gameObject.SetActive(true);
                    tsc.slotsList[i].GetComponent<RewardAnimation>()._plant_Eff.Play();
                    tsc.slotsList[i]._plantpot_obj.GetComponent<PlantPotController>().PlayAnimationOther(nameAnim);
                    break;
                }
            }
        }
    }
    public void setUpPlantPotPrefads(CharacterData data)
    {
        switch (data.detail._growthPlante)
        {
            case GrowthPlante.None:
                data.detail._unitPrefab.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._seed_Img;
                break;
            case GrowthPlante.Seed:
                data.detail._unitPrefab.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._seed_Img;
                break;
            case GrowthPlante.Baby:
                data.detail._unitPrefab.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._babey_Img;
                break;
            case GrowthPlante.Growth:
                data.detail._unitPrefab.GetComponent<PlantPotController>().plant.sprite = data.detail._plant_Image;
                break;
            case GrowthPlante.Rotted:
                data.detail._unitPrefab.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._Rotted_Img;
                break;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------
    #region Havet
    public void onClickHaver(CharacterData data, int index)
    {
        SoundListObject.instance.OnclickSFX(3);
        Debug.Log("2");
        if ((int)data.detail._growthPlante < (int)GrowthPlante.Growth)//|| data.unitData._unitCountTime < 20)
        {
            //Debug.Log("1:return");
            return;
        }
        else if (data.detail._growthPlante == GrowthPlante.Rotted && data.unitData._unitCountTime > data.unitData.DecayTime && data.unitData.DecayTime != 0)
        {
            //TODO: haver plante rotted
            StartCoroutine(setDataHaverRotted(data));
            plantEff(data,true);
        }
        else
        {
            Debug.Log("3");
            if (data.unitData._unitStackCoine < 1)
            {
                return;
            }
            StartCoroutine(setDataToHaver(data));
            plantEff(data, false);
            setPlantEffectedToGameplay(data, "harvest");
            if (!PlayerObject.instance._checkplayTutorial)
            {
                TutorialGameplay.instance.CloseTutorial();
                TutorialGameplay.instance.PlayTutorial_5_Coin();
                Debug.Log("4");
            }
        }
        #region old Haver
        /*if (data.detail._planeNFT)
        {
            Debug.Log("2:NFT");
            float MaxNFT = data.unitData._unitTokenNFTMax;
            int ranReward = (int)UnityEngine.Random.Range(0, MaxNFT);
            int Reawrd = ranReward * data.unitData._unitStackCoine;
            Debug.Log("Reaward: " + Reawrd);

            PlayerObject.instance._tokenNFTReward += Reawrd;

            if (ranReward > MaxNFT - 3)
            {
                PlayerObject.instance._tokenNFTReward += 3;
            }
            data.unitData.resetDateTime();
            tsc.OnSlotDeleteBtnClick(index);
        }
        else
        {
            Debug.Log("2:Coine");
            int MaxCoine = data.unitData._unitCoineMax;
            int ranReward = UnityEngine.Random.Range(0, MaxCoine);
            int Reawrd = ranReward * data.unitData._unitStackCoine;

            Debug.Log("Reaward: " + Reawrd);

            PlayerObject.instance._coineReward += Reawrd;
            if (ranReward > MaxCoine - 1)
            {
                PlayerObject.instance._coineReward += 1;
            }
            data.unitData.resetDateTime();
        }*/
        #endregion
    }
    IEnumerator setDataHaverRotted(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.Harvest(XCoreManager.instance.mXCoreInstance, "zone" + (int)data.detail._zonePos, "block" + (data.detail._unitPos + 1), (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }

        yield return setDeletePlant(data);
        DeleteDataThisZone(data);
        for (int i = 0; i < tsc.slotsList.Count; i++)
        {
            if (tsc.slotsList[i].myData.unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
            {
                tsc.slotsList[i].deleteUnitDataInSlot();
                tsc.slotsList[i].HideAllBtn();
                tsc.slotSelectedIndex = -1;
                break;
            }
        }
        yield return PlayerObject.instance.GetExpPlayer();
        yield return PlayerObject.instance.GetWalletPlayer();
    }
    IEnumerator setDataToHaver(CharacterData data)
    {
        if (data.detail._planeNFT)
        {
            yield return HavetGoldPlante(data, false);
        }
        else
        {
            yield return HavetNormalPlante(data, false);
        }
    }
    IEnumerator HavetGoldPlante(CharacterData data,bool auto)
    {
        IWSResponse response = null;
        if (!auto)
        {
            yield return GameAPI.Harvest(XCoreManager.instance.mXCoreInstance, "zone" + (int)data.detail._zonePos, "block" + (data.detail._unitPos + 1), (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
        }
        yield return setDeletePlant(data);
        DeleteDataThisZone(data);
        for (int i = 0; i < tsc.slotsList.Count; i++)
        {
            if ((tsc.slotsList[i].myData == null) || (tsc.slotsList[i].myData.detail == null) || (tsc.slotsList[i].myData.unitData == null))
            {
                continue;
            }
            if (tsc.slotsList[i].myData.unitData._unitCurrentPlant == data.unitData._unitCurrentPlant)
            {
                tsc.slotsList[i].deleteUnitDataInSlot();
                tsc.slotsList[i].HideAllBtn();
                tsc.slotSelectedIndex = -1;
                break;
            }
        }
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var wallet = response as WalletResp;
        int numsetCoine = 0;
        int numsetGem = 0;
        numsetCoine = wallet.coin - PlayerObject.instance._coineReward;
        for (int i = 0; i < tsc.slotsList.Count; i++)
        {
            if (tsc.slotsList[i].GetComponent<RewardAnimation>().isHaver)
            {
                if (numsetCoine > 10)
                {
                    numsetCoine = 10;
                    tsc.slotsList[i].GetComponent<RewardAnimation>().setUpCoin(numsetCoine);
                }
                else
                {
                    tsc.slotsList[i].GetComponent<RewardAnimation>().setUpCoin(numsetCoine);
                }
                //tsc.slotsList[i].GetComponent<RewardAnimation>().setUpExp(numsetCoine);
            }
        }
        if (wallet.gem > PlayerObject.instance._tokenNFTReward)
        {
            numsetGem = wallet.gem - PlayerObject.instance._tokenNFTReward;
            for (int i = 0; i < tsc.slotsList.Count; i++)
            {
                if (tsc.slotsList[i].GetComponent<RewardAnimation>().isHaver)
                {
                    if (numsetCoine > 10)
                    {
                        numsetCoine = 10;
                        tsc.slotsList[i].GetComponent<RewardAnimation>().setUpGem(numsetGem);
                    }
                    else
                    {
                        tsc.slotsList[i].GetComponent<RewardAnimation>().setUpGem(numsetGem);
                    }
                }
            }
        }
        yield return PlayerObject.instance.GetWalletPlayer();
    }
    IEnumerator HavetNormalPlante(CharacterData data,bool auto)
    {
        IWSResponse response = null;
        if (!auto)
        {
            yield return GameAPI.Harvest(XCoreManager.instance.mXCoreInstance, "zone" + (int)data.detail._zonePos, "block" + (data.detail._unitPos + 1), (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
        }
        yield return UserPlant.GetPlantProgressByBlock(XCoreManager.instance.mXCoreInstance, "zone" + (int)data.detail._zonePos, "block" + (data.detail._unitPos + 1), (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var plants = response as UserPlant;
        //Debug.Log("Date Time: " + plants.timeStamp);
        data.unitData._unitStackCoine = plants.pendingReward;
        if (data.unitData._unitMaxStackCoine <= 1)
        {
            data.unitData._unitCountTimeHaver = setTimeNowStaking(plants.timeStamp) * plants.pendingReward;
        }
        else
        {
            data.unitData._unitCountTimeHaver = setTimeNowStaking(plants.timeStamp);
        }
        yield return PlayerObject.instance.GetExpPlayer();
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var wallet = response as WalletResp;
        int numsetCoine = 0;
        int numsetGem = 0;
        numsetCoine = wallet.coin - PlayerObject.instance._coineReward;
        for (int i = 0; i < tsc.slotsList.Count; i++)
        {
            if (tsc.slotsList[i].GetComponent<RewardAnimation>().isHaver)
            {
                if (numsetCoine > 10)
                {
                    numsetCoine = 10;
                    tsc.slotsList[i].GetComponent<RewardAnimation>().setUpCoin(numsetCoine);
                }
                else
                {
                    tsc.slotsList[i].GetComponent<RewardAnimation>().setUpCoin(numsetCoine);
                }
                //tsc.slotsList[i].GetComponent<RewardAnimation>().setUpExp(numsetCoine);
            }
        }
        if (wallet.gem > PlayerObject.instance._tokenNFTReward)
        {
            numsetGem = wallet.gem - PlayerObject.instance._tokenNFTReward;
            for (int i = 0; i < tsc.slotsList.Count; i++)
            {
                if (tsc.slotsList[i].GetComponent<RewardAnimation>().isHaver)
                {
                    if (numsetCoine > 10)
                    {
                        numsetCoine = 10;
                        tsc.slotsList[i].GetComponent<RewardAnimation>().setUpGem(numsetGem);
                    }
                    else
                    {
                        tsc.slotsList[i].GetComponent<RewardAnimation>().setUpGem(numsetGem);
                    }
                }
            }
        }
        yield return PlayerObject.instance.GetWalletPlayer();
    }
    #endregion

    #region AutoHaverDisplay
    public void AutoSetEffectHarvest(CharacterData data)
    {
        StartCoroutine(setDataToHaverAuto(data));
        plantEff(data, false);
        setPlantEffectedToGameplay(data, "harvest");
    }
    IEnumerator setDataToHaverAuto(CharacterData data)
    {
        if (data.detail._planeNFT)
        {
            yield return HavetGoldPlante(data, true);
        }
        else
        {
            yield return HavetNormalPlante(data, true);
        }
    }
    #endregion
    //---------------------------------------------------------------------------------------------------------------------


    public double setTimeNowStaking(DateTime timeOld)
    {
        double numDouble;
        DateTime dateTimeClint = DateTime.Now;
        TimeSpan timeSpan = dateTimeClint - timeOld;
        numDouble = timeSpan.TotalSeconds + PlayerObject.instance.dateTimeServer;
        Debug.Log("Pase to double: " + PlayerObject.instance.dateTimeServer);
        Debug.Log("Pase to double: " + numDouble);
        return numDouble;
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
    public void onClickBuySlot(int index,int price, moneyType type)
    {
        Debug.Log("Buy Slot with " + type + " on price " + price);
        OpenUiLayerGameplay();
        SoundListObject.instance.OnclickSFX(0);
        switch (type)
        {
            case moneyType.Coine:
                if (PlayerObject.instance._coineReward < price)
                {
                    Debug.Log("price " + price + " is to high");
                    //Error ui 
                    WarningUi.instance._thisObject.SetActive(true);
                    WarningUi.instance.setupWarning("Not enough money", "Please check the money in your wallet.");
                    break;
                }
                else
                {
                    Debug.Log("price " + price + " is ok");
                    indexSlot = index;
                    if (!PlayerObject.instance._checkplayTutorial)
                    {
                        TutorialGameplay.instance.CloseTutorial();
                        //TutorialGameplay.instance._Cursor_obj.SetActive(false);
                        infoBuyslot_tutorial.SetActive(true);
                    }
                    else
                    {
                        infoBuyslot.SetActive(true);
                    }
                }
                break;
            case moneyType.TokenNFT:
                if (PlayerObject.instance._tokenNFTReward < price)
                {
                    Debug.Log("price " + price + " is to high");
                    //Error ui 
                    WarningUi.instance._thisObject.SetActive(true);
                    WarningUi.instance.setupWarning("Not enough money", "Please check the money in your wallet.");
                    break;
                }
                else
                {
                    Debug.Log("price " + price + " is ok");
                    indexSlot = index;
                    infoBuyslot.SetActive(true);
                }
                break;
            case moneyType.THB:
                indexSlot = index;
                string zoneIAP = ((int)zone).ToString();
                slot.index = indexSlot + 1;
                slot.zoneIndex = zoneIAP;
                slot.setUpProductID(indexSlot + 1, zoneIAP);
                break;
        }
    }
    public void onclickConfirmBuySlot()
    {
        StartCoroutine(setBuySlot(indexSlot));
    }
    IEnumerator setBuySlot(int index)
    {
        IWSResponse response = null;
        yield return GameAPI.UnlockBlock(XCoreManager.instance.mXCoreInstance, "zone" + (int)PlayerObject.instance._zone, "block" + (index + 1), (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.RawResult().ToString());
            yield break;
        }
        List<UnlockBlockReward> blockRewardsList = UnlockBlockReward.ParseToList(response.RawResult().ToString());
        tsc.OnUnlockSlotComplete();
        plusCountZoneUnlock(index);
        CloseUiLayerGameplay();
        infoBuyslot.SetActive(false);
        yield return PlayerObject.instance.GetWalletPlayer();
        RewardUi.gameObject.SetActive(true);
        RewardUi.getRewardInData(blockRewardsList);
        if (!PlayerObject.instance._checkplayTutorial)
        {
            yield return Account.SetTutorialPlayed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            infoBuyslot_tutorial.SetActive(false);
        }
    }
    public IEnumerator setIAPManager(string productID, string receipt)
    {
        IWSResponse response = null;
        yield return IAPItem.BuyIAP(XCoreManager.instance.mXCoreInstance, productID, receipt, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<UnlockBlockReward> blockRewardsList = UnlockBlockReward.ParseToList(response.RawResult().ToString());
        tsc.OnUnlockSlotComplete();
        plusCountZoneUnlock(indexSlot);
        CloseUiLayerGameplay();
        infoBuyslotIAP.SetActive(false);
        RewardUi.gameObject.SetActive(true);
        RewardUi.getRewardInData(blockRewardsList);
    }
    public void updateCountZoneunlock()
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == zone)
            {
                ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot = 0;
                for (int c = 0; c < ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone.Count; c++)
                {
                    checkSlotinthisZone(ZoneUnitObject.instance.unitDataZones[i]._cannaBisDatasThisZone[c].detail._unitPos);
                }
                for (int z = 0; z < ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone.Count; z++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone[z] == true)
                    {
                        ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot += 1;
                    }
                }
            }
        }
    }
    public void plusCountZoneUnlock(int index)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == zone)
            {
                ZoneUnitObject.instance.unitDataZones[i]._countUnlockSlot += 1;
                ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone[index] = true;
            }
        }
    }
    public void checkSlotinthisZone(int index)
    {
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == zone)
            {
                ZoneUnitObject.instance.unitDataZones[i]._slotDataThisZone[index] = true;

            }
        }
    }
    public void OpenUiLayerGameplay()
    {
        for (int i = 0; i < unitslot_gameObjec.Count; i++)
        {
            unitslot_gameObjec[i].GetComponent<PolygonCollider2D>().enabled = false;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().plantBtn.GetComponent<PolygonCollider2D>().enabled = false;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().unlockBtn.GetComponent<PolygonCollider2D>().enabled = false;
        }
        for (int i = 0; i < poppup_gameList.Count; i++)
        {
            poppup_gameList[i].GetComponent<PolygonCollider2D>().enabled = false;
        }
        tsc.HideAllSlotBtn();
        indexDeleted = 0;
    }
    public void CloseUiLayerGameplay()
    {
        for (int i = 0; i < unitslot_gameObjec.Count; i++)
        {
            unitslot_gameObjec[i].GetComponent<PolygonCollider2D>().enabled = true;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().plantBtn.GetComponent<PolygonCollider2D>().enabled = true;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().unlockBtn.GetComponent<PolygonCollider2D>().enabled = true;
        }
        for (int i = 0; i < poppup_gameList.Count; i++)
        {
            poppup_gameList[i].GetComponent<PolygonCollider2D>().enabled = true;
        }
        tsc.HideAllSlotBtn();
        indexDeleted = 0;
    }
    public void OpenUiLayerGameplayTutorial()
    {
        for (int i = 0; i < unitslot_gameObjec.Count; i++)
        {
            if (i == 0)
            {
                unitslot_gameObjec[0].GetComponent<PolygonCollider2D>().enabled = true;
                unitslot_gameObjec[0].GetComponent<TeamSlot>().plantBtn.GetComponent<PolygonCollider2D>().enabled = true;
                unitslot_gameObjec[0].GetComponent<TeamSlot>().unlockBtn.GetComponent<PolygonCollider2D>().enabled = true;
                continue;
            }
            unitslot_gameObjec[i].GetComponent<PolygonCollider2D>().enabled = false;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().plantBtn.GetComponent<PolygonCollider2D>().enabled = false;
            unitslot_gameObjec[i].GetComponent<TeamSlot>().unlockBtn.GetComponent<PolygonCollider2D>().enabled = false;
        }
        for (int i = 0; i < poppup_gameList.Count; i++)
        {
            poppup_gameList[i].GetComponent<PolygonCollider2D>().enabled = false;
        }
        tsc.HideAllSlotBtn();
        indexDeleted = 0;
    }
    public void oncloseTutorial()
    {
        TutorialGameplay.instance._pic_img[2].SetActive(true);
        TutorialGameplay.instance._bg_face_img.gameObject.SetActive(true);
    }
}
