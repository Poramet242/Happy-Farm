using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitInventory", menuName = "ScriptableObjects/UnitInventory", order = 2)]
public class UnitInventory : ScriptableObject
{
    [Header("Species Type")]
    public List<SpeciesTypeUint> Species = new List<SpeciesTypeUint>();
}
[System.Serializable]
public class SpeciesTypeUint 
{
    public SpeciesType speciesType;
    public List<UnitDetail> unitDetails = new List<UnitDetail>();
    public List<UnitData> unitDatas = new List<UnitData>();
}
