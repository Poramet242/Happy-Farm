using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class UserBlock : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string userID;
        public string blockID;
        // zone type 
        public ZoneType area;
        public bool isPlanted;
        public string currentPlant;
        public string currentPlantID;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            Debug.Log(jObj.ToString());
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
            var zoneArer = data["area"].Value;
            switch (zoneArer)
            {
                case "zone1":
                    area = ZoneType.Garage;
                    break;
                case "zone2":
                    area = ZoneType.BasketBall;
                    break;
                case "zone3":
                    area = ZoneType.BoxingStadium;
                    break;
            }
            //this.area = data["area"].Value;
            this.isPlanted = data["isPlanted"].AsBool;
            this.currentPlant = data["currentPlant"].Value;
            this.currentPlantID = data["currentPlantID"].Value;

        }

        public static List<UserBlock> ParseToList(string jsonString)
        {
            List<UserBlock> userBlocks = new List<UserBlock>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserBlock();
                item.ParseFromJSONObject(itemJson);
                userBlocks.Add(item);
            }

            return userBlocks;

        }

        public static IEnumerator GetUserBlock(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userBlock"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetUserBlockByArea(XCore xcoreInst, string area, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userBlock/byArea?area=" + area),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}