using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using XSystem;

public class Slot_IAPManager : MonoBehaviour
{
    public int index;
    public string zoneIndex;
    private string _productID_slot;
    [SerializeField] private IAPButton _button;
    [SerializeField] private GameObject _IAPObject;
    //IOS it's have button Restore
    //public GameObject hideButton;
    private void Awake()
    {
        /*if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            hideButton.SetActive(true);
        }*/
    }
    public void setUpProductID(int numIndex,string zoneIndex)
    {
        _productID_slot = "com.srk.happycannabisfarm.buyslot_zone" + zoneIndex + "_block_" + numIndex + "_99thb";
        _button.productId = _productID_slot;
        Debug.Log("ProductID: " + _productID_slot);
        _IAPObject.SetActive(true);
    }
    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == _productID_slot)
        {
            Debug.Log("Receipt: " + product.receipt);
            StartCoroutine(StakeLayerController.instance.setIAPManager(product.definition.id, product.receipt));
        }
    }
    public void OnPurchaseFaild(Product product, PurchaseFailureReason purchaseFailure)
    {
        Debug.Log(product.definition.id + "Failed Because" + purchaseFailure);
        if (purchaseFailure == PurchaseFailureReason.DuplicateTransaction)
        {
            if (product.hasReceipt)
            {
                if (product.definition.id == _productID_slot)
                {

                }
            }
        }
    }
}
