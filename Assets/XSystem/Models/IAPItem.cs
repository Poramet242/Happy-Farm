using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class IAPItem : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string itemID;
        public string productIDiOS;
        public string productIDAndroid;
        public string name;
        public string details;
        public int amount;
        public string currency;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.id = data["id"].Value;
            this.createdOn = Utility.ParseDatetime(data["createdOn"].Value);
            this.updatedOn = Utility.ParseDatetime(data["updatedOn"].Value);
            this.itemID = data["itemID"].Value;
            this.productIDiOS = data["productIDiOS"].Value;
            this.productIDAndroid = data["productIDAndroid"].Value;
            this.name = data["name"].Value;
            this.details = data["details"].Value;
            this.amount = data["amount"].AsInt;
            this.currency = data["currency"].Value;

        }

        public static List<IAPItem> ParseToList(string jsonString)
        {
            List<IAPItem> iapItems = new List<IAPItem>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new IAPItem();
                item.ParseFromJSONObject(itemJson);
                iapItems.Add(item);
            }

            return iapItems;

        }

        public static IEnumerator GetIAPItems(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/iap/list"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator BuyIAP(XCore xcoreInst, string itemID, string receipt, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("itemID", itemID);
            formData.AddField("receipt", receipt);
            if (Application.platform == RuntimePlatform.Android)
            {
                formData.AddField("platform", "android");
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                formData.AddField("platform", "iOS");
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                formData.AddField("platform", "editor");
            }

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/iap/buy"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }
    }
}