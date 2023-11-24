using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantsDisplay : MonoBehaviour
{
    public static PlantsDisplay Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("Data")]
    //[SerializeField] public SpeciesType SpeciesType;
    [SerializeField] public string _unitTokenID;
    [SerializeField] public bool _checkGolden;
    [SerializeField] public List<CharacterData> characterDataList = new List<CharacterData>();
    [Header("Detail Show")]
    [SerializeField] public Image _planeIcone_img;
    [SerializeField] public Image _planeFrame_img;
    [SerializeField] public Text _planeCount_text;
    [SerializeField] public Text _planeName_text;

    public void setDataPlantDisplay(CharacterData data)
    {
        if (data.detail._unitTokenID == _unitTokenID)
        {
            characterDataList.Add(data);
            setUpPlanesDisplay(characterDataList[0]);
        }
    }
    private void Update()
    {
        _planeCount_text.text = characterDataList.Count.ToString();
    }
    public void setUpPlanesDisplay(CharacterData data)
    {
        _planeIcone_img.sprite = data.detail._unitLocalImage;
        _planeCount_text.text = data.unitData._unitCountPlane.ToString();
        _planeName_text.text = data.detail._unitName;
        if (data.detail._plantType == PlantType.Golden)
        {
            _checkGolden = true;
        }
        else
        {
            _checkGolden = false;
        }
    }
    public void onClickPlantDisplay()
    {
        SoundListObject.instance.OnclickSFX(0);

        PlantsInfoDisplay.Instance._plant_Scr.gameObject.SetActive(false);
        PlantsInfoDisplay.Instance.setUpPlaneInfoDisplay(characterDataList);
        if (!PlayerObject.instance._checkplayTutorial)
        {
            InventoryLayerController.instance._tutor_Inventory.SetActive(false);
            InventoryLayerController.instance._totur_Info.SetActive(true);
        }
    }
}
