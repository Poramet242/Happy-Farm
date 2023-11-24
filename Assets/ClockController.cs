using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ClockController : MonoBehaviour
{
    public static ClockController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Assistants Data")]
    [SerializeField] public bool isAutoActive;
    [SerializeField] bool stampTime = false;
    private int lastInstantiatedSecond = -1;
    [SerializeField] public List<AssisstantDetail> assisstantsDisplayList = new List<AssisstantDetail>();
    [Header("Time Data")]
    [SerializeField] DateTime currentTime;
    [SerializeField] DateTime timeServer;
    [SerializeField] DateTime startTime;
    [Header("Stamp Time Auto Legendary")]
    [SerializeField] bool _every6hours_L;
    [SerializeField] bool stampTime_L =false;
    [Header("Stamp Time Auto Epic")]
    [SerializeField] bool _every8houre_E;
    [SerializeField] bool stampTime_E =false;
    [Header("Stamp Time Auto Rare")]
    [SerializeField] bool _every12houre_R;
    [SerializeField] bool stampTime_R =false;
    [Header("Stamp Time Auto common")]
    [SerializeField] bool _every24hours_C;
    [SerializeField] bool stampTime_C =false;
    //[SerializeField] TimeSpan elapsedTime;
    private void Start()
    {
        //startTime = DateTime.Now;
        StartCoroutine(updateClock());
        for (int i = 0; i < assisstantsDisplayList.Count; i++)
        {
            setupStampTimeAuto(assisstantsDisplayList[i]._rarityType, true);
        }
    }
    public void setupStampTimeAuto(RarityType rarity, bool add)
    {
        switch (rarity)
        {
            case RarityType.Common:
                _every24hours_C = add;
                break;
            case RarityType.Rare:
                _every12houre_R = add;
                break;
            case RarityType.Epic:
                _every8houre_E = add;
                break;
            case RarityType.Legendary:
                _every6hours_L = add;
                break;
        }
    }
    #region Auto #1
    private void UpdateClockText()
    {
        //currentTime = timeServer;
        currentTime = DateTime.Now;
        if ((currentTime.Second % 10) == 0)
        {
            GameObject temp = Instantiate(TestUIClock.instance.preNow,TestUIClock.instance.contanNow.transform);
            temp.SetActive(true);
            temp.GetComponent<Text>().text = currentTime.ToString("HH:mm:ss");
        }
        string timeText = currentTime.ToString("HH:mm:ss");
        TestUIClock.instance.timeStamp.text = timeText;
    }
    IEnumerator updateClock()
    {
        //yield return setDateTime();
        while (true)
        {
            UpdateClockText();
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator setDateTime()
    {
        IWSResponse response = null;
        yield return TimeNow.GetTimeNow(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        var time = response as TimeNow;
        timeServer = time.timeNow;
    }
    public void setupAutoHaverToserver(DateTime currentTime)
    {

    }
    #endregion

    #region Auto #2
    private void Update()
    {
       
    }
    public void setUpActiveAutoCommon()
    {
        if (isAutoActive)
        {
            if (!stampTime)
            {
                //DateTime server
                startTime = DateTime.Now;
                stampTime = true;
            }
            TimeSpan elapsedTime = DateTime.Now - startTime;
            string timeText = string.Format("{0:00}:{1:00}:{2:00}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
            TestUIClock.instance.timeNow.text = timeText;
            //
            float elapsedTimeThreshold = 10f;
            //currentSecond = one update to instantiate 
            int currentSecond = elapsedTime.Seconds;
            if (currentSecond != lastInstantiatedSecond && currentSecond % elapsedTimeThreshold == 0)
            {
                // Instantiate your objects here
                GameObject temp = Instantiate(TestUIClock.instance.preStamp, TestUIClock.instance.contanStamp.transform);
                temp.SetActive(true);
                temp.GetComponent<Text>().text = timeText;
                //chage lastInstantiatedSecond = currentSecond to don't update more
                lastInstantiatedSecond = currentSecond;
            }
        }
        else
        {
            stampTime = false;
            lastInstantiatedSecond = -1; // Reset the lastInstantiatedSecond when auto update is deactivated
        }
    }
#endregion
}
