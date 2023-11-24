using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIClock : MonoBehaviour
{
    public static TestUIClock instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Text timeStamp;
    public Text timeNow;
    public GameObject contanStamp;
    public GameObject contanNow;
    public GameObject preStamp;
    public GameObject preNow;
    public InputField InputField;
}
