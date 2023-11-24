using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testTime : MonoBehaviour
{
    [SerializeField] Text x;
    [SerializeField] TimeSpan coldowTime;
    [SerializeField] double y;
    private void Start()
    {
        DateTime today = DateTime.Today;
        DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
        coldowTime = endOfMonth - today;
        y = coldowTime.TotalSeconds;
    }
    private void Update()
    {
        y -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(y);
        x.text = ((timeSpan.Days * 24) + timeSpan.Hours).ToString() + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
    }
}
