using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsVisitController : MonoBehaviour
{
    [Header("Friends Data")]
    [SerializeField] private FriendDetail friendDetails;
    private string secenFriends = "Friend_Garage_zone";

    public void onClickVisitDisFriend()
    {
        FriendLayerController.instance.faceBack.SetActive(true);
        FriendObject.instance.checkSceneFriend = true;
        ZoneUnitObject.instance.resetDatathisZone(FriendObject.instance.checkSceneFriend);
    }
}
