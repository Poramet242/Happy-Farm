using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XSystem;


public enum SortedDisplay 
{ 
    SortedCoin = 0,
    SortedCoinNFT = 1,
    SortedLike = 2,

}
public class FriendLayerController : MonoBehaviour
{
    public static FriendLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] private GameObject _content_obj;
    [SerializeField] private GameObject _unitTemplate;
    [SerializeField] public List<GameObject> _unitDisplayList = new List<GameObject>();
    [SerializeField] public List<FriendDetail> all_unitDataList = new List<FriendDetail>();
    [Header("Sprite")]
    [SerializeField] Sprite[] _buttonBarArray;
    [SerializeField] Image _selectedMyFriends_images;
    [SerializeField] Image _selectedAddFriends_images;
    [SerializeField] Image _selectedRequestFriends_images;
    [SerializeField] GameObject MyFriendsPanel;
    [SerializeField] GameObject AddFriendsPanel;
    [SerializeField] GameObject FriendRequestPanel;
    [SerializeField] InputField SearchField_myFriends;
    [SerializeField] InputField SearchField_AddFriends;
    [Header("Addtibuild")]
    [SerializeField] public GameObject faceBack;
    private void OnEnable()
    {
        ClearDataInventoryDisplay();
        all_unitDataList = new List<FriendDetail>();
        for (int i = 0; i < FriendObject.instance._allFriendslist.Count; i++)
        {
            FriendDetail detail = new FriendDetail();
            detail = FriendObject.instance._allFriendslist[i];
            all_unitDataList.Add(detail);
        }
        if (_unitDisplayList.Count == 0)
        {
            //max => min
            all_unitDataList = all_unitDataList.OrderByDescending((x) => x.countCoin).ToList();
            //min => max
            //all_unitDataList = all_unitDataList.OrderBy(x => x.playerLevel).ToList();
            for (int i = 0; i < all_unitDataList.Count; i++)
            {
                int index = i;
                GameObject friendDisplay = Instantiate(_unitTemplate, _content_obj.transform);
                friendDisplay.name = all_unitDataList[i].playerName;
                friendDisplay.SetActive(true);
                _unitDisplayList.Add(friendDisplay);
                _unitDisplayList[i].GetComponent<FriendsDisplay>().setupFriendDetail(all_unitDataList[i], i);
            }
        }
    }
    /// <summary>
    /// this function is clear data friend display in list and this to sorted value to Friend display
    /// </summary>
    /// <param name="sorted"> get enum to set switch to sorted </param>
    public void setSortedFriendDispaly(SortedDisplay sorted)
    {
        ClearDataInventoryDisplay();
        switch (sorted)
        {
            case SortedDisplay.SortedCoin:
                all_unitDataList = all_unitDataList.OrderByDescending((x) => x.countCoin).ToList();
                break;
            case SortedDisplay.SortedCoinNFT:
                all_unitDataList = all_unitDataList.OrderByDescending((x) => x.countCoinNFT).ToList();
                break;
            case SortedDisplay.SortedLike:
                all_unitDataList = all_unitDataList.OrderByDescending((x) => x.countLike).ToList();
                break;
        }
        for (int i = 0; i < all_unitDataList.Count; i++)
        {
            int index = i;
            GameObject friendDisplay = Instantiate(_unitTemplate, _content_obj.transform);
            friendDisplay.name = all_unitDataList[i].playerName;
            friendDisplay.SetActive(true);
            _unitDisplayList.Add(friendDisplay);
            _unitDisplayList[i].GetComponent<FriendsDisplay>().setupFriendDetail(all_unitDataList[i], i);
        }
    }
    public void resetDisplayMyFriends()
    {
        if (SearchField_myFriends.text == string.Empty)
        {
            for (int i = 0; i < _unitDisplayList.Count; i++)
            {
                _unitDisplayList[i].SetActive(true);
            }
        }
    }
    public void onClickSearchMyFriends(InputField input)
    {
        SoundListObject.instance.OnclickSFX(0);
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            if ((input.text == _unitDisplayList[i].GetComponent<FriendsDisplay>().friendDetails.playerName) || (input.text == _unitDisplayList[i].GetComponent<FriendsDisplay>().friendDetails.playerTokenID))
            {
                _unitDisplayList[i].SetActive(true);
            }
            else
            {
                _unitDisplayList[i].SetActive(false);
            }
        }
    }
    public void DeleteFriend(FriendDetail temp)
    {
        SoundListObject.instance.OnclickSFX(0);
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            if (_unitDisplayList[i].GetComponent<FriendsDisplay>().friendDetails == temp)
            {
                Destroy(_unitDisplayList[i]);
                _unitDisplayList.Remove(_unitDisplayList[i]);
            }
        }
        _unitDisplayList = _unitDisplayList.OrderByDescending((x) => x.GetComponent<FriendsDisplay>().friendDetails.countCoin).ToList();
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            _unitDisplayList[i].GetComponent<FriendsDisplay>().indexFriend = i;
        }
    }
    public void ClearDataInventoryDisplay()
    {
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            Destroy(_unitDisplayList[i]);
        }
        _unitDisplayList.Clear();
        SearchField_myFriends.text = string.Empty;
    }



    #region on click btn
    public void onClickMyFriends()
    {
        SoundListObject.instance.OnclickSFX(0);
        MyFriendsPanel.SetActive(true);
        AddFriendsPanel.SetActive(false);
        FriendRequestPanel.SetActive(false);
        _selectedMyFriends_images.sprite = _buttonBarArray[0];
        _selectedAddFriends_images.sprite = _buttonBarArray[1];
        _selectedRequestFriends_images.sprite = _buttonBarArray[1];
        if (SearchField_AddFriends == null){return;}
        SearchField_AddFriends.text = null;
        this.enabled = false;
        this.enabled = true;
    }
    public void onClickAddFriends()
    {
        SoundListObject.instance.OnclickSFX(0);
        MyFriendsPanel.SetActive(false);
        AddFriendsPanel.SetActive(true);
        FriendRequestPanel.SetActive(false);
        _selectedMyFriends_images.sprite = _buttonBarArray[1];
        _selectedAddFriends_images.sprite = _buttonBarArray[0];
        _selectedRequestFriends_images.sprite = _buttonBarArray[1];
        if (SearchField_myFriends == null){return;}
        SearchField_myFriends.text = null;
    }
    public void onClickFriendRequest()
    {
        SoundListObject.instance.OnclickSFX(0);
        MyFriendsPanel.SetActive(false);
        AddFriendsPanel.SetActive(false);
        FriendRequestPanel.SetActive(true);
        _selectedMyFriends_images.sprite = _buttonBarArray[1];
        _selectedAddFriends_images.sprite = _buttonBarArray[1];
        _selectedRequestFriends_images.sprite = _buttonBarArray[0];
    }
    public void OnClickCloseLayer()
    {
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        MyFriendsPanel.SetActive(true);
        AddFriendsPanel.SetActive(true);
        SearchField_myFriends.text = null;
        SearchField_AddFriends.text = null;
        this.gameObject.SetActive(false);
    }
    #endregion
}
