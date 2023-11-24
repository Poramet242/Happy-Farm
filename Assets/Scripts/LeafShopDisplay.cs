using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class LeafShopDisplay : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CharacterData characterData;
    [Header("Sprite")]
    [SerializeField] private Image _iconLeaf;
    [SerializeField] private Text _nameLeaf;
    [SerializeField] private Text _buyPrice;
    [SerializeField] private Button _buy_btn;

    [SerializeField] private Sprite isbuy_srp;
    [SerializeField] private Sprite nonesell_srp;
    private void Update()
    {
        if (characterData.unitData._currentCountPlantGold == 0)
        {
            _buy_btn.GetComponent<Image>().sprite = nonesell_srp;
            _buy_btn.enabled = false;
        }
        else
        {
            _buy_btn.GetComponent<Image>().sprite = isbuy_srp;
            _buy_btn.enabled = true;
        }
    }
    public void setUpLeafShop(CharacterData data)
    {
        characterData = data;
        _iconLeaf.sprite = data.detail._unitLocalImage;
        _nameLeaf.text = data.detail._unitName;
        _buyPrice.text = data.unitData._priceBuyPlane.ToString("#,##0");
    }
    public void onClickOpenInfo()
    {
        SoundListObject.instance.OnclickSFX(0);
        ShopLayerController.instance.infoGoldPlant_obj.SetActive(true);
        ShopLayerController.instance.infoGoldShow_cls.setupDataPlantShop(characterData);
    }
    public void onClickBuyLeaf()
    {
        SoundListObject.instance.OnclickSFX(0);
        ShopLayerController.instance.infoGoldPlant_obj.SetActive(true);
        ShopLayerController.instance.infoGoldShow_cls.setupDataPlantShop(characterData);
    }
    IEnumerator BuySeedNFT(CharacterData data)
    {
        IWSResponse response = null;
        yield return GameAPI.BuySeed(XCoreManager.instance.mXCoreInstance, data.detail._unitTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error get data to planting seed");
            yield break;
        }
        yield return PlayerObject.instance.GetWalletPlayer();
        yield return UserSeed.GetUserSeed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        List<UserSeed> all_seeds = UserSeed.ParseToList(response.RawResult().ToString());
        for (int i = 0; i < all_seeds.Count; i++)
        {
            UnitDetail unitDetail = ScriptableObject.CreateInstance<UnitDetail>();
            UnitData unitData = ScriptableObject.CreateInstance<UnitData>();
            //set plant ID
            unitDetail._unitTokenID = all_seeds[i].plantID;
            unitData._unitTokenID = all_seeds[i].plantID;
            unitData._unitCurrentPlant = all_seeds[i].id;
            unitDetail._unitCurrentPlant = all_seeds[i].id;

            var plant = GameData.instance.GetPlantInfoByPlantID(unitDetail._unitTokenID);
            StakeUnitObject.instance.SetUpDataPlantinfo(plant, unitDetail, unitData);

            CharacterData newdata = new CharacterData();
            newdata.detail = unitDetail;
            newdata.unitData = unitData;
            StakeUnitObject.instance._allUnitDataList.Add(newdata.unitData);
            StakeUnitObject.instance._allUnitDetailList.Add(newdata.detail);
        }
    }
}
