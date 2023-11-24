using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class FriendAPI : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string senderID;
        public string targetID;
        public bool isActive;
        public bool isAccepted;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.senderID = data["senderID"].Value;
            this.targetID = data["targetID"].Value;
            this.isActive = data["isActive"].AsBool;
            this.isAccepted = data["isAccepted"].AsBool;

        }

        public static IEnumerator GetFriendList(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/friend/list"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator GetReceivedFriendList(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/friend/received"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator GetSentFriendList(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/friend/sent"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator GetRecommendedFriendList(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/friend/recommend"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator FindFriend(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<Account>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/find"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator AcceptFriendRequest(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<FriendAPI>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/accept"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator DenyFriendRequest(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<FriendAPI>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/deny"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator SendFriendRequest(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<FriendAPI>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/sendRequest"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator RemoveFriend(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<FriendAPI>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/remove"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator FindFriendByName(XCore xcoreInst, string keyword, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("keyword", keyword);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/findByName"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }


        public static IEnumerator FindFriendUID(XCore xcoreInst, string targetID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("targetID", targetID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/friend/findByUID"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

    }

}