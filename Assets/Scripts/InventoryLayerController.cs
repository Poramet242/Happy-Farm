using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryLayerController : MonoBehaviour
{
    public static InventoryLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Unit Inventory list")]
    [SerializeField] private GameObject _content_obj;
    [SerializeField] private GameObject _unitTemplate;
    [SerializeField] public List<GameObject> _unitinventoryDisplayList = new List<GameObject>();
    [SerializeField] public List<CharacterData> all_unitDataList = new List<CharacterData>();
    [Header("Adtibuild")]
    [SerializeField] public GameObject _sell_btn;
    [SerializeField] public GameObject _plant_btn;
    [SerializeField] public GameObject _close_btn;
    [Header("Tutorial gameplay")]
    [SerializeField] public GameObject _tutor_Inventory;
    [SerializeField] public GameObject _totur_Info;
    [SerializeField] public GameObject _totur_Close;
    [SerializeField] public GameObject _totur_Info_plant;
    private void OnEnable()
    {
        ClearDataInventoryDisplay();
        all_unitDataList = new List<CharacterData>();
        for (int i = 0; i < StakeUnitObject.instance._allUnitDetailList.Count; i++)
        {
            if (StakeUnitObject.instance._allUnitDataList[i]._unitCountPlane == 0)
            {
                continue;
            }
            CharacterData allUnit = new CharacterData();
            allUnit.detail = StakeUnitObject.instance._allUnitDetailList[i];
            allUnit.unitData = StakeUnitObject.instance._allUnitDataList[i];
            if (!allUnit.detail._unitStaking)
            {
                all_unitDataList.Add(allUnit);
            }
        }

        if (_unitinventoryDisplayList.Count == 0)
        {
            List<string> allSpeciesTypeList = StakeUnitObject.instance.getAllUnitSperity();
            for (int i = 0; i < allSpeciesTypeList.Count; i++)
            {
                GameObject unitPlant = Instantiate(_unitTemplate, _content_obj.transform);
                unitPlant.SetActive(true);
                //unitPlant.GetComponent<PlantsDisplay>().SpeciesType = all_unitDataList[i].detail._speciesType;
                unitPlant.GetComponent<PlantsDisplay>()._unitTokenID = allSpeciesTypeList[i];
                _unitinventoryDisplayList.Add(unitPlant);
            }
            for (int i = 0; i < all_unitDataList.Count; i++)
            {
                SetDataFromScriptableObjects(all_unitDataList[i]);
            }
        }
        if (!PlayerObject.instance._checkplayTutorial)
        {
            TutorialGameplay.instance._infototurial_obj.SetActive(false);
            TutorialGameplay.instance._infototurial_obj2.SetActive(false);
            TutorialGameplay.instance._skip_btn.SetActive(false);
            //TutorialGameplay.instance._Cursor_obj.SetActive(false);
            _tutor_Inventory.SetActive(true);
            if (TutorialGameplay.instance.stageTutorial == Tutorial.Plante)
            {
                _tutor_Inventory.SetActive(false);
                _totur_Info.SetActive(false);
                _totur_Close.SetActive(false);
                _totur_Info_plant.SetActive(true);
            }
        }
    }
    public void SortIconImgByLockStatus()
    {
        _unitinventoryDisplayList.Sort((a, b) => b.GetComponent<PlantsDisplay>()._checkGolden.CompareTo(a.GetComponent<PlantsDisplay>()._checkGolden));
        for (int i = 0; i < _unitinventoryDisplayList.Count; i++)
        {
            _unitinventoryDisplayList[i].transform.SetSiblingIndex(i);
        }
    }
    public void Start()
    {
        if (_unitinventoryDisplayList.Count != 0)
        {
            PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(_unitinventoryDisplayList[0].GetComponent<PlantsDisplay>().characterDataList);
        }
    }
    private void Update()
    {
        for (int i = 0; i < _unitinventoryDisplayList.Count; i++)
        {
            if (_unitinventoryDisplayList[i]==null)
            {
                _unitinventoryDisplayList.RemoveAt(i);
            }
        }
    }
    public void onClickPlant()
    {
        PlantsInfoDisplay.Instance.onClickPlant();
        StakeLayerController.instance.plant_btn(false);
        if (!PlayerObject.instance._checkplayTutorial)
        {
            TutorialGameplay.instance.CloseTutorial();
            TutorialGameplay.instance._skip_btn.SetActive(true);
            TutorialGameplay.instance.PlayTutorial_4_Haver();
        }
    }
    public void SetDataFromScriptableObjects(CharacterData tempData)
    {
        for (int i = 0; i < _unitinventoryDisplayList.Count; i++)
        {
            _unitinventoryDisplayList[i].GetComponent<PlantsDisplay>().setDataPlantDisplay(tempData);
        }
    }
    public void removePlantZeroInList(CharacterData characterData)
    {
        all_unitDataList.Remove(characterData);
        StakeUnitObject.instance._allUnitDataList.Remove(characterData.unitData);
        StakeUnitObject.instance._allUnitDetailList.Remove(characterData.detail);
        for (int i = 0; i < _unitinventoryDisplayList.Count; i++)
        {
            if (_unitinventoryDisplayList[i].GetComponent<PlantsDisplay>().characterDataList.Count == 0)
            {
                Destroy(_unitinventoryDisplayList[i]);
            }
        }
        setNewInfoDisplay();
    }
    public void setNewInfoDisplay()
    {
        if (_unitinventoryDisplayList[0].GetComponent<PlantsDisplay>().characterDataList.Count == 0)
        {
            if (PlantsInfoDisplay.Instance._characterDataList.Count == 0 && _unitinventoryDisplayList.Count == 1)
            {
                _sell_btn.GetComponent<Button>().interactable = false;
                PlantsInfoDisplay.Instance.setInfoNoneDetail();
                return;
            }
            PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(_unitinventoryDisplayList[1].GetComponent<PlantsDisplay>().characterDataList);
        }
        else
        {
            PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(_unitinventoryDisplayList[0].GetComponent<PlantsDisplay>().characterDataList);
        }
    }
    public void ClearDataInventoryDisplay()
    {
        for (int i = 0; i < _unitinventoryDisplayList.Count; i++)
        {
            Destroy(_unitinventoryDisplayList[i]);
        }
        _unitinventoryDisplayList.Clear();
    }
    public void onClickClose()
    {
        if (PlayerObject.instance._checkplayTutorial)
        {
            SoundListObject.instance.OnclickSFX(0);
            //close
            StakeLayerController.instance.CloseUiLayerGameplay();
            //addtibuild
            this.gameObject.SetActive(false);
        }
        else
        {
            if (TutorialGameplay.instance.stageTutorial == Tutorial.Inventory)
            {
                TutorialGameplay.instance._pic_img[3].SetActive(true);
                TutorialGameplay.instance._bg_face_img.gameObject.SetActive(true);
                TutorialGameplay.instance._skip_btn.SetActive(true);
                SoundListObject.instance.OnclickSFX(0);
                //close
                StakeLayerController.instance.CloseUiLayerGameplay();
                //addtibuild
                this.gameObject.SetActive(false);
            }
            else if (TutorialGameplay.instance.stageTutorial == Tutorial.Plante)
            {
                return;
            }
        }
    }
}
