using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class AlphaGamePlayDemo
{
    #region
    public static bool CheckData;
    public static List<int> countUnlockZone = new List<int>();
    #endregion
    #region GamePlayDataProfile
    public static float TokenNFTReward = 0f;
    public static float CoineReward = 0f;
    public static List<FriendDetail> _friendDetails = new List<FriendDetail>();
    public static List<Sprite> _localIcon = new List<Sprite>();
    #endregion

    #region GamePlayDataCannabis
    public static bool _staking;
    //unstake
    public static List<UnitDetail> _unitDetailListUnstake = new List<UnitDetail>();
    public static List<UnitData> _unitDataListUnstake = new List<UnitData>();
    //stake
    public static List<UnitDetail> _unitDetailListStake = new List<UnitDetail>();
    public static List<UnitData> _unitDataListStake = new List<UnitData>();
    //all
    public static List<UnitDetail> _allUnitDetailList = new List<UnitDetail>();
    public static List<UnitData> _allUnitDataList = new List<UnitData>();
    #endregion

    #region GameplayDataAssistants
    public static List<AssisstantDetail> _assistantsAllList = new List<AssisstantDetail>();
    #endregion
}
