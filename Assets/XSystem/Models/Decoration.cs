using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    [System.Serializable]
    public class Decoration : BaseWSResponse
    {
        public string itemID;
        public FurnitureType itemType;
        //rarity
        public RarityType rarityType;
        public string imageID;
        public int price;
        public string priceCurrency;
        public bool isUnlocked;
        public bool isUsed;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.itemID = data["itemID"].Value;
            this.itemType = (FurnitureType)data["itemType"].AsInt;
            //rarity
            this.rarityType = (RarityType)data["rarity"].AsInt;
            this.imageID = data["imageID"].Value;
            this.price = data["price"].AsInt;
            this.priceCurrency = data["priceCurrency"].Value;
            this.isUnlocked = data["isUnlocked"].AsBool;
            this.isUsed = data["isUsed"].AsBool;

        }

        public static List<Decoration> ParseToList(string jsonString)
        {
            List<Decoration> decorations = new List<Decoration>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Decoration();
                item.ParseFromJSONObject(itemJson);
                decorations.Add(item);
            }

            return decorations;

        }

        public static IEnumerator GetDecorationItems(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/decoration/items/all"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator BuyDecorationItem(XCore xcoreInst,string itemID,Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("itemID", itemID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/decoration/buy"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator SetDecorationItem(XCore xcoreInst,string itemID,Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("itemID", itemID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/decoration/set"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

    }
}
