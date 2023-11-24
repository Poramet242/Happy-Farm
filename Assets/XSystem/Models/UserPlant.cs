using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class UserPlant : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string plantID;
        public string userID;
        public DateTime plantedTime;
        public DateTime timeStamp;
        public string seedID;
        public int totalExp;
        public int level;
        public int pendingReward;
        public int pendingRewardNFT;
        public int pendingExp;
        public bool isActive;

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
            this.plantID = data["plantID"].Value;
            this.userID = data["userID"].Value;
            this.plantedTime = Utility.ParseDatetime(data["plantedTime"].Value);
            this.timeStamp = Utility.ParseDatetime(data["timeStamp"].Value);
            this.seedID = data["seedID"].Value;
            this.totalExp = data["totalExp"].AsInt;
            this.level = data["level"].AsInt;
            this.pendingReward = data["pendingReward"].AsInt;
            this.pendingRewardNFT = data["pendingRewardNFT"].AsInt;
            this.pendingExp = data["pendingExp"].AsInt;
            this.isActive = data["isActive"].AsBool;

        }

        public static List<UserPlant> ParseToList(string jsonString)
        {
            List<UserPlant> userPlants = new List<UserPlant>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UserPlant();
                item.ParseFromJSONObject(itemJson);
                userPlants.Add(item);
            }

            return userPlants;

        }

        public static IEnumerator GetAllPlantProgress(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/plantProgress/all"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetPlantProgressByArea(XCore xcoreInst,string area, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/plantProgress/byArea?area="+area),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator GetPlantProgressByBlock(XCore xcoreInst,string area,string blockID, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<UserPlant>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/plantProgress?area="+area+"&blockID="+blockID),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

    }
}