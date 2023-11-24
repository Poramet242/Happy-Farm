using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class AutoHarvestAPI : BaseWSResponse
    {
        
        public List<AutoHarvestData> autoHarvestDatas;
        public int totalCoin;
        public float totalGem;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.totalCoin = data["totalCoin"].AsInt;
            this.totalGem = data["totalGem"].AsFloat;

            this.autoHarvestDatas = new List<AutoHarvestData>();
            var d = data["autoHarvestData"].AsArray;
            for (int i = 0; i <d.Count; i++)
            {
                var itemJson = d[i].AsObject;
                var item = new AutoHarvestData();
                item.ParseFromJSONObject(itemJson);
                autoHarvestDatas.Add(item);
//                Debug.Log(item.areaID);
            }


        }

        public static List<AutoHarvestAPI> ParseToList(string jsonString)
        {
            List<AutoHarvestAPI> gachaAPIs = new List<AutoHarvestAPI>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new AutoHarvestAPI();
                item.ParseFromJSONObject(itemJson);
                gachaAPIs.Add(item);
            }

            return gachaAPIs;

        }
        public static IEnumerator AutoHarvest(XCore xcoreInst, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();

            yield return xcoreInst.POST<AutoHarvestAPI>(
                apiPath: Uri.EscapeUriString("/api/v1/game/autoHarvest"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

    }
}
