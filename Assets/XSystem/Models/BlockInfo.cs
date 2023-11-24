using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    [System.Serializable]
    public class BlockInfo : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string blockID;
        public string area;
        //Block price
        public int price;
        public bool isPlanted;
        public string currentPlant;
        public string currentPlantID;


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
            this.userID = data["userID"].Value;
            this.blockID = data["blockID"].Value;
            this.area = data["area"].Value;
            this.price = data["price"].AsInt;
            this.isPlanted = data["isPlanted"].AsBool;
            this.currentPlant = data["currentPlant"].Value;
            this.currentPlantID = data["currentPlantID"].Value;

        }

        public static List<BlockInfo> ParseToList(string jsonString)
        {
            List<BlockInfo> blockInfos = new List<BlockInfo>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new BlockInfo();
                item.ParseFromJSONObject(itemJson);
                blockInfos.Add(item);
            }

            return blockInfos;

        }

        public static IEnumerator GetBlockInfo(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/blockInfo"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}