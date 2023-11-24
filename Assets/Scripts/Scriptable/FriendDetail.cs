using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(fileName = "FriendDetail", menuName = "ScriptableObjects/FriendDetail", order = 4)]
public class FriendDetail : ScriptableObject
{
    [Header("Data")]
    public string playerTokenID;
    public string playerName;
    public string playerURLImage;
    public Sprite playerLocalImage;
    [Range(1, 100)]
    public int playerLevel = 1;
    [Header("Product")]
    public int countCoin;
    public int countCoinNFT;
    public int countLike;
    public bool[] unlockZone = new bool[3];
    [Header("Cannabis")]
    public List<CharacterData> all_unitdatalist = new List<CharacterData>();
    [Header("Assistant")]
    public List<AssisstantDetail> all_unitAssisstanDetail = new List<AssisstantDetail>();
    
}
