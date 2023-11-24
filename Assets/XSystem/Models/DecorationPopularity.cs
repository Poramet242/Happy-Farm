using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class DecorationPopularity : BaseWSResponse
    {
        public int popularity;


        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            var data = jObj["data"].AsObject;
           
            this.popularity = data["entities"].AsInt;

        }

         public static IEnumerator GetPopularity(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<DecorationPopularity>(
            apiPath: Uri.EscapeUriString("/api/v1/decoration/point"),
            headers: null,
            callback: callback,
            apiTrackCode: -1);

        }
    }
}