using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureShop : MonoBehaviour
{
    public static FurnitureShop instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("This Furniture")]
    [SerializeField] private FurnitureType thisFurniture;
    [SerializeField] public GameObject thisObject;
    [Header("Data")]
    [SerializeField] private List<FurnitureDetail> furnitureDetailsList = new List<FurnitureDetail>();
    [SerializeField] private List<GameObject> furnitures_ojbList = new List<GameObject>();
    [SerializeField] private GameObject Content_obj;
    [SerializeField] private Transform target;

    public void setupThisFurniture(FurnitureType furniture)
    {
        ClearDataShopDisplay();
        StakeLayerController.instance.OpenUiLayerGameplay();
        thisFurniture = furniture;
        setupThisShop(furniture);
    }
    public void setupThisShop(FurnitureType furniture)
    {
        switch (furniture)
        {
            case FurnitureType.Gamearcade:
                setupdataListShop(furniture);
                break;
            case FurnitureType.Car:
                setupdataListShop(furniture);
                break;
            case FurnitureType.BassPad:
                setupdataListShop(furniture);
                break;
            case FurnitureType.Stadium:
                setupdataListShop(furniture);
                break;            
            case FurnitureType.Ring:
                setupdataListShop(furniture);
                break;            
            case FurnitureType.Locker:
                setupdataListShop(furniture);
                break;
        }
    }
    public void setupdataListShop(FurnitureType furniture)
    {
        furnitureDetailsList = new List<FurnitureDetail>();
        //TODO: get list furnitureDetailsList to display shop
        for (int f = 0; f < FurnitureUnitObject.instance.all_furnitureDetails.Count; f++)
        {
            if (FurnitureUnitObject.instance.all_furnitureDetails[f].furnitureType == furniture)
            {
                furnitureDetailsList.Add(FurnitureUnitObject.instance.all_furnitureDetails[f]);
            }
        }
        //---------------------------------------------------
        if (furnitures_ojbList.Count == 0)
        {
            for (int i = 0; i < furnitureDetailsList.Count; i++)
            {
                GameObject furnitureDisplay = Instantiate(Content_obj, target.transform);
                furnitureDisplay.name = furnitureDetailsList[i].unitName;
                furnitureDisplay.SetActive(true);
                furnitures_ojbList.Add(furnitureDisplay);
                furnitures_ojbList[i].GetComponent<FurnitureDisplay>().setupFurnitureDisplay(furnitureDetailsList[i]);
            }
        }
    }
    public void ClearDataShopDisplay()
    {
        furnitureDetailsList.Clear();
        for (int i = 0; i < furnitures_ojbList.Count; i++)
        {
            Destroy(furnitures_ojbList[i]);
        }
        furnitures_ojbList.Clear();
    }
    public void onClickClose()
    {
        SoundListObject.instance.OnclickSFX(0);
        StakeLayerController.instance.CloseUiLayerGameplay();
        thisObject.SetActive(false);
    }


}
