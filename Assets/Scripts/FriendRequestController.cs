using CannabisFarm.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XSystem;

public class FriendRequestController : MonoBehaviour
{
    public static FriendRequestController instance;
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
    [SerializeField] public List<GameObject> _unitDisplayList = new List<GameObject>();
    [SerializeField] public List<FriendDetail> nowRequestFriendDetail = new List<FriendDetail>();
    private void OnEnable()
    {
        for (int i = 0; i < _unitDisplayList.Count; i++)
        {
            Destroy(_unitDisplayList[i]);
        }
        nowRequestFriendDetail.Clear();
        _unitDisplayList.Clear();
        StartCoroutine(LoadRequestFriend());
    }
    IEnumerator LoadRequestFriend()
    {
        IWSResponse response = null;
        yield return FriendAPI.GetReceivedFriendList(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        List<Account> accountsFriend = Account.ParseToList(response.RawResult().ToString());
        FriendObject.instance.setFriendDetail(accountsFriend, nowRequestFriendDetail);
        yield return setupDisplayAddFriend(nowRequestFriendDetail);
    }
    IEnumerator setupDisplayAddFriend(List<FriendDetail> friendDetails)
    {
        if (_unitDisplayList.Count == 0)
        {
            friendDetails = friendDetails.OrderByDescending(x => x.playerLevel).ToList();
            for (int i = 0; i < friendDetails.Count; i++)
            {
                GameObject friendDisplay = Instantiate(_untiTemplate, _content_obj.transform);
                friendDisplay.name = friendDetails[i].name;
                friendDisplay.SetActive(true);
                _unitDisplayList.Add(friendDisplay);
                _unitDisplayList[i].GetComponent<FriendRequestDisplay>().setupFriendDetail(friendDetails[i]);
            }
        }
        yield break;
    }
}
