using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class AreaInfo : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string areaID;
        public string name;
        public int price;
        public string priceCurrency;
        public int price_2;
        public string priceCurrency_2;


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
            this.areaID = data["areaID"].Value;
            this.name = data["name"].Value;
            this.price = data["price"].AsInt;
            this.priceCurrency = data["priceCurrency"].Value;
            this.price_2 = data["price_2"].AsInt;
            this.priceCurrency_2 = data["priceCurrency_2"].Value;

        }

        public static List<AreaInfo> ParseToList(string jsonString)
        {
            List<AreaInfo> areaInfos = new List<AreaInfo>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new AreaInfo();
                item.ParseFromJSONObject(itemJson);
                areaInfos.Add(item);
            }

            return areaInfos;

        }

        public static IEnumerator GetAreaInfo(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/areaInfo"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}