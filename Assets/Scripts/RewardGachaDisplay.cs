using UnityEngine;
using UnityEngine.UI;

public class RewardGachaDisplay : MonoBehaviour
{
    public static RewardGachaDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public CharacterData characterDatas;
    [Header("Adtibuild")]
    [SerializeField] private Image _plantIconDisplay;
    [SerializeField] private Text _countPlant;
    [SerializeField] private Text _namePlant;
    [SerializeField] private GameObject _poppupNew;


    public void setupDataRewardGachaDisplay(CharacterData data)
    {
        characterDatas = data;
        setupDisplayObject(characterDatas);
    }
    public void setupDisplayObject(CharacterData data)
    {
        _plantIconDisplay.sprite = data.detail._unitLocalImage;
        _countPlant.text = data.unitData._unitCountPlane.ToString();
        _namePlant.text = data.detail._unitName;
    }

}
