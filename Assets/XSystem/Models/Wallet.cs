using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class WalletResp : BaseWSResponse
    {
        public int coin;
        public int gem;
        public override void ParseFromJSONObject(JSONObject jObj)
        {
            base.ParseFromJSONObject(jObj);
            Debug.Log(jObj.ToString());

            var data = jObj["data"].AsObject;
            var entities = data["entities"].AsObject;
            this.coin = entities["coin"].AsInt;
            this.gem = entities["gem"].AsInt;

        }

        public static IEnumerator GetWallet(XCore xcoreInst, Action<IWSResponse> callback)
        {
            yield return xcoreInst.GET<WalletResp>(
                apiPath: Uri.EscapeUriString("/api/v1/wallet"),
                headers: null,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator WithdrawToken(XCore xcoreInst,int amount,Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("amount", amount);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/smartcontract/withdrawtoken"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

    }

}
