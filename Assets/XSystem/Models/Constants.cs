using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CannabisFarm.Models
{
    public enum RarityType
    {
        None = -1,
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }

    public class GachaRate
    {
        public RarityType rarityType;
        public float rate;
        public List<string> idList;

        public void ParseFromJSONObject(SimpleJSON.JSONObject jObj)
        {
            this.rarityType = (RarityType)jObj["name"].AsInt;
            this.rate = jObj["rate"].AsFloat;
            this.idList = new List<string>();

            var idJson = jObj["idList"].AsArray;
            for (int i = 0; i < idJson.Count; i++)
            {
                this.idList.Add(idJson[i].Value);
            }

        }

    }

    public class AutoHarvestData
    {
        public string id;
        public string areaID;
        public string blockID;
        public int coin;
        public float gem;

        public void ParseFromJSONObject(SimpleJSON.JSONObject jObj)
        {
            this.id = jObj["id"].Value;
            this.areaID = jObj["areaID"].Value;
            this.blockID = jObj["blockID"].Value;
            this.coin = jObj["coin"].AsInt;
            this.gem = jObj["gem"].AsFloat;

        }

    }

    public enum Result
    {
        Success = 0,
        Failed = 1
    }

}
