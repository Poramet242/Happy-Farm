using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class FriendRequestDisplay : MonoBehaviour
{
    public static FriendRequestDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] private FriendDetail friendDetail;
    [Header("Display")]
    [SerializeField] private Image profile_friend;
    [SerializeField] private Text name_text;
    [SerializeField] private Text level_text;
    [SerializeField] private Text count_coin_text;
    [SerializeField] private Text count_NFT_text;
    [SerializeField] private Button add_btn;
    [SerializeField] private Button delete_btn;
    public void setupFriendDetail(FriendDetail detail)
    {
        friendDetail = detail;
        setupDisplay(detail);
    }
    public void setupDisplay(FriendDetail detail)
    {
        profile_friend.sprite = detail.playerLocalImage;
        name_text.text = detail.playerName;
        level_text.text = "Lv." + detail.playerLevel.ToString();
        count_coin_text.text = detail.countCoin.ToString("#,##0");
        count_NFT_text.text = detail.countCoinNFT.ToString("#,##0");
    }
    public void onClickAddfriendRequest()
    {
        SoundListObject.instance.OnclickSFX(0);
        StartCoroutine(setAddFriendRequest(friendDetail));
    }
    public void onClickDeleteRequest()
    {
        SoundListObject.instance.OnclickSFX(0);
        StartCoroutine(setDeleteRequest(friendDetail));
    }
    IEnumerator setAddFriendRequest(FriendDetail friend)
    {
        IWSResponse response = null;
        yield return FriendAPI.AcceptFriendRequest(XCoreManager.instance.mXCoreInstance, friend.playerTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        add_btn.interactable = false;
        delete_btn.interactable = false;
        FriendObject.instance._allFriendslist.Add(friend);
    }
    IEnumerator setDeleteRequest(FriendDetail friend)
    {
        IWSResponse response = null;
        yield return FriendAPI.DenyFriendRequest(XCoreManager.instance.mXCoreInstance, friend.playerTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        this.gameObject.SetActive(false);
    }
}
