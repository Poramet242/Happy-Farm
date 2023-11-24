using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class WithdrawalController : MonoBehaviour
{
    [SerializeField] private InputField withdrawal_field;
    [SerializeField] private Text current_Amount_text;
    [SerializeField] private GameObject Pn_current;
    [SerializeField] private GameObject withdrawal_c;
    [SerializeField] private Text current_Amount_text_C;
    [SerializeField] private GameObject Pn_current_c;
    [SerializeField] private GameObject faceback;
    [SerializeField] private WarningUi warningUi;
    [SerializeField] private int count_input;

    [Header("Warning Mate Mask")]
    [SerializeField] private GameObject warning_text;
    [SerializeField] private Button Withdrawal;

    public void setup()
    {
        SoundListObject.instance.OnclickSFX(0);
        if (string.IsNullOrEmpty(PlayerObject.instance._address))
        {
            warning_text.SetActive(true);
            Withdrawal.interactable = false;
        }
        else
        {
            warning_text.SetActive(false);
            Withdrawal.interactable = true;
        }
        this.gameObject.SetActive(true);
        onclickOpneThisUi();
        current_Amount_text.text = PlayerObject.instance._tokenNFTReward.ToString("#,##0");
        withdrawal_field.text = string.Empty;
    }
    public void onClickWithdrawal()
    {
        SoundListObject.instance.OnclickSFX(0);
        int.TryParse(withdrawal_field.text, out count_input);
        if (count_input > PlayerObject.instance._tokenNFTReward || count_input <= 0)
        {
            warningUi._thisObject.SetActive(true);
            warningUi._innfo_txt.text = "Please enter the correct amount. !!!";
            return;
        }
        withdrawal_c.SetActive(true);
        current_Amount_text_C.text = count_input.ToString("#,##0");
    }
    public void onclickCloseThisUi()
    {
        StakeLayerController.instance.CloseUiLayerGameplay();
    }
    public void onclickOpneThisUi()
    {
        StakeLayerController.instance.OpenUiLayerGameplay();
    }
    public void onClickConfirmWithdrawal()
    {
        SoundListObject.instance.OnclickSFX(0);
        StartCoroutine(setCurrentAmount());
    }
    IEnumerator setCurrentAmount()
    {
        withdrawal_c.SetActive(false);
        faceback.SetActive(true);
        IWSResponse  response = null;
        yield return WalletResp.WithdrawToken(XCoreManager.instance.mXCoreInstance, count_input, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log(response.RawResult().ToString());
            yield break;
        }
        yield return new WaitForSeconds(3f);
        yield return PlayerObject.instance.GetWalletPlayer();
        current_Amount_text.text = PlayerObject.instance._tokenNFTReward.ToString("#,##0");
        faceback.SetActive(false);
        withdrawal_field.text = string.Empty;
    }
}
