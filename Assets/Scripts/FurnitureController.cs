using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureController : MonoBehaviour
{
    [SerializeField] public FurnitureType furniture;
    public void onClickFurniture()
    {
        FurnitureShop.instance.thisObject.SetActive(true);
        FurnitureShop.instance.setupThisFurniture(furniture);
    }
}
