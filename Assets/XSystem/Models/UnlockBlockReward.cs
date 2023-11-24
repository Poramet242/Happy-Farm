using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class UnlockBlockReward : BaseWSResponse
    {
        public string blockID;
        public string areaID;
        public string plantID;
        public int amount;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.blockID = data["blockID"].Value;
            this.areaID = data["areaID"].Value;
            this.plantID = data["plantID"].Value;
            this.amount = data["amount"].AsInt;
        }

        public static List<UnlockBlockReward> ParseToList(string jsonString)
        {
            List<UnlockBlockReward> unlockBlockRewards = new List<UnlockBlockReward>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new UnlockBlockReward();
                item.ParseFromJSONObject(itemJson);
                unlockBlockRewards.Add(item);
            }

            return unlockBlockRewards;

        }

        public static IEnumerator GetUnlockBlockReward(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/blockUnlockReward"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}