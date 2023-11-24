using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class FriendsDisplay : MonoBehaviour
{
    public static FriendsDisplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] public int indexFriend;
    [SerializeField] public FriendDetail friendDetails;
    [Header("Detail Show")]
    [SerializeField] public Text honorFriend_text;
    [SerializeField] public Image honorFriend_img;
    [SerializeField] public Image friendImages;
    [SerializeField] public Text friendName;
    [SerializeField] public Text friendLevel;
    [SerializeField] public Text countCoine;
    [SerializeField] public Text countCoineNFT;

    private void Update()
    {
        setupHonorThisFriend(indexFriend);
    }
    public void setupFriendDetail(FriendDetail detail, int index)
    {
        indexFriend = index;
        friendDetails = detail;
        setupDisplayFriend(detail);
    }
    public void setupDisplayFriend(FriendDetail detail)
    {
        friendName.text = detail.playerName;
        friendImages.sprite = detail.playerLocalImage;
        friendLevel.text = "Lv."+detail.playerLevel.ToString();
        countCoine.text = detail.countCoin.ToString("#,##0.####");
        countCoineNFT.text = detail.countCoinNFT.ToString("#,##0.####");
    }
    public void setupHonorThisFriend(int index)
    {
        if (index < 0 || index >= ImageDisplayController.instance._honor_img.Count)
        {
            honorFriend_img.gameObject.SetActive(false);
            honorFriend_text.gameObject.SetActive(true);
            honorFriend_text.text = (index + 1).ToString("#,##0");
        }
        else
        {
            honorFriend_img.gameObject.SetActive(true);
            honorFriend_text.gameObject.SetActive(false);
            honorFriend_img.sprite = ImageDisplayController.instance._honor_img[index];
        }
    }
    public void onclickDelete()
    {
        StartCoroutine(setDeleteFriend(friendDetails));
    }
    IEnumerator setDeleteFriend(FriendDetail friend)
    {
        IWSResponse response = null;
        yield return FriendAPI.RemoveFriend(XCoreManager.instance.mXCoreInstance, friend.playerTokenID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        FriendLayerController.instance.DeleteFriend(friendDetails);
    }
}
