using System;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject
{
    [Header("Data")]
    public string _unitTokenID;
    public string _unitName;
    public string _unitCurrentPlant;
    [Header("Product")]
    public int _priceBuyPlane;
    public int _priceSellPlane;
    public int _unitCountPlane;
    public int _unitCoineMax;
    public float _unitTokenNFTMax;
    public bool _unitCanBuy;
    [Header("Gold plant")]
    public int _currentCountPlantGold;
    public int _maxCountPlantGold;
    [Header("Grow Stack")]
    public int _unitStackCoine;
    public int _unitMaxStackCoine;
    [Header("Date Tiem")]
    public bool isLife;
    public int GrowTime;
    public int DecayTime;
    public int timePerCoin;
    public double _unitCountTime;
    public double _unitCountTimeHaver;
    public DateTime _unitDateTimeStamp;
    [Header("InfoData")]
    public string _unitInfo;

    public void resetDateTime()
    {
        _unitStackCoine = 0;
        _unitCountTime = 10;
        _unitCountTimeHaver = 0;
        //_unitDateTimeStamp = DateTime.Now;
        Debug.Log("3:resetDateTime");
    }
    public int setDataCountTime(int time)
    {
        int count = 0;
        count = (time * 60) * 60;
        return count;
    }
}
