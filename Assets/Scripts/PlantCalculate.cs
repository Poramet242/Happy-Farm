using System;
using UnityEngine;
using UnityEngine.UI;

public class PlantCalculate : MonoBehaviour
{
    [Header("Auto Haver")]
    [SerializeField] public bool isAutoHaver;
    [Header("Data")]
    [SerializeField] private PlantCalculate thisblock;
    [SerializeField] public double dateTimeSlot;
    [SerializeField] public CharacterData myData;
    [SerializeField] private TeamSlot teamSlot;
    [SerializeField] public bool CheckPlantData;
    [SerializeField] private bool CheckHaver;
    [Header("Adtibuild")]
    [SerializeField] private GameObject _plantPot_obj;
    [SerializeField] private GameObject _canvasValueGrowth;
    [SerializeField] private GameObject _bg_value;
    [SerializeField] private GameObject _canvasGrowth;
    [SerializeField] private Image _valueTimeGrowth;
    [SerializeField] private Text _stackPoint_txt;
    private void Update()
    {
        if (teamSlot.hasDataInSlot)
        {
            if (!CheckPlantData)
            {
                myData = teamSlot.myData;
                _plantPot_obj = teamSlot._plantpot_obj;
                _canvasValueGrowth.SetActive(true);
                setUpTimeCalculatePlant(myData);
                CheckPlantData = true;
            }
            else
            {
                //myData.unitData._unitCountTime += Time.deltaTime;
                dateTimeSlot = myData.unitData._unitCountTime;
                if (myData.unitData.DecayTime == 0)
                {
                    setUnitSpriteGrowthPlante(dateTimeSlot);
                }
                else
                {
                    setUnitSpriteGrowthPlantToRotted(dateTimeSlot);
                }
            }
        }
        else
        {
            _canvasValueGrowth.SetActive(false);
            _valueTimeGrowth.fillAmount = 0f;
            myData.detail = null;
            myData.unitData = null;
            CheckPlantData = false;
            dateTimeSlot = 0f;
        }
    }
    public void setUpTimeCalculatePlant(CharacterData characterData)
    {
        setTimeStartInValue(DateTime.Now, characterData);
    }
    public void setUnitSpriteGrowthPlante(double timeGrowth)
    {
        if (timeGrowth > (myData.unitData.GrowTime * 0.3f) && (timeGrowth < (myData.unitData.GrowTime * 0.6f)))
        {
            _plantPot_obj.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._babey_Img;
            myData.detail._growthPlante = GrowthPlante.Baby;
            //setValueTimeGrowthPlant((float)myData.unitData._unitCountTime, false);
            TimeGrowthPlant((float)myData.unitData._unitCountTime);
            if (!PlayerObject.instance._checkplayTutorial)
            {
                TutorialGameplay.instance._textShow_toturia_text.text = "Once this progress bar is filled up, the full grown plant will be ready to be gathered.";
            }
        }
        else if (timeGrowth > myData.unitData.GrowTime)// mod 3 and go to next stage
        {
            _plantPot_obj.GetComponent<PlantPotController>().plant.sprite = myData.detail._plant_Image;
            if (!_plantPot_obj.GetComponent<PlantPotController>()._animator)
            {
                _plantPot_obj.GetComponent<PlantPotController>()._animator = true;
                _plantPot_obj.GetComponent<PlantPotController>()._zoneBlocks = SplitingToint(gameObject.GetComponent<TeamSlot>()._blockID, "block");
                _plantPot_obj.GetComponent<PlantPotController>().PlayAnimation("animation");
            }
            myData.detail._growthPlante = GrowthPlante.Growth;
            if (myData.unitData._unitCountTimeHaver < myData.unitData.timePerCoin)
            {
                setValueTimeGrowthPlant((float)myData.unitData._unitCountTimeHaver, false);
            }
            else
            {
                setValueTimeGrowthPlant((float)myData.unitData._unitCountTimeHaver, true);
            }
        }
        else
        {
            TimeGrowthPlant((float)myData.unitData._unitCountTime);
            if (!PlayerObject.instance._checkplayTutorial)
            {
                TutorialGameplay.instance._textShow_toturia_text.text = "Once the seed was planted, the system will show the planting stage by displaying the progress bar.";
            }
        }
    }
    public void setUnitSpriteGrowthPlantToRotted(double timeGrowth)
    {
        if (timeGrowth > (myData.unitData.GrowTime * 0.3f) && (timeGrowth < (myData.unitData.GrowTime * 0.6f)))
        {
            _plantPot_obj.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._babey_Img;
            myData.detail._growthPlante = GrowthPlante.Baby;
            //setValueTimeGrowthPlant((float)myData.unitData._unitCountTime, false);
            TimeGrowthPlant((float)myData.unitData._unitCountTime);
        }
        else if ((timeGrowth > myData.unitData.GrowTime) && (timeGrowth < myData.unitData.DecayTime))// mod 3 and go to next stage
        {
            _plantPot_obj.GetComponent<PlantPotController>().plant.sprite = myData.detail._plant_Image;
            if (!_plantPot_obj.GetComponent<PlantPotController>()._animator)
            {
                _plantPot_obj.GetComponent<PlantPotController>()._animator = true;
                _plantPot_obj.GetComponent<PlantPotController>()._zoneBlocks = SplitingToint(gameObject.GetComponent<TeamSlot>()._blockID, "block");
                _plantPot_obj.GetComponent<PlantPotController>().PlayAnimation("animation");
            }
            myData.detail._growthPlante = GrowthPlante.Growth;
            if (myData.unitData._unitCountTimeHaver < myData.unitData.timePerCoin)
            {
                setValueTimeGrowthPlant((float)myData.unitData._unitCountTimeHaver, false);
            }
            else
            {
                setValueTimeGrowthPlant((float)myData.unitData._unitCountTimeHaver, true);
            }
        }
        else if (timeGrowth > myData.unitData.DecayTime)
        {
            _plantPot_obj.GetComponent<PlantPotController>().plant.sprite = ImageDisplayController.instance._Rotted_Img;
            _plantPot_obj.GetComponent<PlantPotController>()._animator = false;
            myData.detail._growthPlante = GrowthPlante.Rotted;
            myData.unitData.isLife = false;
            _canvasGrowth.SetActive(false);
            return;
        }
        else
        {
            TimeGrowthPlant((float)myData.unitData._unitCountTime);
        }
    }
    public void setValueTimeGrowthPlant(float time,bool checkTimeHaver)
    {
        //effect count character in zone
        _canvasGrowth.SetActive(true);
        //int effect = ZoneUnitObject.instance.countAssisstantDetailThiszone(PlayerObject.instance._zone);
        if (checkTimeHaver)
        {
            float fillHaver = (time % myData.unitData.timePerCoin) / myData.unitData.timePerCoin;
            int stack = Mathf.FloorToInt(time / myData.unitData.timePerCoin);
            if (stack < myData.unitData._unitMaxStackCoine)
            {
                _valueTimeGrowth.fillAmount = fillHaver;
                myData.unitData._unitStackCoine = stack;
                if (myData.unitData._unitStackCoine > 0)
                {
                    _stackPoint_txt.gameObject.SetActive(true);
                    _stackPoint_txt.text = "x" + myData.unitData._unitStackCoine;
                    _bg_value.SetActive(true);
                    if (!PlayerObject.instance._checkplayTutorial)
                    {
                        TutorialGameplay.instance._textShow_toturia_text.text = "Click the coins to collect your income.";
                    }
                }
                else
                {
                    _stackPoint_txt.gameObject.SetActive(false);
                    _bg_value.SetActive(false);
                }
            }
            else
            {
                stack = myData.unitData._unitMaxStackCoine;
                myData.unitData._unitStackCoine = stack;
                _valueTimeGrowth.fillAmount = 1f;
                _stackPoint_txt.text = "x" + myData.unitData._unitStackCoine;
                _stackPoint_txt.gameObject.SetActive(true);
                _bg_value.SetActive(true);
            }
        }
        else
        {
            float fillNone = (time % myData.unitData.timePerCoin) / myData.unitData.timePerCoin;
            _valueTimeGrowth.fillAmount = fillNone;
            _stackPoint_txt.gameObject.SetActive(false);
            _bg_value.SetActive(false);
        }
       
    }
    public void TimeGrowthPlant(float time)
    {
        _canvasGrowth.SetActive(true);
        float fillNone = (time % myData.unitData.GrowTime) / myData.unitData.GrowTime;
        _valueTimeGrowth.fillAmount = fillNone;
        _stackPoint_txt.gameObject.SetActive(false);
        _bg_value.SetActive(false);
    }
    public int SplitingToint(string str, string fixer)
    {
        string input = str;
        string prefix = fixer;
        string numPart = input.Substring(prefix.Length);
        int num;
        int.TryParse(numPart, out num);
        return num;
    }
    #region DATA TIME
    public void setTimeNowInValue(DateTime timeOld, DateTime timeNow, CharacterData characterData)
    {
        TimeSpan timeSpan = timeNow - timeOld;
        characterData.unitData._unitCountTime = timeSpan.TotalSeconds;
        Debug.Log("Pase to double: " + characterData.unitData._unitCountTime);
    }
    //Use to set time in start plante
    public void setTimeStartInValue(DateTime timeNow, CharacterData characterData)
    {
        TimeSpan timeSpan = TimeSpan.Zero;
        //characterData.unitData._unitTime = timeSpan.TotalSeconds;
        characterData.unitData._unitDateTimeStamp = timeNow;
        //Debug.Log("Stamp time: " + characterData.unitData._unitDateTimeStamp);
    }
    #endregion
}
