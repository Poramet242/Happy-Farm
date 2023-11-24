using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CannabisFarm.Models;
using UnityEngine;

namespace XSystem.Test
{
    public class TestAPICall : MonoBehaviour
    {
        XCore mXCoreInstance;

        private void Awake()
        {
            Application.runInBackground = true;
            mXCoreInstance = XCore.FromConfig(XAPIConfig.New(
                host: "http://localhost",
                port: 1198,
                version: "0.0.1",
                versionCode: 1));
            XUnityDispatcher.Initialize();
        }

        // Start is called before the first frame update
        IEnumerator Start()
        {
            IWSResponse response = null;

            ////////////////////USE THIS PART///////////////////////////

            yield return XUser.RestoreSession(mXCoreInstance, "pdiHOYi8eE8kiv9j",
                (r) =>
                {
                    response = r;
                });
            if (response.Success() == false)
            {
                Debug.LogErrorFormat("cannot restore login with session token due to error: {0}", response.ErrorsString());
                Debug.Log(response.RawResult().ToString());
                yield break;
            }

            yield return AutoHarvestAPI.AutoHarvest(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            Debug.Log(response.RawResult().ToString());
            var d = response as AutoHarvestAPI;
            Debug.Log(d.totalCoin);

           /*  yield return DecorationPopularity.GetPopularity(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            Debug.Log(response.RawResult().ToString());
            var count = response as DecorationPopularity;
            Debug.Log(count.popularity);

            yield return WalletResp.WithdrawToken(mXCoreInstance,12, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }


            yield return SeedBuyCount.GetSeedBuyCount(mXCoreInstance, "plant_20", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             Debug.Log(response.RawResult().ToString());
             var count = response as SeedBuyCount;
             Debug.Log(count.count);

              yield return Account.SetTutorialPlayed(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             yield return NextLevelExp.GetNextLevelExp(mXCoreInstance,2, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             Debug.Log(response.RawResult().ToString());
             var levelData = response as NextLevelExp;
             Debug.Log(levelData.currentLevelExp); 
             Debug.Log(levelData.nextLevelExp); 

             yield return UnlockBlockReward.GetUnlockBlockReward(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             List<UnlockBlockReward> items = UnlockBlockReward.ParseToList(response.RawResult().ToString());
             Debug.Log(items[0].plantID);

             yield return GameAPI.UnlockBlock(mXCoreInstance,"zone1","block2", (r) => response = r);
              if (!response.Success())
             {
                 Debug.Log(response.RawResult().ToString());
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             List<UnlockBlockReward> items2 = UnlockBlockReward.ParseToList(response.RawResult().ToString());

             Debug.Log(items2[0].plantID);
             Debug.Log(items2[0].amount);

             yield return Decoration.GetDecorationItems(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             List<Decoration> items = Decoration.ParseToList(response.RawResult().ToString());
             Debug.Log(items[0].itemID);

             yield return Decoration.BuyDecorationItem(mXCoreInstance,"game_2", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

              yield return Decoration.SetDecorationItem(mXCoreInstance,"game_2", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             yield return GameVersion.GetGameVersion(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             var res = response as GameVersion;
             Debug.Log(res.requiredVersionAndroid);
             Debug.Log(Application.version);

             yield return Assistant.GetAssistants(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             List<Assistant> items = Assistant.ParseToList(response.RawResult().ToString());
             Debug.Log(items[0].tokenID);

             yield return Assistant.SetAssistantArea(mXCoreInstance,2,"zone1", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }


             yield return Account.SetNamePaid(mXCoreInstance,"tester abc", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             var res2 = response as Account;
             Debug.Log(res2.displayName);

             yield return UserProfile.GetUserProfile(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             var user = response as UserProfile;
             Debug.Log(user.displayName);

             yield return WalletResp.GetWallet(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             var wallet = response as WalletResp;
             Debug.Log(wallet.coin);

             yield return UserBlock.GetUserBlock(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             Debug.Log(response.RawResult().ToString());

             List<UserBlock> blocks = UserBlock.ParseToList(response.RawResult().ToString());
             Debug.Log(blocks[0].blockID);

             yield return BlockInfo.GetBlockInfo(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             Debug.Log(response.RawResult().ToString());

             List<BlockInfo> blockInfos = BlockInfo.ParseToList(response.RawResult().ToString());
             Debug.Log(blocks[0].blockID);

             yield return UserSeed.GetUserSeed(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             Debug.Log(response.RawResult().ToString());

             List<UserSeed> seeds = UserSeed.ParseToList(response.RawResult().ToString());
             Debug.Log(seeds[0].plantID);

             yield return GachaAPI.DrawGacha(mXCoreInstance, "test", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             var gachaResult = response as UserSeed;
             Debug.Log(gachaResult.plantID);

             yield return GachaAPI.DrawGachaX10(mXCoreInstance, "test", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             Debug.Log(response.RawResult().ToString());

             List<UserSeed> gachaSeeds = UserSeed.ParseToList(response.RawResult().ToString());
             Debug.Log(seeds[0].plantID);


             yield return GachaAPI.GetGachaList(mXCoreInstance, (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }

             Debug.Log(response.RawResult().ToString());

             List<GachaAPI> gachaInfos = GachaAPI.ParseToList(response.RawResult().ToString());
             Debug.Log(gachaInfos[0].gachaRates[0].idList[0]);

                         yield return RoninResp.GetAllRonins(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<RoninResp> ronins = RoninResp.ParseRonins(response.RawResult().ToString());
                         Debug.Log(ronins[0].tokenID);

                         yield return UserProfile.SetName(mXCoreInstance, "Ronin01", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }


                         yield return TeamResp.GetTeam(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         var team = response as TeamResp;
                         Debug.Log(team.followers[0].tokenAddress);


                         var leader = new RoninIDInfo(1,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         var follower_1 = new RoninIDInfo(2,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         var follower_2 = new RoninIDInfo(3,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         RoninIDInfo[] newTeam = { follower_1, follower_2 };
                         int[] position = {2,3};
                         //Debug.Log("[\"" + string.Join("\",\"", newTeam) + "\"]");
                         yield return TeamResp.SetTeam(mXCoreInstance, leader, newTeam,position,FormationType.Spearhead, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }


                         yield return LocationResp.GetLocation(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         var location = response as LocationResp;
                         Debug.Log(location.region);

                         yield return LocationResp.SetLocation(mXCoreInstance, 1,1 , (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         yield return GameAPI.SetStartGame(mXCoreInstance, "1-1", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r = response as GameAPI;
                         Debug.Log("Start Success " + r.isSuccess);

                         yield return GameAPI.SetEndGame(mXCoreInstance, "1-1",3, true, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         r = response as GameAPI;
                         Debug.Log("End Success " + r.isSuccess+ "\nEXP : "+r.exp+"\nReward : "+r.reward);

                         yield return GameAPI.GetLatestLevel(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         r = response as GameAPI;
                         Debug.Log("latestLevel " + r.latestLevel);

                         yield return LandResp.GetAllLands(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<LandResp> lands = LandResp.ParseLands(response.RawResult().ToString());
                         Debug.Log(lands[0].tokenID);

                         yield return GameProgress.GetProgress(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<GameProgress> progresses = GameProgress.ParseProgresses(response.RawResult().ToString());
                         Debug.Log(progresses[0].levelID);

                          yield return GameProgress.SetProgress(mXCoreInstance, "1-8",3, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r = response as BaseWSResponse;
                         Debug.Log(r.success);

                         yield return ItemResp.GetAllItems(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<ItemResp> items = ItemResp.ParseItems(response.RawResult().ToString());
                         Debug.Log(items[0].itemID);

                         yield return ItemResp.GetInventory(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<ItemResp> items = ItemResp.ParseItems(response.RawResult().ToString());
                         Debug.Log(items[0].itemID);

                         yield return ItemResp.BuyItem(mXCoreInstance, "c-01", 1, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         yield return CustomValueResp.GetValue(mXCoreInstance,"test", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         var user = response as CustomValueResp;
                         Debug.Log(user.value);    

                         //var leader = new RoninIDInfo(1,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         var follower_1 = new RoninIDInfo(2,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         var follower_2 = new RoninIDInfo(3,"0x3A3DA9E7D77815d4f79E0910A5F580dA3e9162CA");
                         RoninIDInfo[] newTeam = { follower_1, follower_2 };


                         yield return StakeRecord.RecordRoninStake(mXCoreInstance, follower_1, "1", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r1 = response as StakeRecord;
                         Debug.Log("RecordRoninStake " + r1.isStaked);
                         yield return new WaitForSeconds(3f);

                         yield return StakeRecord.RecordRoninUnstake(mXCoreInstance, follower_1, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r2 = response as StakeRecord;
                         Debug.Log("RecordRoninUnstake " + r2.isStaked);
                         yield return new WaitForSeconds(3f);

                         yield return StakeRecord.RecordLandStake(mXCoreInstance, "1", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r3 = response as StakeRecord;
                         Debug.Log("RecordLandStake " + r3.isStaked);

                         yield return new WaitForSeconds(3f);

                         yield return StakeRecord.RecordLandUnstake(mXCoreInstance, "1", (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }
                         var r4 = response as StakeRecord;
                         Debug.Log("RecordLandUnStake " + r4.isStaked);*/

            /* yield return StakeRecord.GetRoninStakeRecord(mXCoreInstance, "1", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var r5 = response as StakeRecord;
             Debug.Log("RecordLandUnStake " + r5.isStaked);

             yield return StakeRecord.GetLandStakeRecord(mXCoreInstance, "1", (r) => response = r);
             if (!response.Success())
             {
                 Debug.LogError(response.ErrorsString());
                 yield break;
             }
             var r6 = response as StakeRecord;
             Debug.Log("RecordLandUnStake " + r6.isStaked);8/

             /*
                          yield return Annoucement.GetAnnoucement(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<Annoucement> annoucements = Annoucement.ParseAnnoucements(response.RawResult().ToString());
                         Debug.Log(annoucements[0].image);

                          yield return News.GetNews(mXCoreInstance,0,5, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         Debug.Log(response.RawResult().ToString());

                         List<News> annoucements = News.ParseNews(response.RawResult().ToString());
                         Debug.Log(annoucements[0].topic);

                         yield return CoinAPI.GetCoin(mXCoreInstance, (r) => response = r);
                         if (!response.Success())
                         {
                             Debug.LogError(response.ErrorsString());
                             yield break;
                         }

                         var r = response as CoinAPI;

                         Debug.Log(r.coin);*/

            /*yield return StakePositionResp.GetStakePosition(mXCoreInstance, (r) => response = r);
           if (!response.Success())
           {
               Debug.LogError(response.ErrorsString());
               yield break;
           }

           var stakePosition = response as StakePositionResp;
           Debug.Log(stakePosition.positions[0]);


           string[] newTeam = { "606", "609" };
           int[] position = {2,3};
           Debug.Log("[\"" + string.Join("\",\"", newTeam) + "\"]");
           yield return StakePositionResp.SetStakePosition(mXCoreInstance, newTeam,position, (r) => response = r);
           if (!response.Success())
           {
               Debug.LogError(response.ErrorsString());
               yield break;
           }

            yield return GameAPI.GetLatestLevel(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            var r = response as GameAPI;
            Debug.Log("latestLevel " + r.latestLevel);


            //Time
            yield return GameAPI.GetCurrentTime(mXCoreInstance, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            r = response as GameAPI;
            Debug.Log("currentTime " + r.now);


            string[] newTeam = { "8", "9" };
            int[] position = {2,3};
            Debug.Log("[\"" + string.Join("\",\"", newTeam) + "\"]");
            yield return TeamResp.SetTeam(mXCoreInstance, "7", newTeam,position,FormationType.Spearhead, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }



            FixStakeResponse r = FixStakeResponse.ParseResp(response.RawResult().ToString());
            Debug.Log(response.RawResult().ToString());
            Debug.Log(r.isSuccess);
            Debug.Log(r.toStake.Count);
            Debug.Log(r.toUnstake.Count);*/




            ////////////////////END///////////////////////////


        }
    }
}