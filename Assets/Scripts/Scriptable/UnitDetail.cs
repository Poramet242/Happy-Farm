using CannabisFarm.Models;
using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitDeatil", menuName = "ScriptableObjects/UnitDeatil", order = 0)]
public class UnitDetail : ScriptableObject
{
    public bool _planeNFT;
    public string _unitTokenID;
    public string _unitName;
    public string _unitCurrentPlant;
    [Header("Imgaes")]
    public string _unitURLImage;
    public int _unitPos;
    [Header("Zone")]
    public ZoneType _zonePos;
    [Header("Growth")]
    public GrowthPlante _growthPlante;
    public SpeciesType _speciesType;
    [Header("Image Type")]
    public Sprite _unitLocalImage;
    public Sprite _plant_Image;
    public PlantType _plantType;
    public UnitData _unitData;
    [Header("Staking")]
    public bool _unitStaking;
    [Header("Animator")]
    public GameObject _unitPrefab;
    public GameObject _unitParticle;
}
public enum PlantType
{
    Normal = 0,
    Golden = 1,
}
