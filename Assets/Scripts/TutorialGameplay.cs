using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XSystem;

public class TutorialGameplay : MonoBehaviour
{
    public static TutorialGameplay instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public Image _face_img;
    public Image _face_img_coin;
    public Image _bg_face_img;
    public GameObject[] _pic_img;
    public GameObject _Gameplay_obj;
    //public GameObject _Cursor_obj;
    [Header("Show info gameObject")]
    public GameObject _infototurial_obj;
    public Text _textShow_toturia_text;
    public GameObject _infototurial_obj2;
    public Text _textShow_toturia_text2;
    public GameObject _skip_btn;
    [Header("Stage")]
    public Tutorial stageTutorial;
    [Header("Inventory")]
    public Transform _Inventory;
    [Header("Gacha")]
    public Transform _Gacha;
    [Header("Assistants")]
    public Transform _Assistants;
    [Header("Friend")]
    public Transform _Friend;
    [Header("Zone")]
    public Transform _Zone;
    [Header("Slot")]
    public Transform _Slot;
    [Header("Coine")]
    public Transform _Coine;    
    [Header("Coine")]
    public Transform _Furniture;
    private void Start()
    {
        StakeLayerController.instance.OpenUiLayerGameplayTutorial();
        _pic_img[1].SetActive(true);
        _bg_face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Slot;
    }
    private void Update()
    {

    }
    private void OnApplicationQuit()
    {
        
    }
    public void SkipTutorial()
    {
        StakeLayerController.instance.CloseUiLayerGameplay();
        StartCoroutine(setSkipTutorial());
    }
    public void PlayTutorial_1()
    {
        _pic_img[1].SetActive(false);
        _bg_face_img.gameObject.SetActive(false);
        _infototurial_obj.SetActive(true);
        _face_img.gameObject.SetActive(true);
        _skip_btn.SetActive(true);
        CastleController.instance.cantClickUiDisplay(true);
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Slot);
    }
    public void PlayTutorial_2_Inventory()
    {
        _pic_img[2].SetActive(false);
        _bg_face_img.gameObject.SetActive(false);

        _infototurial_obj.SetActive(false);
        _textShow_toturia_text.text = "";

        _infototurial_obj2.SetActive(true);
        _textShow_toturia_text2.text = "Firstly, press this button to open you inventory.";

        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Inventory;
        CastleController.instance.cantClickUiDisplay(false);
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Inventory);
    }

    public void PlayTutorial_3_Plant()
    {
        _pic_img[3].SetActive(false);
        _bg_face_img.gameObject.SetActive(false);
        _infototurial_obj2.SetActive(false);
        //Info
        _infototurial_obj.SetActive(true);
        _textShow_toturia_text.text = "Click icon + to select the seed.";
        //-----------------------------------------------
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Plante;
        CastleController.instance.cantClickUiDisplay(true);
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Slot);
    }
    public void PlayTutorial_4_Haver()
    {
        _pic_img[4].SetActive(false);
        _bg_face_img.gameObject.SetActive(false);
        _face_img.gameObject.SetActive(true);
        _infototurial_obj.SetActive(true);
        stageTutorial = Tutorial.Haver;
        CastleController.instance.cantClickUiDisplay(true);
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Slot);
    }
    public void PlayTutorial_5_Coin()
    {
        _pic_img[5].SetActive(true);
        _face_img_coin.gameObject.SetActive(true);
        _bg_face_img.gameObject.SetActive(false);
        //_infototurial_obj.SetActive(true);
        //_textShow_toturia_text.text = "See your coin";
        stageTutorial = Tutorial.Coine;
        CastleController.instance.cantClickUiDisplay(true);
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Coine);
    }
    public void PlayTutorial_6_Gacha()
    {
        _pic_img[6].SetActive(true);
        _face_img_coin.gameObject.SetActive(false);
        CastleController.instance.cantClickUiDisplay(true);
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Gacha;
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Gacha);
    }
    public void PlayTutorial_7_Zone()
    {
        _pic_img[7].SetActive(true);
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Zone;
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Zone);
    }
    public void PlayTutorial_8_Friend()
    {
        _pic_img[8].SetActive(true);
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Friend;
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Friend);
    }
    public void PlayTutorial_9_Assistants()
    {
        _pic_img[9].SetActive(true);
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Assistants;
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Assistants);
    }
    public void PlayTutorial_10_Furniture()
    {
        _pic_img[10].SetActive(true);
        _face_img.gameObject.SetActive(true);
        stageTutorial = Tutorial.Furniture;
        //_Cursor_obj.SetActive(true);
        updateNextStage(_Furniture);
    }    
    public void PlayTutorial_11_StartGame()
    {
        for (int i = 0; i < _pic_img.Length; i++)
        {
            _pic_img[i].SetActive(false);
        }
        _skip_btn.SetActive(false);
        _bg_face_img.gameObject.SetActive(true);
        _Gameplay_obj.SetActive(true);
        _face_img.gameObject.SetActive(false);
        _face_img_coin.gameObject.SetActive(false);
        //_Cursor_obj.SetActive(false);
        stageTutorial = Tutorial.StartGame;

    }
    public void CloseTutorial()
    {
        _face_img.gameObject.SetActive(false);
        _infototurial_obj.SetActive(false);
    }
    public void updateNextStage(Transform nextPos)
    {
        //_Cursor_obj.transform.position = nextPos.position;
        if (nextPos == _Slot)
        {
            StakeLayerController.instance.OpenUiLayerGameplayTutorial();
            _face_img.transform.position = nextPos.position;
        }
        else
        {
            StakeLayerController.instance.OpenUiLayerGameplay();
            _face_img.transform.position = nextPos.position;
            _face_img_coin.transform.position = nextPos.position;
        }
    }
    IEnumerator setSkipTutorial()
    {
        IWSResponse response = null;
        yield return Account.SetTutorialPlayed(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
        yield return Account.GetUserProfile(XCoreManager.instance.mXCoreInstance, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            Debug.Log("Error GetUserProfile");
            yield break;
        }
        var user = response as Account;
        PlayerObject.instance._checkplayTutorial = user.tutorialPlayed;
        //Clear tutorial
        SettingController.instance.ClearSettingObject();
        ProfileLayerController.instance.ClearPlayerProfileObject();
        //PlayerObject.instance.ClearPlayerObject();
        SoundManager.instance.StopBGM();
        SceneManager.LoadScene("Garage_zone");
    }
    public void playSoundVFX()
    {
        SoundListObject.instance.OnclickSFX(0);
    }
}
public enum Tutorial
{
    Inventory = 0,
    Gacha = 1,
    Slot = 2,
    Assistants = 3,
    Friend = 4,
    Zone = 5,
    Haver = 6,
    Coine = 7,
    Plante = 8,
    Furniture = 9,
    StartGame = 10,
}
