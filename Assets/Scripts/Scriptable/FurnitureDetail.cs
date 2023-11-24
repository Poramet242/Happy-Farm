using UnityEngine;
using System;
using CannabisFarm.Models;

[System.Serializable]
[CreateAssetMenu(fileName = "FurnitureDetail", menuName = "ScriptableObjects/FurnitureDetail", order = 5)]
public class FurnitureDetail : ScriptableObject
{
    [Header("Data")]
    public bool isUseFurniture;
    public bool isBuyFurniture;
    public string unitName;
    public string unitID;
    public string unitImagesID;
    public int unitPrice;
    public int unitPrice_NFT;
    public RarityType rarityType;
    public Sprite localImages;
    public FurnitureType furnitureType;
}
