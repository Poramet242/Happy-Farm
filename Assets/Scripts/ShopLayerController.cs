using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class ShopLayerController : MonoBehaviour
{
    public static ShopLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LeafShopPanel.SetActive(true);
    }
    [Header("Sprite")]
    [SerializeField] Sprite[] _buttonBarArray;
    [SerializeField] Image _selectedCoineShop_images;
    [SerializeField] Image _selectedLeafShop_images;
    [SerializeField] GameObject CoineShopPanel;
    [SerializeField] GameObject LeafShopPanel;
    [Header("Adtibuild")]
    [SerializeField] private GameObject _content_obj;
    [SerializeField] private GameObject _unitTemp;
    [SerializeField] private List<GameObject> all_unitSellPlantlist = new List<GameObject>();
    [SerializeField] public GameObject _WarningUi;
    [SerializeField] public GameObject infoGoldPlant_obj;
    [SerializeField] public ShopPlantGoldInfoDisplay infoGoldShow_cls;
    [Header("Golden plant Bar")]
    [SerializeField] public Text timeCooldown_text;
    [SerializeField] public TimeSpan cooldownTime;
    [SerializeField] public double LeafShopBarCooldownTime;
    [SerializeField] public bool isLeafShopBar;
    public void onclikeCoineShopBar()
    {
        SoundListObject.instance.OnclickSFX(0);
        LeafShopPanel.SetActive(false);
        CoineShopPanel.SetActive(true);
        _selectedCoineShop_images.sprite = _buttonBarArray[0];
        _selectedLeafShop_images.sprite = _buttonBarArray[1];
        isLeafShopBar = false;
    }   
    public void onclikeLeafShopBar()
    {
        SoundListObject.instance.OnclickSFX(0);
        LeafShopPanel.SetActive(true);
        CoineShopPanel.SetActive(false);
        _selectedLeafShop_images.sprite = _buttonBarArray[0];
        _selectedCoineShop_images.sprite = _buttonBarArray[1];
        isLeafShopBar = true;
        CalculateTimeShopGolden();
        onSetupPlantGoldShop();
    }
    public void CalculateTimeShopGolden()
    {
        LeafShopBarCooldownTime = 0;
        DateTime today = DateTime.Now;
        DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        cooldownTime = endOfMonth - today;
        LeafShopBarCooldownTime = cooldownTime.TotalSeconds;
        if (LeafShopBarCooldownTime <= 0)
        {
            for (int i = 0; i < StakeUnitObject.instance._allSellUnitDataList.Count; i++)
            {
                StartCoroutine(resetCountGoldenPlant(StakeUnitObject.instance._allSellUnitDataList[i]));
            }
        }
    }
    public void onSetupPlantGoldShop()
    {
        all_unitSellPlantlist.Clear();
        for (int i = 0; i < StakeUnitObject.instance._allSellUnitDataList.Count; i++)
        {
            GameObject unit = Instantiate(_unitTemp, _content_obj.transform);
            unit.SetActive(true);
            all_unitSellPlantlist.Add(unit);
            SetDataFromScriptableObjects(StakeUnitObject.instance._allSellUnitDataList[i], unit);
        }
    }
    public void SetDataFromScriptableObjects(CharacterData tempData, GameObject @object)
    {
        @object.GetComponent<LeafShopDisplay>().setUpLeafShop(tempData);
    }
    private void Update()
    {
        if (isLeafShopBar)
        {
            LeafShopBarCooldownTime -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(LeafShopBarCooldownTime);
            timeCooldown_text.text = ((timeSpan.Days * 24) + timeSpan.Hours).ToString() + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        }
    }
    IEnumerator resetCountGoldenPlant(CharacterData Selldata)
    {
        IWSResponse response = null;
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
    }

    public void onclickClose()
    {
        SoundListObject.instance.OnclickSFX(0);
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        this.gameObject.SetActive(false);
        isLeafShopBar = false;

    }
}
