using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class SpinLayerController : MonoBehaviour
{
    public static SpinLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] private GameObject _content1_obj;
    [SerializeField] private GameObject _content10_obj;
    [SerializeField] private GameObject _unitTemplate;
    [SerializeField] private SpinRewardAnimation _rewardAnimation;
    [Header("DataGacha")]
    [SerializeField] private int _coutSpin;
    [SerializeField] private bool _checkbuttonNFT;
    [SerializeField] private List<CharacterData> _rewardSpinGachaDisplayList = new List<CharacterData>();
    [SerializeField] private List<GameObject> _rewardSpinGachaObjeList = new List<GameObject>();
    [Header("Adtibuild")]
    [SerializeField] private GameObject _gachaPanel;
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private GameObject _closeReward_btn;
    [SerializeField] private GameObject _backGround;
    [SerializeField] private GameObject _close_btn;
    [SerializeField] private WarningUi _callError;
    [SerializeField] public Text coine_Price_text;
    [SerializeField] public Text coine10_Price_text;
    [SerializeField] public Text gem_Price_text;
    [SerializeField] public Text gem10_Price_text;
    [Header("Reward")]
    [SerializeField] private Image _backGroundImage;
    [SerializeField] private Sprite _reward_1;
    [SerializeField] private Sprite _reward_10;
    public void onClickGacha(Spin_btn spin_Btn)
    {
        SoundListObject.instance.OnclickSFX(1);
        StartCoroutine(setGachaPack(spin_Btn));
    }
    public void CloseObjectTogachaGameplay()
    {
        _rewardPanel.SetActive(false);
        _closeReward_btn.SetActive(false);
        _backGround.SetActive(false);
        ProfileLayerController.instance._profileObj.SetActive(false);
        ProfileLayerController.instance._activeSkillInconPanel.SetActive(false);
        ProfileLayerController.instance._infoPanel.SetActive(false);
        CastleController.instance.thisCastleController.SetActive(false);
    }
    public void ShowDefToGameplay()
    {
        ProfileLayerController.instance._profileObj.SetActive(true);
        ProfileLayerController.instance._activeSkillInconPanel.SetActive(true);
        ProfileLayerController.instance._infoPanel.SetActive(true);
        _rewardPanel.SetActive(true);
        _closeReward_btn.SetActive(true);
        _backGround.SetActive(true);
    }
    IEnumerator setGachaPack(Spin_btn spin_Btn)
    {
        IWSResponse response = null;
        yield return GachaAPI.GetGachaList(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<GachaAPI> gachas = GachaAPI.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < gachas.Count; i++)
        {
            if (spin_Btn._checkNFT && gachas[i].priceCurrency == "gem")
            {
                StartCoroutine(CalculateGachaSpin(spin_Btn._countSpin, gachas[i].gachaID, spin_Btn._checkNFT));
            }
            else if (!spin_Btn._checkNFT && gachas[i].priceCurrency == "coin")
            {
                StartCoroutine(CalculateGachaSpin(spin_Btn._countSpin, gachas[i].gachaID, spin_Btn._checkNFT));
            }
        }

    }
    IEnumerator CalculateGachaSpin(int count, string GachaID, bool checkNFT)
    {
        IWSResponse response = null;
        SoundListObject.instance.OnclickSFX(2);
        if (count == 10)
        {
            yield return GachaAPI.DrawGachaX10(XCoreManager.instance.mXCoreInstance, GachaID, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                if (checkNFT)
                {
                    _callError.gameObject.SetActive(true);
                    _callError._innfo_txt.text = "Not enough Gem";
                }
                else
                {
                    _callError.gameObject.SetActive(true);
                    _callError._innfo_txt.text = "Not enough Coin";
                }
                yield break;
            }
            List<UserSeed> seeds = UserSeed.ParseToList(response.RawResult().ToString());
            for (int i = 0; i < seeds.Count; i++)
            {
                UnitDetail detail = ScriptableObject.CreateInstance<UnitDetail>();
                UnitData unitData = ScriptableObject.CreateInstance<UnitData>();

                detail._unitTokenID = seeds[i].plantID;
                unitData._unitTokenID = seeds[i].plantID;
                unitData._unitCurrentPlant = seeds[i].id;
                detail._unitCurrentPlant = seeds[i].id;

                var plant = GameData.instance.GetPlantInfoByPlantID(detail._unitTokenID);
                StakeUnitObject.instance.SetUpDataPlantinfo(plant, detail, unitData);
                //set character data
                CharacterData newdata = new CharacterData();
                newdata.detail = detail;
                newdata.unitData = unitData;

                _rewardSpinGachaDisplayList.Add(newdata);
            }
            yield return PlayerObject.instance.GetWalletPlayer();
            ShowRewardDisplayGacha(_rewardSpinGachaDisplayList, checkNFT,false);
        }
        else if (count == 1)
        {
            yield return GachaAPI.DrawGacha(XCoreManager.instance.mXCoreInstance, GachaID, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                if (checkNFT)
                {
                    _callError.gameObject.SetActive(true);
                    _callError._innfo_txt.text = "Not enough Gem";
                }
                else
                {
                    _callError.gameObject.SetActive(true);
                    _callError._innfo_txt.text = "Not enough Coin";
                }
                yield break;
            }
            var gachaData = response as UserSeed;
            UnitDetail detail = ScriptableObject.CreateInstance<UnitDetail>();
            UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
            detail._unitTokenID = gachaData.plantID;
            unitData._unitTokenID = gachaData.plantID;
            unitData._unitCurrentPlant = gachaData.id;
            detail._unitCurrentPlant = gachaData.id;
            var plant = GameData.instance.GetPlantInfoByPlantID(detail._unitTokenID);
            StakeUnitObject.instance.SetUpDataPlantinfo(plant, detail, unitData);
            //set character data
            CharacterData newdata = new CharacterData();
            newdata.detail = detail;
            newdata.unitData = unitData;
            _rewardSpinGachaDisplayList.Add(newdata);
            yield return PlayerObject.instance.GetWalletPlayer();
            ShowRewardDisplayGacha(_rewardSpinGachaDisplayList, checkNFT,true);
        }
    }
    #region oldsetupDetailGacha
    public CharacterData setupDetailSpin(UnitDetail detail, UnitData data)
    {
        CharacterData unitDataSpint = new CharacterData();
        //Detail plant
        detail._speciesType = RandomEnumValue<SpeciesType>();
        detail._unitTokenID = "#" + UnityEngine.Random.Range(0, 10).ToString();
        detail._unitName = "sativa_" + detail._speciesType;
        detail._zonePos = ZoneType.None;
        detail._growthPlante = GrowthPlante.None;
        if (detail._planeNFT)
        {
            List<GameObject> objPrefabsNFT = new List<GameObject>();
            foreach (GameObject item in Resources.LoadAll<GameObject>("Prefabs/Cannabis/NFT"))
            {
                objPrefabsNFT.Add(item);
            }
            detail._speciesType = SpeciesType.Gold;
            detail._planeNFT = true;
            detail._unitLocalImage = ImageDisplayController.instance._plantsIcon[0];
            detail._unitPrefab = objPrefabsNFT[0];
            data._unitTokenNFTMax = UnityEngine.Random.Range(10, 20);
        }
        else
        {
            List<GameObject> objPrefabs = new List<GameObject>();
            foreach (GameObject item in Resources.LoadAll<GameObject>("Prefabs/Cannabis/Coine"))
            {
                objPrefabs.Add(item);
            }
            detail._unitLocalImage = ImageDisplayController.instance._plantsIcon[(int)detail._speciesType];
            detail._unitPrefab = objPrefabs[(int)detail._speciesType - 1];
            data._unitTokenNFTMax = UnityEngine.Random.Range(5, 10);
        }
        //Data plant
        data._unitTokenID = detail._unitTokenID;
        data._unitName = detail._unitName;
        //data._pricePlane = 1000;
        data._unitCountPlane = 1;
        data._unitInfo = "it's cannabis";
        //Setname
        data.name = data._unitName;
        detail.name = detail._unitName;
        //Return data 
        unitDataSpint.unitData = data;
        unitDataSpint.detail = detail;
        return unitDataSpint;
    }
    //RandomEnumValue
    public static SpeciesType RandomEnumValue<SpeciesType>()
    {
        var values = Enum.GetValues(typeof(SpeciesType));
        int random = UnityEngine.Random.Range(1, values.Length);
        return (SpeciesType)values.GetValue(random);
    }
    #endregion
    public void ShowRewardDisplayGacha(List<CharacterData> characterDatas,bool checkNFT,bool num)
    {
        ShowGameObjectActive(true);
        for (int i = 0; i < characterDatas.Count; i++)
        {
            if (num)
            {
                _content1_obj.SetActive(true);
                _content10_obj.SetActive(false);
                GameObject unitPlant = Instantiate(_unitTemplate, _content1_obj.transform);
                unitPlant.SetActive(true);
                _rewardSpinGachaObjeList.Add(unitPlant);
                _rewardSpinGachaObjeList[i].GetComponent<RewardGachaDisplay>().setupDataRewardGachaDisplay(characterDatas[i]);
                _backGroundImage.sprite = _reward_1;

            }
            else if(!num)
            {
                _content1_obj.SetActive(false);
                _content10_obj.SetActive(true);
                GameObject unitPlant = Instantiate(_unitTemplate, _content10_obj.transform);
                unitPlant.SetActive(true);
                _rewardSpinGachaObjeList.Add(unitPlant);
                _rewardSpinGachaObjeList[i].GetComponent<RewardGachaDisplay>().setupDataRewardGachaDisplay(characterDatas[i]);
                _backGroundImage.sprite = _reward_10;
            }
        }
        _rewardAnimation.setUpAnimationSpin(SettingController.instance._checkAnimation, checkNFT);
    }
    public void clearDataRewardGacha()
    {
        for (int i = 0; i < _rewardSpinGachaObjeList.Count; i++)
        {
            Destroy(_rewardSpinGachaObjeList[i]);
        }
        _rewardSpinGachaDisplayList.Clear();
        _rewardSpinGachaObjeList.Clear();
    }
    public void onClickCloseReward()
    {
        SoundListObject.instance.OnclickSFX(0);
        addDataRewardGacha();
        ShowGameObjectActive(false);
        clearDataRewardGacha();
    }
    public void addDataRewardGacha()
    {
        for (int i = 0; i < _rewardSpinGachaDisplayList.Count; i++)
        {
            StakeUnitObject.instance._allUnitDataList.Add(_rewardSpinGachaDisplayList[i].unitData);
            StakeUnitObject.instance._allUnitDetailList.Add(_rewardSpinGachaDisplayList[i].detail);
        }
    }
    public void onclickClose()
    {
        SoundListObject.instance.OnclickSFX(0);
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        this.gameObject.SetActive(false);
        CastleController.instance.thisCastleController.SetActive(true);
    }
    public void ShowGameObjectActive(bool check)
    {
        _gachaPanel.SetActive(!check);
        _close_btn.SetActive(!check);
        _rewardPanel.SetActive(check);
        _closeReward_btn.SetActive(check);
    }
}
