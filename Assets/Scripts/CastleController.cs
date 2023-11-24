using System;
using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour
{
    public static CastleController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        checkTutorialPlay = PlayerObject.instance._checkplayTutorial;
    }
    [SerializeField] public bool checkTutorialPlay;
    [SerializeField] public GameObject thisCastleController;
    [Header("Button")]
    [SerializeField] public Button _shopping_btn;
    [SerializeField] public Button _inventory_btn;
    [SerializeField] public Button _assistants_btn;
    [SerializeField] public Button _friend_btn;
    [SerializeField] public Button _zone_btn;
    [SerializeField] public Button _spin_btn;
    [SerializeField] public Button _recruitAssistant_btn;
    [SerializeField] public Button _setting_btn;
    [Header("URL")]
    [SerializeField] private string _url;
    [Header("Adtibuild")]
    [SerializeField] public Text _limitCharacter_txt;

    private void Update()
    {
        if (FriendObject.instance.checkSceneFriend)
        {
            _shopping_btn.gameObject.SetActive(false);
            _inventory_btn.gameObject.SetActive(false);
            _assistants_btn.gameObject.SetActive(false);
            _friend_btn.gameObject.SetActive(false);
            _zone_btn.gameObject.SetActive(true);
            _spin_btn.gameObject.SetActive(false);
            _recruitAssistant_btn.gameObject.SetActive(false);
        }
        else
        {
            _shopping_btn.gameObject.SetActive(true);
            _inventory_btn.gameObject.SetActive(true);
            _assistants_btn.gameObject.SetActive(true);
            _friend_btn.gameObject.SetActive(true);
            _zone_btn.gameObject.SetActive(true);
            _spin_btn.gameObject.SetActive(true);
            _recruitAssistant_btn.gameObject.SetActive(true);
        }
        _limitCharacter_txt.text = ZoneUnitObject.instance.countAssisstantDetailThiszone(PlayerObject.instance._zone).ToString();
    }
    public void cantClickUiDisplay(bool all)
    {
        _shopping_btn.enabled = false;
        _inventory_btn.enabled = false;
        _assistants_btn.enabled = false;
        _friend_btn.enabled = false;
        _zone_btn.enabled = false;
        _spin_btn.enabled = false;
        _recruitAssistant_btn.enabled = false;
        _setting_btn.enabled = false;
        if (!all)
        {
            _inventory_btn.enabled = true;
        }
    }
    public void canClickUiDisplay()
    {
        _shopping_btn.enabled = true;
        _inventory_btn.enabled = true;
        _assistants_btn.enabled = true;
        _friend_btn.enabled = true;
        _zone_btn.enabled = true;
        _spin_btn.enabled = true;
        _recruitAssistant_btn.enabled = true;
    }

    #region Onclick button
    public void onClickInventory_btn()
    {
        if (checkTutorialPlay)
        {
            //close
            StakeLayerController.instance.OpenUiLayerGameplay();
            //addtibuild
            SoundListObject.instance.OnclickSFX(0);
            MainLobbyController.instance.m_inventory.SetActive(true);
            InventoryLayerController.instance._sell_btn.SetActive(true);
            InventoryLayerController.instance._plant_btn.SetActive(false);
        }
        else
        {
            //TODO: Tutorial Inventory
            TutorialGameplay.instance._face_img.gameObject.SetActive(false);
            //close
            StakeLayerController.instance.OpenUiLayerGameplay();
            //addtibuild
            SoundListObject.instance.OnclickSFX(0);
            MainLobbyController.instance.m_inventory.SetActive(true);
            InventoryLayerController.instance._sell_btn.SetActive(true);
            InventoryLayerController.instance._plant_btn.SetActive(false);
        }
        if (!PlayerObject.instance._checkplayTutorial)
        {
            TutorialGameplay.instance._infototurial_obj.SetActive(false);
        }
        if (InventoryLayerController.instance._unitinventoryDisplayList.Count != 0)
        {
            PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(InventoryLayerController.instance._unitinventoryDisplayList[0].GetComponent<PlantsDisplay>().characterDataList);
            InventoryLayerController.instance.SortIconImgByLockStatus();
        }
    }
    public void onClickShopping_btn()
    {
        //close
        StakeLayerController.instance.OpenUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        MainLobbyController.instance.m_shopping.SetActive(true);
        ShopLayerController.instance.onclikeCoineShopBar();
    }
    public void onClickAssistants_btn()
    {
        //close
        StakeLayerController.instance.OpenUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        MainLobbyController.instance.m_assistants.SetActive(true);
    }
    public void onClickFriends_btn()
    {
        //close
        StakeLayerController.instance.OpenUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        MainLobbyController.instance.m_friend.SetActive(true);
        FriendLayerController.instance.onClickMyFriends();
    }
    public void onClickZone_btn()
    {
        //close
        StakeLayerController.instance.OpenUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        MainLobbyController.instance.m_zone.SetActive(true);
    }
    public void onClickGacha_btn()
    {
        if (checkTutorialPlay)
        {
            //close
            StakeLayerController.instance.OpenUiLayerGameplay();
            //addtibuild
            SoundListObject.instance.OnclickSFX(0);
            MainLobbyController.instance.m_spin.SetActive(true);
        }
        else
        {
            //TODO: Tutorial Gacha
            TutorialGameplay.instance._face_img.gameObject.SetActive(false);
            //close
            StakeLayerController.instance.OpenUiLayerGameplay();
            //addtibuild
            SoundListObject.instance.OnclickSFX(0);
            MainLobbyController.instance.m_spin.SetActive(true);
        }

    }
    public void onClickRecruitAssistant_btn()
    {
        SoundListObject.instance.OnclickSFX(0);
        Application.OpenURL(_url);
    }
    public void onClickSetting_btn()
    {
        //close
        StakeLayerController.instance.OpenUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        if (MainLobbyController.instance.m_setting != null)
        {
            MainLobbyController.instance.m_setting.SetActive(true);
        }
        else
        {
            GameObject x = GameObject.Find("SettingLayer(Clone)");
            x.SetActive(true);
            MainLobbyController.instance.m_setting = x;
        }
        SettingController.instance.onclickGameSetting_bar();
    }
    #endregion

}
