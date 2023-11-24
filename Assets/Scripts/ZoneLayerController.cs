using System;
using System.Collections.Generic;
using UnityEngine;

public class ZoneLayerController : MonoBehaviour
{
    public static ZoneLayerController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] private List<Zone> zones = new List<Zone>();


    private void OnEnable()
    {
        StartCoroutine(PlayerObject.instance.GetExpPlayer());
        for (int i = 0; i < zones.Count; i++)
        {
            zones[i].enabled = false;
            for (int j = 0; j < ZoneUnitObject.instance.unitDataZones.Count; j++)
            {
                if (ZoneUnitObject.instance.unitDataZones[j].ZoneType == zones[i].zones)
                {
                    if (ZoneUnitObject.instance.unitDataZones[j]._checkUnlock)
                    {
                        zones[i].checkunLock = true;
                        zones[i].checkIsBuy = true;
                        zones[i].enabled = true;
                        break;
                    }
                }
            }
        }
    }
    private void Start()
    {
        CheckUnlockZone(PlayerObject.instance._levelPlayer);
    }
    public void CheckUnlockZone(int num)
    {
        if (num >= 15 && num < 30)
        {
            zones[1].checkunLock = true;
            zones[1].enabled = true;
        }
        else if (num >= 30)
        {
            zones[1].checkunLock = true;
            zones[1].enabled = true;
            zones[2].checkunLock = true;
            zones[2].enabled = true;
        }
    }
    private void Update()
    {
        for (int i = 0; i < zones.Count; i++)
        {
            if (zones[i].checkunLock)
            {
                zones[i].OpenBuyZone();
            }
            if (zones[i].checkunLock && zones[i].checkIsBuy)
            {
                zones[i].OpenZone();
                zones[i].enabled = true;
            }
        }
    }
    public void onclickClose()
    {
        SoundListObject.instance.OnclickSFX(0);
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        this.gameObject.SetActive(false);
    }
}
