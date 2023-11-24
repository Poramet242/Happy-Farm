using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class NextLevelExp : BaseWSResponse
    {
        public int currentLevelExp;
        public int nextLevelExp;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
           
            this.currentLevelExp = data["currentLevelExp"].AsInt;
            this.nextLevelExp = data["nextLevelExp"].AsInt;

        }

         public static IEnumerator GetNextLevelExp(XCore xcoreInst,int level, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<NextLevelExp>(
            apiPath: Uri.EscapeUriString("/api/v1/gameData/nextLevelExp?level="+level),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}