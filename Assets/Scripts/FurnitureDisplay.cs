using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class FurnitureDisplay : MonoBehaviour
{
    [SerializeField] public bool isUseFurniture;
    [SerializeField] public bool isBuyFurniture;

    [SerializeField] public FurnitureDetail thisfurnitureDetail;
    [SerializeField] public Image iconFurniture;
    [SerializeField] public Text priceFurniture;

    [SerializeField] public Button buy_btn;
    [SerializeField] public Button use_btn;

    [SerializeField] public GameObject lock_image;
    [SerializeField] public Sprite toUse_spr;
    [SerializeField] public Sprite unUse_spr;

    [SerializeField] public GameObject _select_obj;
    [SerializeField] public Color _selected_img;
    [SerializeField] public Color old_Selected_img;
    public void setupFurnitureDisplay(FurnitureDetail detail)
    {
        thisfurnitureDetail = detail;
        iconFurniture.sprite = detail.localImages;

        isUseFurniture = detail.isUseFurniture;
        isBuyFurniture = detail.isBuyFurniture;

        isBuyThisFurniture(detail.isUseFurniture);

        isUseThisFurniture(detail.isBuyFurniture);
    }
    private void Update()
    {
        buy_btn.enabled = false;
        priceFurniture.text = thisfurnitureDetail.unitPrice.ToString("#,##0");
        //priceFurniture.text = "0";
    }
    public void isUseThisFurniture(bool checkUse)
    {
        if (checkUse)
        {
            use_btn.enabled = false;
            use_btn.gameObject.GetComponent<Image>().sprite = toUse_spr;
            _select_obj.GetComponent<Image>().color = _selected_img;
        }
        else
        {
            use_btn.enabled = true;
            use_btn.gameObject.GetComponent<Image>().sprite = unUse_spr;
            _select_obj.GetComponent<Image>().color = old_Selected_img;
        }
    }
    public void isBuyThisFurniture(bool checkBuy)
    {
        if (checkBuy)
        {
            use_btn.gameObject.SetActive(true);
            buy_btn.gameObject.SetActive(false);
            lock_image.SetActive(false);
            use_btn.enabled = true;
        }
        else
        {
            buy_btn.gameObject.SetActive(true);
            lock_image.SetActive(true);
            use_btn.gameObject.SetActive(false);
        }
    }
    public void onClickBuy()
    {
        StartCoroutine(setBuyFurniture(thisfurnitureDetail));
    }
    IEnumerator setBuyFurniture(FurnitureDetail furniture)
    {
        IWSResponse response = null;
        yield return Decoration.BuyDecorationItem(XCoreManager.instance.mXCoreInstance, furniture.unitID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            isBuyThisFurniture(false);
            furniture.isBuyFurniture = false;
            yield break;
        }
        yield return PlayerObject.instance.GetWalletPlayer();
        isBuyThisFurniture(true);
        furniture.isBuyFurniture = true;
    }
    public void onClickUes()
    {
        StartCoroutine(setUseFurniture(thisfurnitureDetail));
    }
    IEnumerator setUseFurniture(FurnitureDetail furniture)
    {
        IWSResponse response = null;
        yield return Decoration.SetDecorationItem(XCoreManager.instance.mXCoreInstance, furniture.unitID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            isUseThisFurniture(false);
            furniture.isUseFurniture = false;
            yield break;
        }
        isUseThisFurniture(true);
        furniture.isUseFurniture = true;
        StakeLayerController.instance.setfurnitureInGamePlay(furniture);
        StakeLayerController.instance.CloseUiLayerGameplay();
        FurnitureShop.instance.onClickClose();
    }
}
