using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    [System.Serializable]
    public class PlantInfo : BaseWSResponse
    {
        public string id;
        public DateTime createdOn;
        public DateTime updatedOn;
        public string plantID;
        public string name;
        public RarityType rarity;
        //species
        public SpeciesType species;
        //document 
        public string documentPlant;
        //-------------------------
        public bool canBuy;
        public int buyPrice;
        public string buyPriceType;
        public int buyLimitPerMonth;
        public bool canSell;
        public int sellPrice;
        public string sellPriceType;
        public int growTime;
        public int timePerCoin;
        public int harvestExp;
        public int rewardAmount;
        //rewardAmount NFT
        public float rewardAmountNFT;
        //-------------------------
        public string rewardType;
        public int decayTime;
        public int growStack_1;
        public int growStack_5;
        public int growStack_10;


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
            this.name = data["name"].Value;
            this.rarity = (RarityType)data["rarity"].AsInt;
            this.species = (SpeciesType)data["species"].AsInt;
            this.documentPlant = data["document"].Value;
            this.canBuy = data["canBuy"].AsBool;
            this.buyPrice = data["buyPrice"].AsInt;
            this.buyPriceType = data["buyPriceType"].Value;
            this.buyLimitPerMonth = data["buyLimitPerMonth"].AsInt;
            this.canSell = data["canSell"].AsBool;
            this.sellPrice = data["sellPrice"].AsInt;
            this.sellPriceType = data["sellPriceType"].Value;
            this.growTime = data["growTime"].AsInt;
            this.timePerCoin = data["timePerCoin"].AsInt;
            this.harvestExp = data["harvestExp "].AsInt;
            this.rewardAmount = data["rewardAmount"].AsInt;
            this.rewardAmountNFT = data["rewardAmountNFT"].AsFloat;
            this.rewardType = data["rewardType"].Value;
            this.decayTime = data["decayTime"].AsInt;
            this.growStack_1 = data["growStack_1"].AsInt;
            this.growStack_5 = data["growStack_5"].AsInt;
            this.growStack_10 = data["growStack_10"].AsInt;
           
        }

        public static List<PlantInfo> ParseToList(string jsonString)
        {
            List<PlantInfo> plantInfos = new List<PlantInfo>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new PlantInfo();
                item.ParseFromJSONObject(itemJson);
                plantInfos.Add(item);
            }

            return plantInfos;

        }

        public static IEnumerator GetPlantInfo(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/plantInfo"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }

        public static IEnumerator GetPlantInfoByArea(XCore xcoreInst, string area, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/plantInfo/byArea?area=" + area),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}