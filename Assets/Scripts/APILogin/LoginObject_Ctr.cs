#region Android
using Google;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#endregion

#region IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Native;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
#endregion

#region Facebook
using Facebook.Unity;
#endregion

using UnityEngine;
using UnityEngine.UI;
using System.Text;
using XSystem;

public class LoginObject_Ctr : MonoBehaviour
{
    #region E-mail
    public bool isLogin;
    public GameObject _panelSystem;
    public GameObject _buttonGroup;
    public GameObject _playGaem_btn;
    [SerializeField] public WarningUi _warningUi;
    [Header("LoginWithMail")]
    public GameObject LoginWithMail;
    public GameObject _login;
    public GameObject _register;
    [Header("Login")]
    public InputField EmailLoign;
    public InputField passwordLoign;
    [Header("Register")]
    public InputField userNameRegister;
    public InputField passwordRegister;
    public InputField EmailRegister;
    public InputField confirmPassword;
    public Button Confirm_btn;
    #endregion

    #region facebook
    private string TokenFacebook;
    #endregion

    #region google
    private GoogleSignInConfiguration configuration;
    private string TokenGoogle;
    public string webClientId = "<your client id here>";
    public string webClientIdIOS = "957645404596-52bj8flc7ac1c7o139lhdpqcvn1gnqa8.apps.googleusercontent.com";
    #endregion

    #region Apple
    private IAppleAuthManager appleAuthManager;
    public string AppleUserIdKey { get; private set; }

    public string Token { get; private set; }
    public string Error { get; private set; }
    #endregion

    void Awake()
    {
        //Facebook ActivateApp
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        //Google ActivateApp
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestEmail = true,
            RequestIdToken = true
        };
    }
    /*private void Start()
    {
#if UNITY_IOS
        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
        }
#endif
    }*/
    private void Update()
    {
        if ((confirmPassword.text == passwordRegister.text) && ((confirmPassword.text != string.Empty) && (passwordRegister.text != string.Empty)))
        {
            Confirm_btn.interactable = true;
        }
        else
        {
            Confirm_btn.interactable = false;
        }
#if UNITY_IOS
        //------------------------------------------------------------APPLE-----------------------------------------------------------
        // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (appleAuthManager != null) 
        {
            appleAuthManager.Update();
        }
        //----------------------------------------------------------------------------------------------------------------------------
#endif
    }
    public void showButtonLogin()
    {
        _buttonGroup.SetActive(true);
        _panelSystem.SetActive(false);
        _playGaem_btn.SetActive(false);
    }
    public void showPlayGame()
    {
        _playGaem_btn.SetActive(true);
        _panelSystem.SetActive(false);
        _buttonGroup.SetActive(false);
    }
    public void _callplanelSysyem()
    {
        _panelSystem.SetActive(true);
        _buttonGroup.SetActive(false);
        _playGaem_btn.SetActive(false);
    }
    public void showLogin(bool check)
    {
        if (check)
        {
            isLogin = true;
            LoginWithMail.SetActive(check);
            _login.SetActive(true);
            _register.SetActive(false);

        }
        else
        {
            isLogin = false;
            _login.SetActive(true);
            _register.SetActive(false);
            LoginWithMail.SetActive(check);
        }
    }
    public void ClarInputfild()
    {
        EmailLoign.text = null;
        passwordLoign.text = null;
        userNameRegister.text = null;
        passwordRegister.text = null;
        confirmPassword.text = null;
        EmailRegister.text = null;
    }
    public void onClickRegister()
    {
        isLogin = false;
        _login.SetActive(false);
        _register.SetActive(true);
    }
    public void onClickCancel()
    {
        isLogin = true;
        _login.SetActive(true);
        _register.SetActive(false);
        ClarInputfild();
    }
    public void onClickLoginWithEmail()
    {
        StartCoroutine(LoginWithEmail());
    }
    public void onClickConfirm()
    {
        StartCoroutine(RegisterWithEmail());
    }
    public void onClickFacebook()
    {
        var perms = new List<string>() { "public_profile", "email" };
        FB.LogInWithReadPermissions(perms, AuthCallback);
    }
    public void onClickAppleLogin()
    {
        if (appleAuthManager == null)
        {
            Initialize();
        }
        SignInApple();
    }
    public void Initialize()
    {
        var deserializer = new PayloadDeserializer();
        appleAuthManager = new AppleAuthManager(deserializer);
    }

