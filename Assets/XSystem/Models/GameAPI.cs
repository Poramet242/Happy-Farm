using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using XSystem;

namespace CannabisFarm.Models
{
    public class GameAPI
    {

        public static IEnumerator Planting(XCore xcoreInst,string plantID,string area,string blockID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("plantID", plantID);
            formData.AddField("area", area);
            formData.AddField("blockID", blockID);

            yield return xcoreInst.POST<UserBlock>(
                apiPath: Uri.EscapeUriString("/api/v1/game/planting"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator RemovePlant(XCore xcoreInst,string area,string blockID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("area", area);
            formData.AddField("blockID", blockID);

            yield return xcoreInst.POST<UserBlock>(
                apiPath: Uri.EscapeUriString("/api/v1/game/removePlant"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator Harvest(XCore xcoreInst,string area,string blockID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("area", area);
            formData.AddField("blockID", blockID);

            yield return xcoreInst.POST<UserPlant>(
                apiPath: Uri.EscapeUriString("/api/v1/game/harvest"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator UnlockBlock(XCore xcoreInst,string area,string blockID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("area", area);
            formData.AddField("blockID", blockID);

            yield return xcoreInst.POST<BaseWSResponse>(
                apiPath: Uri.EscapeUriString("/api/v1/game/unlockBlock"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator SellSeed(XCore xcoreInst,string plantID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("plantID", plantID);
            yield return xcoreInst.POST<WalletResp>(
                apiPath: Uri.EscapeUriString("/api/v1/game/sellSeed"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

        public static IEnumerator BuySeed(XCore xcoreInst,string plantID, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            formData.AddField("plantID", plantID);

            yield return xcoreInst.POST<WalletResp>(
                apiPath: Uri.EscapeUriString("/api/v1/game/buySeed"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

         public static IEnumerator UnlockArea(XCore xcoreInst,string areaID,bool use2ndCurrency, Action<IWSResponse> callback)
        {
            var formData = new WWWForm();
            Debug.Log("Area ID: " + areaID);
            formData.AddField("areaID", areaID);
            formData.AddField("use2ndCurrency", use2ndCurrency.ToString());

            yield return xcoreInst.POST<UserBlock>(
                apiPath: Uri.EscapeUriString("/api/v1/game/unlockArea"),
                headers: null,
                postData: formData,
                callback: callback,
                apiTrackCode: -1);
        }

    }

}
