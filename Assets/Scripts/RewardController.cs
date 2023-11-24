using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    public static RewardController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Token NFT")]
    [SerializeField] public Text _tokenNFT;
    [SerializeField] public Button _tokenNFT_btn;
    [Header("Coine")]
    [SerializeField] public Text _coine;
    [SerializeField] public Button _coine_btn;
    private void Update()
    {
       _tokenNFT.text = PlayerObject.instance._tokenNFTReward.ToString("#,##0.####");
       _coine.text = PlayerObject.instance._coineReward.ToString("#,##0.####");
    }

}
