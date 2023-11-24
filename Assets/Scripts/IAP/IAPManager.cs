using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using XSystem;

public class IAPManager : MonoBehaviour
{
    private string buy1000Coin = "com.srk.happycannabisfarm.buy1000coin";
    private string buy3500Coin = "com.srk.happycannabisfarm.buy3500coin";
    private string buy6500Coin = "com.srk.happycannabisfarm.buy6500coin";
    private string buy18000Coin = "com.srk.happycannabisfarm.buy18000coin";
    private string buy60000Coin = "com.srk.happycannabisfarm.buy60000coin";
    //IOS it's have button Restore
    //public GameObject hideButton;
    private void Awake()
    {
        /*if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            hideButton.SetActive(true);
        }*/
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == buy1000Coin)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(setIAPManager(product.definition.id,product.receipt));
            //Debug.Log("You get 500 coin");
        }
        if (product.definition.id == buy3500Coin)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(setIAPManager(product.definition.id, product.receipt));
            //Debug.Log("You get 1000 coin");
        }
        if (product.definition.id == buy6500Coin)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(setIAPManager(product.definition.id, product.receipt));
            //Debug.Log("You get 1500 coin");
        }
        if (product.definition.id == buy18000Coin)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(setIAPManager(product.definition.id, product.receipt));
            //Debug.Log("You get 2500 coin");
        }        
        if (product.definition.id == buy60000Coin)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(setIAPManager(product.definition.id, product.receipt));
            //Debug.Log("You get 2500 coin");
        }
    }
    public void OnPurchaseFaild(Product product,PurchaseFailureReason purchaseFailure)
    {
        Debug.Log(product.definition.id + "Failed Because" + purchaseFailure);
        /*if (purchaseFailure == PurchaseFailureReason.DuplicateTransaction)
        {
            if (product.hasReceipt)
            {
                if (product.definition.id == buySlot99THB)
                {
                    //acction gameplay.....
                }
            }
        }*/
    }
    IEnumerator setIAPManager(string productID,string receipt)
    {
        IWSResponse response = null;
        yield return IAPItem.BuyIAP(XCoreManager.instance.mXCoreInstance, productID, receipt, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        yield return PlayerObject.instance.GetWalletPlayer();
        SoundListObject.instance.OnclickSFX(1);
    }
}
