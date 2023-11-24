using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitDataLoader : MonoBehaviour
{
    public static UnitDataLoader Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #region Assistant
    public Sprite GetLocalIcon(string IconID)
    {
        string basePath = "Icon/" + IconID;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    public Sprite GetLocalImagesAssistan(string ImagesID)
    {
        string basePath = "Icon/" + ImagesID;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    public GameObject GetLocalGameObjectAssistan(string NameID)
    {
        string basePath = "Data/CharacterFarmer/CharacterGameObject/" + NameID;
        GameObject temp = Resources.Load<GameObject>(basePath);
        return temp;
    }
    #endregion

    #region plant cannabis
    public Sprite GetLocalImages(string speciesType)
    {
        string basePath = "Data/Cannabis/Icon/" + speciesType;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    public Sprite GetLocalPlant(string speciesType)
    {
        string basePath = "Data/Cannabis/Icon/" + speciesType;
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    public GameObject GetLocalObjPlant(string speciesType)
    {
        string basePath = "Prefabs/Cannabis/PlantPot/Plant/" + speciesType;
        GameObject unitPrefab = Resources.Load<GameObject>(basePath);
        return unitPrefab;
    }
    public GameObject GetLocalParticlePlant(string speciesType)
    {
        string basePath = "Particle/" + speciesType;
        GameObject unitPrefab = Resources.Load<GameObject>(basePath);
        return unitPrefab;
    }
    #endregion

    #region Furniture
    public Sprite GetLocalImagesFurniture(FurnitureType furnitureType,string ImagesID)
    {
        string basePath = "Furniture/";
        switch (furnitureType)
        {
            case FurnitureType.Gamearcade:
                basePath += "Gamearcade/" + ImagesID;
                break;
            case FurnitureType.Car:
                basePath += "Car/" + ImagesID;
                break;
            case FurnitureType.BassPad:
                basePath += "BassPad/" + ImagesID;
                break;
            case FurnitureType.Stadium:
                basePath += "Stadium/" + ImagesID;
                break;
            case FurnitureType.Ring:
                basePath += "Ring/" + ImagesID;
                break;
            case FurnitureType.Locker:
                basePath += "Locker/" + ImagesID;
                break;
        }
        Sprite sprite = Resources.Load<Sprite>(basePath);
        return sprite;
    }
    #endregion
}
