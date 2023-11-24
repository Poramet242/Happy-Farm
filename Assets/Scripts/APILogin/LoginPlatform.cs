using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPlatform : MonoBehaviour
{
    [SerializeField] public LoginTypes LoginTypes;
    [SerializeField] private Login login;
    public void onclickLoginThisPlatform()
    {
        login.OnClickLogin(LoginTypes);
    }
}
