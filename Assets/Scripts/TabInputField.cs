using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    [SerializeField] private LoginObject_Ctr loginObject_Ctr;
    [Header("Login")]
    [SerializeField] private InputField EmailInput;
    [SerializeField] private InputField PasswordInput;
    public int inputSelectedLogin;
    [Header("Register")]
    [SerializeField] private InputField EmailInputRe;
    [SerializeField] private InputField PasswordInputRe;
    [SerializeField] private InputField CfasswordInputRe;
    public int inputSelectedRegister;


    private void Update()
    {
        if (loginObject_Ctr.isLogin)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                inputSelectedLogin--;
                if (inputSelectedLogin < 0) inputSelectedLogin = 1;
                SelectInputFild();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                inputSelectedLogin++;
                if (inputSelectedLogin > 1) inputSelectedLogin = 0;
                SelectInputFild();
            }
            void SelectInputFild()
            {
                switch (inputSelectedLogin)
                {
                    case 0: EmailInput.Select();
                        break;
                    case 1: PasswordInput.Select(); 
                        break;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKeyDown(KeyCode.LeftShift))
            {
                inputSelectedRegister--;
                if (inputSelectedRegister < 0) inputSelectedRegister = 2;
                SelectInputFild();
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                inputSelectedRegister++;
                if (inputSelectedRegister > 2) inputSelectedRegister = 0;
                SelectInputFild();
            }
            void SelectInputFild()
            {
                switch (inputSelectedRegister)
                {
                    case 0:EmailInputRe.Select();
                        break;
                    case 1:PasswordInputRe.Select();
                        break;
                    case 2:CfasswordInputRe.Select();
                        break;
                }
            }
        }
    }
    public void EmailSelectedLogin() => inputSelectedLogin = 0;
    public void PasswordSelectedLogin() => inputSelectedLogin = 1;

    public void EmailSelectedRegister() => inputSelectedRegister = 0;
    public void PasswordSelectedRegister() => inputSelectedRegister = 1;
    public void CfPasswordSelectedRegister() => inputSelectedRegister = 2;


}
