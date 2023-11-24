using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class Account : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string uid;
        //Acctype and acc address
        public LoginTypes accountType;
        public string accountAddress;
        //--------------------------
        public string displayName;
        public string displayImageID;
        public List<string> unlockedArea;
        public int coin;
        public int gem;
        public int level;
        public int exp;
        public bool nameLocked;
        public bool tutorialPlayed;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            Debug.Log(jObj.ToString());
            base.ParseFromJSONObject(jObj);
            if (jObj["success"] == "false")
            {
                return;
            }
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.userID = data["userID"].Value;
            //------------------------------------------------------
            this.accountType = (LoginTypes)data["accountType"].AsInt;
            this.accountAddress = data["walletAddress"].Value;
            this.uid = data["uid"].Value;
            //------------------------------------------------------
            this.unlockedArea = new List<string>();
            var items = data["unlockedArea"].AsArray;
            for (int i = 0; i < unlockedArea.Count; i++)
            {
                this.unlockedArea.Add(items[i].Value);
            }
            //------------------------------------------------------
            this.displayName = data["displayName"].Value;
            this.displayImageID = data["displayImageID"].Value;
            this.coin = data["coin"].AsInt;
            this.gem = data["gem"].AsInt;
            this.level = data["level"].AsInt;
            this.exp = data["exp"].AsInt;
            this.nameLocked = data["nameLocked"].AsBool;
            this.tutorialPlayed = data["tutorialPlayed"].AsBool;
        }

        public static List<Account> ParseToList(string jsonString)
        {
            List<Account> accounts = new List<Account>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Account();
                item.ParseFromJSONObject(itemJson);
                accounts.Add(item);
            }

            return accounts;

        }

        public static IEnumerator GetUserProfile(XCore xcoreInst, Action<IWSResponse> callback)
        {

            //  var header = new Dictionary<string, string>();
            // header["X-Session-Token"] = XSession.Current().Token();

            yield return xcoreInst.GET<Account>(
            apiPath: Uri.EscapeUriString("/api/v1/account"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator SetName(XCore xcoreInst, string name, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("name", name);

            yield return xcoreInst.POST<Account>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setName"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }


        public static IEnumerator setProfileImage(XCore xcoreInst, string imageID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("imageID", imageID);

            yield return xcoreInst.POST<Account>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setProfileImage"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator SetNamePaid(XCore xcoreInst, string name, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("name", name);

            yield return xcoreInst.POST<Account>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setNamePaid"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator ResetAccount(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<LoginResult>(
                apiPath: Uri.EscapeUriString("/api/v1/auth/reset"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator DeleteAccount(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/auth/delete"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator UnbindAccount(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/auth/unbind"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator SetTutorialPlayed(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/account/setTutorialPlayed"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }
    }
}
