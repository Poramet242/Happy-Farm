using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class SeedBuyCount : BaseWSResponse
    {
        public int count;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            this.count = data["entities"].AsInt;

        }

        public static IEnumerator GetSeedBuyCount(XCore xcoreInst,string plantID, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<SeedBuyCount>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/seedBuyCount?plantID="+plantID),
            headers: null,
            callback: callback,
            apiTrackCode: -1);
        }
    }
}