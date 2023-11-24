using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class Assistant : BaseWSResponse
    {
        public string assistantID;
        public string name;
        public int tokenID;
        public string imageID;
        public string areaID;
        //zone type 
        public ZoneType area;
        //rarity type
        public RarityType rarity;
        public UnitSkill skillType;
        public DateTime timeStamp;
        public DateTime autoHarvestTimeStamp;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            data = data["entities"].AsObject;
            if (data == null || data.ToString() == string.Empty || data.ToString() == "{}")
            {
                data = jObj;
            }
            this.assistantID = data["assistantID"].Value;
            this.name = data["name"].Value;
            this.tokenID = data["tokenID"].AsInt;
            this.imageID = data["imageID"].Value;
            this.areaID = data["areaID"].Value;
            this.rarity = (RarityType)data["rarity"].AsInt;
            this.timeStamp = Utility.ParseDatetime(data["timeStamp"].Value);
            this.autoHarvestTimeStamp = Utility.ParseDatetime(data["autoHarvestTimeStamp"].Value);
            var zoneArer = data["areaID"].Value;
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
                default:
                    area = ZoneType.None;
                    break;
            }
            //this.skillType = (UnitSkill)data["skillTypes"].AsArray;

            var items = data["skillTypes"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].AsInt != 9)
                {
                    var itemJson = items[i].AsInt;
                    this.skillType = (UnitSkill)itemJson;
                }
            }

        }

        public static List<Assistant> ParseToList(string jsonString)
        {
            List<Assistant> assistants = new List<Assistant>();

            var jObj = SimpleJSON.JSON.Parse(jsonString).AsObject;
            var data = jObj["data"].AsObject;

            var items = data["entities"].AsArray;
            for (int i = 0; i < items.Count; i++)
            {
                var itemJson = items[i].AsObject;
                var item = new Assistant();
                item.ParseFromJSONObject(itemJson);
                assistants.Add(item);
            }

            return assistants;

        }

        public static IEnumerator GetAssistants(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<BaseWSResponse>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/userAssistant"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }

        public static IEnumerator SetAssistantArea(XCore xcoreInst,int tokenID,string areaID,Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("tokenID", tokenID);
            formData.AddField("areaID", areaID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/game/setAssistantArea"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }
    }
}
