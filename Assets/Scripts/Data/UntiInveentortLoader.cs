using UnityEngine;

public class UntiInveentortLoader : MonoBehaviour
{
    public static UntiInveentortLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void setUpSpeciesTypeUint(UnitData unitData, UnitDetail unitDetail, UnitInventory unitInventory)
    {
        SpeciesTypeUint speciesTypeUint = new SpeciesTypeUint();
        switch (unitDetail._speciesType)
        {
            case SpeciesType.A:
                //before
                if (PlayerPrefs.GetString("isFirstTimesetUpSpecies") == "false")
                {
                    for (int i = 0; i < unitInventory.Species.Count; i++)
                    {
                        if (unitInventory.Species[i].speciesType == SpeciesType.A)
                        {
                            unitInventory.Species[i].unitDatas.Add(unitData);
                            unitInventory.Species[i].unitDetails.Add(unitDetail);
                            break;
                        }
                    }
                    break;
                }
                //after
                else
                {
                    //PlayerPrefs.SetString("isFirstTimesetUpSpecies", "true");
                    PlayerPrefs.SetInt("isFirstTimesetUpSpecies",1);
                    speciesTypeUint.speciesType = unitDetail._speciesType;
                    speciesTypeUint.unitDatas.Add(unitData);
                    speciesTypeUint.unitDetails.Add(unitDetail);
                    unitInventory.Species.Add(speciesTypeUint);
                    PlayerPrefs.SetString("isFirstTimesetUpSpecies", "false");
                    break;
                }
            case SpeciesType.B:
                if (PlayerPrefs.GetString("isFirstTimesetUpSpecies") == "false")
                {
                    for (int i = 0; i < unitInventory.Species.Count; i++)
                    {
                        if (unitInventory.Species[i].speciesType == SpeciesType.B)
                        {
                            unitInventory.Species[i].unitDatas.Add(unitData);
                            unitInventory.Species[i].unitDetails.Add(unitDetail);
                            break;
                        }
                    }
                    break;
                }
                //after
                else
                {
                    PlayerPrefs.SetString("isFirstTimesetUpSpecies", "true");
                    speciesTypeUint.speciesType = unitDetail._speciesType;
                    speciesTypeUint.unitDatas.Add(unitData);
                    speciesTypeUint.unitDetails.Add(unitDetail);
                    unitInventory.Species.Add(speciesTypeUint);
                    PlayerPrefs.SetString("isFirstTimesetUpSpecies", "false");
                    break;
                }
            case SpeciesType.C:
                break;
            case SpeciesType.D:
                break;
            case SpeciesType.E:
                break;
        }
    }
}
