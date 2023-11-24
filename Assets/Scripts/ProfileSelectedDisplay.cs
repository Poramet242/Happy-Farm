using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelectedDisplay : MonoBehaviour
{
    public static ProfileSelectedDisplay instance;
    private void Awake()
    {
        if (instance)
        {
            instance = this;
        }
    }
    [SerializeField] public bool isLock;
    [SerializeField] private Image _Character_img;
    [SerializeField] private GameObject thislock;
    private Sprite spritetemp;
    //[SerializeField] private AssisstantDetail Detail;
    [SerializeField] public int num;
    [SerializeField] private string ImgaeID;
    [SerializeField] public Toggle imagesProfileToggle;

    /*public void SetData(AssisstantDetail unitDetail)
    {
        Detail = unitDetail;
        _Character_img.sprite = Detail._unitLocalImage;
    }*/

    public void SetData(Sprite unit,bool checkget)
    {
        //Detail = unitDetail;
        isLock = checkget;
        ImgaeID = unit.name;
        _Character_img.sprite = unit;
    }
    private void Update()
    {
        if (isLock)
        {
            //TODO: lock this images
            thislock.SetActive(true);
        }
        else
        {
            //TODO: unlock this images
            thislock.SetActive(false);
        }
    }

    public void SetImagesProfile()
    {
        if (isLock)
        {
            imagesProfileToggle.graphic.gameObject.SetActive(true);
            ProfileLayerController.instance.checkSelectedImages = true;
            return;
        }
        else
        {
            SoundListObject.instance.OnclickSFX(0);
            imagesProfileToggle.graphic.gameObject.SetActive(true);
            ProfileLayerController.instance.checkSelectedImages = false;
            ProfileLayerController.instance._selectedSprite = _Character_img.sprite;
        }
    }
    private void OnDestroy() => Dispose();
    private void Dispose() => UnityEngine.Object.Destroy(spritetemp);
}
