using System;
using System.Collections.Generic;
using UnityEngine;


public class AssistantsLayerController : MonoBehaviour
{
    public static AssistantsLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Assistants Inventory list")]
    [SerializeField] private GameObject _content_obj;
    [SerializeField] private GameObject _unitTemplate;
    [SerializeField] private List<GameObject> _assisInventoryDisplayList = new List<GameObject>();
    [SerializeField] private List<AssisstantDetail> all_AssisDataList = new List<AssisstantDetail>();
    [Header("Count Auto")]
    [SerializeField] public int countAuto = 0;
    [SerializeField] public List<AssisstantDetail> _assisDiapalyList= new List<AssisstantDetail>();
    [Header("Warning")]
    [SerializeField] public WarningUi warningPanel_obj;
    public void Initialize()
    {
        all_AssisDataList = new List<AssisstantDetail>();
        for (int i = 0; i < AssistantsObject.instance._assistantsAllList.Count; i++)
        {
            AssisstantDetail allAssis = new AssisstantDetail();
            allAssis = AssistantsObject.instance._assistantsAllList[i];
            all_AssisDataList.Add(allAssis);
        }
        if (_assisInventoryDisplayList.Count == 0)
        {
            for (int i = 0; i < all_AssisDataList.Count; i++)
            {
                GameObject unitAssistants = Instantiate(_unitTemplate, _content_obj.transform);
                unitAssistants.SetActive(true);
                _assisInventoryDisplayList.Add(unitAssistants);
                SetDataFromScriptableObjects(unitAssistants, all_AssisDataList[i]);
            }
        }
        for (int i = 0; i < all_AssisDataList.Count; i++)
        {
            if (all_AssisDataList[i]._unitWork && all_AssisDataList[i]._zonePos == PlayerObject.instance._zone)
            {
                _assisDiapalyList.Add(all_AssisDataList[i]);
                AutoCalculateHaver.instance.setupAssistantAutoDispalyList(all_AssisDataList[i]);
                setDataCharacterDisplayZone(all_AssisDataList[i]);
            }
        }
        onOpneAutoHaver(_assisDiapalyList);
    }
    public void setDataCharacterDisplayZone(AssisstantDetail detail)
    {
        for (int i = 0; i < _assisInventoryDisplayList.Count; i++)
        {
            if (_assisInventoryDisplayList[i].GetComponent<AssistantsDisplay>()._assistantsDataDetail._unitTokenID == detail._unitTokenID)
            {
               StartCoroutine(MovementController.instance.setupInstanceAssistants(detail));
            }
        }
    }
    public void SetDataFromScriptableObjects(GameObject tempObj, AssisstantDetail tempdata)
    {
        tempObj.GetComponent<AssistantsDisplay>().setDataAssistantsDisplay(tempObj, tempdata);
    }
    public void onClickClose()
    {
        StakeLayerController.instance.CloseUiLayerGameplay();
        SoundListObject.instance.OnclickSFX(0);
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        onOpneAutoHaver(_assisDiapalyList);
    }
    #region Auto Haver
    /// <summary>
    /// this function to setup count assisstants to active auto Haver get list AssisstantDetail to work in zone
    /// </summary>
    /// <param name="assisstantsAuto"></param>
    public void onOpneAutoHaver(List<AssisstantDetail> assisstantsAuto)
    {
        if (assisstantsAuto.Count != 0)
        {
            countAuto = 0;
            for (int i = 0; i < assisstantsAuto.Count; i++)
            {
                switch (assisstantsAuto[i]._rarityType)
                {
                    case CannabisFarm.Models.RarityType.Common: //24h
                        countAuto += 1;
                        break;
                    case CannabisFarm.Models.RarityType.Rare: //12h
                        countAuto += 1;
                        break;
                    case CannabisFarm.Models.RarityType.Epic: //8h
                        countAuto += 1;
                        break;
                    case CannabisFarm.Models.RarityType.Legendary: //6h
                        countAuto += 1;
                        break;
                }
            }
            if (countAuto > StakeLayerController.instance.unitslot_gameObjec.Count)
            {
                countAuto = StakeLayerController.instance.unitslot_gameObjec.Count;
            }
        }
    }
    #endregion
}
