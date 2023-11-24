using CannabisFarm.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XSystem;

public class ProfileLayerController : MonoBehaviour
{
    public static ProfileLayerController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Profile")]
    [SerializeField] public GameObject _profileObj;
    [SerializeField] private Text _namePlayer_text;
    [SerializeField] private Text _level_text;
    [SerializeField] private Image _valueLevel;
    [SerializeField] public Image _profile_img;
    [Header("Skill Icon")]
    [SerializeField] public GameObject _activeSkillInconPanel;
    [SerializeField] public GameObject _infoPanel;
    [Header("Select Images")]
    [SerializeField] private List<GameObject> _iconImg = new List<GameObject>();
    [SerializeField] private GameObject _selectObj;
    [SerializeField] private GameObject _tempImg;
    [SerializeField] private GameObject _contan;
    [SerializeField] public Sprite _selectedSprite;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private GameObject _confirm_btn;
    [Header("Edit Name")]
    [SerializeField] private GameObject _editObj;
    [SerializeField] private string _name;
    [SerializeField] private InputField _nameField;
    [SerializeField] private GameObject[] _payEditName;
    [SerializeField] GameObject _infoEditName;
    [Header("Other")]
    [SerializeField] private GameObject warningUi;
    [SerializeField] private Text confirmYouName_text;
    [SerializeField] public bool checkSelectedImages;
    [Header("Button")]
    [SerializeField] private Button editName_btn;
    [SerializeField] private Button editImages_btn;
    [Header("Popular point")]
    [SerializeField] public Text poppular_text;
    public void Initialize()
    {
        _level_text.text = PlayerObject.instance._levelPlayer.ToString();
        updateLevelTogameplay();
        if (_iconImg.Count == 0)
        {
            setImagesDisplay();
        }

    }
    private void Start()
    {
        _profile_img.sprite = PlayerObject.instance._imagesProfile_spr;

    }
    private void Update()
    {
        if (!PlayerObject.instance._checkplayTutorial)
        {
            editName_btn.enabled = false;
            editImages_btn.enabled = false;
        }
        _infoEditName.SetActive(!PlayerObject.instance._checkEditName);
        _namePlayer_text.text = PlayerObject.instance._playerName;
        confirmYouName_text.text = _nameField.text;
        poppular_text.text = PlayerObject.instance._popularPoint.ToString("#,##0");
        if (PlayerObject.instance._checkEditName)
        {
            _payEditName[0].SetActive(false);
            _payEditName[1].SetActive(true);
        }
        if (checkSelectedImages)
        {
            _confirm_btn.GetComponent<Button>().interactable = false;
        }
        else
        {
            _confirm_btn.GetComponent<Button>().interactable = true;
        }
    }
    public void updateLevelTogameplay()
    {
        _level_text.text = PlayerObject.instance._levelPlayer.ToString();
        float fillLevel = 0.0f;// (PlayerObject.instance.current_livelPlayer % PlayerObject.instance.max_levelPlayer) / PlayerObject.instance.max_levelPlayer;
        fillLevel = (PlayerObject.instance.current_levelPlayer - PlayerObject.instance.min_levelPlayer) / (PlayerObject.instance.max_levelPlayer - PlayerObject.instance.min_levelPlayer);
        _valueLevel.fillAmount = fillLevel;
        if (PlayerObject.instance.current_levelPlayer > PlayerObject.instance.max_levelPlayer)
        {
            _valueLevel.fillAmount = 0.0f;
        }

    }
    public void ClearPlayerProfileObject()
    {
        Destroy(this.gameObject);
    }
    public void onclickCloseThisUi()
    {
        StakeLayerController.instance.CloseUiLayerGameplay();
    }
    public void onclickOpneThisUi()
    {
        StakeLayerController.instance.OpenUiLayerGameplay();
    }

