using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPlanteDisplay : MonoBehaviour
{
    public static RewardPlanteDisplay instance;
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
    [SerializeField] private int num;
    [SerializeField] private Image _plantIconDisplay;
    [SerializeField] private Text _countPlant;
    [SerializeField] private Text _namePlant;
    [SerializeField] private GameObject _poppupNew;


    public void setupDataRewardDisplay(CharacterData data, int count)
    {
        characterDatas = data;
        num = count;
        setupDisplayObject(characterDatas);
    }
    public void setupDisplayObject(CharacterData data)
    {
        _plantIconDisplay.sprite = data.detail._unitLocalImage;
        _countPlant.text = "+" + num.ToString();
        _namePlant.text = data.detail._unitName;
    }
}
