using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class AddFriendController : MonoBehaviour
{
    public static AddFriendController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Data")]
    [SerializeField] private GameObject _content_obj;
    [SerializeField] private GameObject _untiTemplate;
    [SerializeField] private InputField textSearchInput;
    [SerializeField] private GameObject _searchpopup;
    [SerializeField] public List<GameObject> _unitDisplayList = new List<GameObject>();
    [SerializeField] public List<FriendDetail> nowSearchFriendDetail = new List<FriendDetail>();
    private void OnEnable()
    {
        textSearchInput.text = null;
        _searchpopup.SetActive(true);
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            Destroy(_unitDisplayList[i]);
        }
        nowSearchFriendDetail.Clear();
        _unitDisplayList.Clear();
    }
    public void resetData()
    {
        _searchpopup.SetActive(true);
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            Destroy(_unitDisplayList[i]);
        }
        nowSearchFriendDetail.Clear();
        _unitDisplayList.Clear();
    }
    public void onClickSearchAddFriend()
    {
        resetData();
        SoundListObject.instance.OnclickSFX(0);
        StartCoroutine(LoadAddFriendSearch(textSearchInput.text));
    }
    IEnumerator LoadAddFriendSearch(string input)
    {
        IWSResponse response = null;
        yield return FriendAPI.FindFriendByName(XCoreManager.instance.mXCoreInstance, input, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        Debug.Log(response.RawResult().ToString());
        List<Account> accountsFriend = Account.ParseToList(response.RawResult().ToString());
        yield return FriendAPI.FindFriendUID(XCoreManager.instance.mXCoreInstance, input, (r) => response = r);
        if (response.Success())
        {
            var accountFriendUID = response as Account;
            if (!accountsFriend.Contains(accountFriendUID))
            {
                accountsFriend.Add(accountFriendUID);
            }
        }
        FriendObject.instance.setFriendDetail(accountsFriend, nowSearchFriendDetail);
        yield return setupDisplayAddFriend(nowSearchFriendDetail);
    }
    IEnumerator setupDisplayAddFriend(List<FriendDetail> friendDetails)
    {
        _searchpopup.SetActive(false);
        if (_unitDisplayList.Count == 0)
        {
            friendDetails = friendDetails.OrderByDescending(x => x.playerLevel).ToList();
            for (int i = 0; i < friendDetails.Count; i++)
            {
                GameObject friendDisplay = Instantiate(_untiTemplate, _content_obj.transform);
                friendDisplay.name = friendDetails[i].name;
                friendDisplay.SetActive(true);
                _unitDisplayList.Add(friendDisplay);
                _unitDisplayList[i].GetComponent<AddFriendDisplay>().setupFriendDetail(friendDetails[i]);
            }
        }
        yield break;
    }
}