    #region Edit ImageProfile
    public void HideEditorImages(bool check)
    {
        //addtibuild
        _selectObj.SetActive(check);
        SoundListObject.instance.OnclickSFX(0);
        for (int i = 0; i < _iconImg.Count; i++)
        {
            if (_iconImg[i].name == PlayerObject.instance._imagesID)
            {
                _iconImg[i].GetComponent<Toggle>().isOn = true;
            }
            else
            {
                _iconImg[i].GetComponent<Toggle>().isOn = false;
            }
        }
    }
    public void resetImagesSelect()
    {
        for (int i = 0; i < _iconImg.Count; i++)
        {
            _iconImg[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    public void setImagesDisplay()
    {
        List<GameObject> tempObjects = new List<GameObject>();
        for (int i = 0; i < PlayerObject.instance._allLocalImages.Count; i++)
        {
             GameObject tempImage = Instantiate(_tempImg, _contan.transform);
             tempImage.name = PlayerObject.instance._allLocalImages[i].name;
             tempImage.SetActive(true);
             tempImage.transform.GetChild(0).gameObject.name = "Icon " + i;
             SetTempFromScriptableObjects(tempImage, PlayerObject.instance._allLocalImages[i], setIsLockImagesProfileDisplay(PlayerObject.instance._allLocalImages[i].name));
             tempObjects.Add(tempImage);
        }
         tempObjects.Sort((a, b) => a.GetComponent<ProfileSelectedDisplay>().isLock.CompareTo(b.GetComponent<ProfileSelectedDisplay>().isLock));
         _iconImg.Clear();
        foreach (GameObject obj in tempObjects)
        {   
            _iconImg.Add(obj);
        }
        setToggle();
        SortIconImgByLockStatus();
    }
    public void setToggle()
    {
        foreach (GameObject item in _iconImg)
        {
            item.GetComponent<Toggle>().group = _toggleGroup;
        }
    }
    public void SortIconImgByLockStatus()
    {
        _iconImg.Sort((a, b) => a.GetComponent<ProfileSelectedDisplay>().isLock.CompareTo(b.GetComponent<ProfileSelectedDisplay>().isLock));
        for (int i = 0; i < _iconImg.Count; i++)
        {
            _iconImg[i].transform.SetSiblingIndex(i);
        }
    }

    public void OnClickSaveSelectImages()
    {
        PlayerObject.instance._imagesProfile_spr = _selectedSprite;
        PlayerObject.instance._imagesID = _selectedSprite.name;
        _profile_img.sprite = PlayerObject.instance._imagesProfile_spr;
        StartCoroutine(setSelectImagesDisplay(PlayerObject.instance._imagesID));
    }
    IEnumerator setSelectImagesDisplay(string ImagesID)
    {
        IWSResponse response = null;
        yield return Account.setProfileImage(XCoreManager.instance.mXCoreInstance, ImagesID, (r) => response = r);
        if (!response.Success())
        {
            Debug.LogError(response.ErrorsString());
            yield break;
        }
    }
    public void SetTempFromScriptableObjects(GameObject tempObj, Sprite tempDetail,bool checkget)
    {
        tempObj.GetComponent<ProfileSelectedDisplay>().SetData(tempDetail, checkget);
    }
    public bool setIsLockImagesProfileDisplay(string str, bool checkTest = false)
    {
        if(checkTest)
        {
            return true;
        }
        bool check = true;
        if (str == "Avatardefault")
        {
            check = false;
            return check;
        }
        for (int i = 0; i < AssistantsObject.instance._assistantsAllList.Count; i++)
        {
            if (AssistantsObject.instance._assistantsAllList[i]._unitImageID == str)
            {
                check = false;
                break;
            }
            else
            {
                check = true;
            }
        }
        return check;
    }
    #endregion

    #region Edit Name
    public void OnClickEditName(bool check)
    {
        SoundListObject.instance.OnclickSFX(0);
        if ((_nameField.text == null) && (_nameField.text == string.Empty))
        { 
            warningUi.SetActive(true);
            warningUi.GetComponent<WarningUi>()._innfo_txt.text = "Please enter your name";
            return;
        }
        else 
        {
            if (check)
            {
                _name = _nameField.text;
                StartCoroutine(SetPlayerName(_name, check));
                PlayerObject.instance._checkEditName = check;
            }
            else
            {
                _name = _nameField.text;
                StartCoroutine(SetPlayerName(_name, check));
            }
        }
    }
    public void resetStringName()
    {
        _nameField.text = null;
    }
    public void HideEditorName(bool check)
    {
        //addtibuild
        SoundListObject.instance.OnclickSFX(0);
        if (PlayerObject.instance._checkEditName)
        {
            _payEditName[0].SetActive(!PlayerObject.instance._checkEditName);
            _payEditName[1].SetActive(PlayerObject.instance._checkEditName);
            //close images
            _infoEditName.SetActive(!PlayerObject.instance._checkEditName);
        }
        _editObj.SetActive(check);
        resetStringName();
    }
    IEnumerator SetPlayerName(string name, bool check)
    {
        IWSResponse response = null;
        if (check)
        {
            yield return Account.SetName(XCoreManager.instance.mXCoreInstance, name, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            PlayerObject.instance._playerName = name;
        }
        else
        {
            yield return Account.SetNamePaid(XCoreManager.instance.mXCoreInstance, name, (r) => response = r);
            if (!response.Success())
            {
                Debug.LogError(response.ErrorsString());
                yield break;
            }
            PlayerObject.instance._playerName = name;
            yield return PlayerObject.instance.GetWalletPlayer();
        }

    }
    #endregion
    public void playsoundVFX()
    {
        SoundListObject.instance.OnclickSFX(0);
    }
}
