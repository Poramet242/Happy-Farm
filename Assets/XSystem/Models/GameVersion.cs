using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class GameVersion : BaseWSResponse
    {
        public int requiredVersionAndroid;
        public int requiredVersionIOS;

        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
            requiredVersionAndroid = data["requiredVersionAndroid"].AsInt;
            requiredVersionIOS = data["requiredVersionIOS"].AsInt;

        }

        public static IEnumerator GetGameVersion(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<GameVersion>(
            apiPath: Uri.EscapeUriString("/api/v1/version"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }

}