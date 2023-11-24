using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class PlayerObject : MonoBehaviour
{
    public static PlayerObject instance;
    private void Awake()
    {
        if (instance != null && instance !=this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Player Info")]
    [SerializeField] public bool _checkplayTutorial;
    [SerializeField] public string _playerName = "";
    [SerializeField] public string _address = "";
    [SerializeField] public string _accNo;
    [SerializeField] public LoginTypes _accType;
    [SerializeField] public ZoneType _zone;
    [SerializeField] public bool _checkEditName;
    [Header("Point popular")]
    [SerializeField] public int _popularPoint;
    [Header("Coine")]
    [SerializeField] public int _tokenNFTReward = 0;
    [SerializeField] public int _coineReward = 0;
    [Header("Profile images")]
    [SerializeField] public string _imagesID = "";
    [SerializeField] public Sprite _imagesProfile_spr;
    [SerializeField] public List<Sprite> _allLocalImages;
    [Header("Level player")]
    [SerializeField] public int _levelPlayer = 0;
    [SerializeField] public float max_levelPlayer = 0;
    [SerializeField] public float min_levelPlayer = 0;
    [SerializeField] public float current_levelPlayer = 0;

    //------------------------------------------
    [Header("Date Time Count")]
    [SerializeField] public DateTime IsNowServerDateTime;

    [SerializeField] public double dateTimeServer;
    [SerializeField] public double dateTimecooldown;
    public IEnumerator GetExpPlayer()
    {
        IWSResponse response = null;
        yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        var user = response as Account;
        //exp
        _levelPlayer = user.level;
        current_levelPlayer = user.exp;
        yield return NextLevelExp.GetNextLevelExp(XCoreManager.instance.mXCoreInstance, _levelPlayer, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var levelData = response as NextLevelExp;
        max_levelPlayer = levelData.nextLevelExp;
        min_levelPlayer = levelData.currentLevelExp;
        ProfileLayerController.instance.updateLevelTogameplay();
    }
    public IEnumerator GetWalletPlayer()
    {
        IWSResponse response = null;
        yield return WalletResp.GetWallet(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        var wallet = response as WalletResp;
        _coineReward = wallet.coin;
        _tokenNFTReward = wallet.gem;
    }

    public void ClearPlayerObject()
    {
        Destroy(this.gameObject);
    }
}
