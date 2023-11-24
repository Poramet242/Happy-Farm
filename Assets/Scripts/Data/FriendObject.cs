using CannabisFarm.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendObject : MonoBehaviour
{
    public static FriendObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Friend")]
    [SerializeField] public bool checkSceneFriend;
    [SerializeField] public List<FriendDetail> _allFriendslist = new List<FriendDetail>();

    public void setFriendDetail(List<Account> accountsFriend, List<FriendDetail> FriendDetail)
    {
        for (int i = 0; i < accountsFriend.Count; i++)
        {
            FriendDetail friendDetail = ScriptableObject.CreateInstance<FriendDetail>();
            friendDetail.name = accountsFriend[i].displayName;
            friendDetail.playerTokenID = accountsFriend[i].userID;
            friendDetail.playerName = accountsFriend[i].displayName;
            friendDetail.playerURLImage = accountsFriend[i].displayImageID;
            if (friendDetail.playerURLImage == null || friendDetail.playerURLImage == string.Empty)
            {
                friendDetail.playerLocalImage = UnitDataLoader.Instance.GetLocalIcon("Avatardefault");
                setupFriendAccount(accountsFriend[i], friendDetail);
            }
            else
            {
                friendDetail.playerLocalImage = UnitDataLoader.Instance.GetLocalIcon(accountsFriend[i].displayImageID);
                setupFriendAccount(accountsFriend[i], friendDetail);
            }
            FriendDetail.Add(friendDetail);
        }
    }
    public void setupFriendAccount(Account accountsFriend, FriendDetail friendDetail)
    {
        friendDetail.playerLevel = accountsFriend.level;
        friendDetail.countCoin = accountsFriend.coin;
        friendDetail.countCoinNFT = accountsFriend.gem;
    }
}