#region setloginWithEmail
    IEnumerator LoginWithEmail()
    {
        Debug.Log("LoginWithEmail");
        IWSResponse response = null;

        yield return XUser.Login(XCoreManager.instance.mXCoreInstance, EmailLoign.text, passwordLoign.text, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            _warningUi._thisObject.SetActive(true);
            _warningUi._innfo_txt.text = response.ErrorsString();
            yield break;
        }
        showLogin(false);
        showPlayGame();
    }
    IEnumerator RegisterWithEmail()
    {
        Debug.Log("RegisterWithEmail");
        IWSResponse response = null;
        yield return XUser.SignUp(XCoreManager.instance.mXCoreInstance, EmailRegister.text, passwordRegister.text, userNameRegister.text, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            _warningUi._thisObject.SetActive(true);
            _warningUi._innfo_txt.text = response.ErrorsString();
            yield break;
        }
        _login.SetActive(true);
        _register.SetActive(false);
        ClarInputfild();
    }
#endregion

#region setloginFacebook
    // Awake function from Unity's MonoBehavior
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }
    private void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
            TokenFacebook = aToken.TokenString;
            StartCoroutine(setTokenFacebook(TokenFacebook));
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }
    IEnumerator setTokenFacebook(string token)
    {
        IWSResponse response = null;
        yield return XUser.FacebookAuth(XCoreManager.instance.mXCoreInstance, token, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            _warningUi._thisObject.SetActive(true);
            _warningUi._innfo_txt.text = response.ErrorsString();
            yield break;
        }
        showLogin(false);
        showPlayGame();
    }
#endregion

#region setloginGoogle
    public void onclickLoginGoole()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        //GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public void OnSignOut()
    {
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("IDToken: " + task.Result.IdToken + "!");
            TokenGoogle = task.Result.IdToken;
            StartCoroutine(setloginGoogle(TokenGoogle));
        }
    }
    IEnumerator setloginGoogle(string token)
    {
        Debug.Log("start SetloginGoogle!!!");
        IWSResponse response = null;
        yield return XUser.GoogleAuth(XCoreManager.instance.mXCoreInstance, token, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            _warningUi._thisObject.SetActive(true);
            _warningUi._innfo_txt.text = response.ErrorsString();
            yield break;
        }
        showLogin(false);
        showPlayGame();
        Debug.Log("End SetloginGoogle!!!");
    }
#endregion

#region setloginApple
    public void SignInApple()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);
        appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                // Obtained credential, cast it to IAppleIDCredential
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    // Apple User ID
                    // You should save the user ID somewhere in the device
                    var userId = appleIdCredential.User;
                    PlayerPrefs.SetString(AppleUserIdKey, userId);

                    // Email (Received ONLY in the first login)
                    var email = appleIdCredential.Email;

                    // Full name (Received ONLY in the first login)
                    var fullName = appleIdCredential.FullName;

                    // Identity token
                    var identityToken = Encoding.UTF8.GetString(
                        appleIdCredential.IdentityToken,
                        0,
                        appleIdCredential.IdentityToken.Length);
                        Token = identityToken;
                    StartCoroutine(setloginApple(Token));
                    // Authorization code
                    var authorizationCode = Encoding.UTF8.GetString(
                        appleIdCredential.AuthorizationCode,
                        0,
                        appleIdCredential.AuthorizationCode.Length);

                    // And now you have all the information to create/login a user in your system
                }
                else
                {
                    Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    Error = "Retrieving Apple Id Token failed.";
                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                Debug.Log("Sign-in with Apple error. Message: " + error);
                Error = "Retrieving Apple Id Token failed.";
            });
    }
    IEnumerator setloginApple(string token)
    {
        Debug.Log("start SetloginGoogle!!!");
        IWSResponse response = null;
        yield return XUser.AppleAuth(XCoreManager.instance.mXCoreInstance, token, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            _warningUi._thisObject.SetActive(true);
            _warningUi._innfo_txt.text = response.ErrorsString();
            yield break;
        }
        showLogin(false);
        showPlayGame();
        Debug.Log("End SetloginApple!!!");
    }

#endregion
}
