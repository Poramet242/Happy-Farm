using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class GachaAPI : BaseWSResponse
    {
        
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string gachaID;
        public string name;
        public int price;
        public string priceCurrency;
        public List<string> items;
        public List<GachaRate> gachaRates;


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
            this.gachaID = data["gachaID"].Value;
            this.name = data["name"].Value;
            this.price = data["name"].AsInt;
            this.priceCurrency = data["priceCurrency"].Value;

            this.items = new List<string>();
            var items = data["items"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
               this.items.Add(items[i].Value);
            }

            this.gachaRates = new List<GachaRate>();
            var rates = data["gachaRate"].AsArray;
            for (int i = 0; i <rates.Count; i++)
            {
                var itemJson = rates[i].AsObject;
                var item = new GachaRate();
                item.ParseFromJSONObject(itemJson);
                gachaRates.Add(item);
                Debug.Log(item.rarityType);
            }


        }

        public static List<GachaAPI> ParseToList(string jsonString)
        {
            List<GachaAPI> gachaAPIs = new List<GachaAPI>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new GachaAPI();
                item.ParseFromJSONObject(itemJson);
                gachaAPIs.Add(item);
            }

            return gachaAPIs;

        }
        public static IEnumerator DrawGacha(XCore xcoreInst, string gachaID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("gachaID", gachaID);

            yield return xcoreInst.POST<UserSeed>(
                apiPath: Uri.EscapeUriString("/api/v1/gacha/draw"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator DrawGachaX10(XCore xcoreInst, string gachaID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("gachaID", gachaID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/gacha/drawX10"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }
        
        public static IEnumerator GetGachaList(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gacha/list"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}
