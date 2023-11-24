using UnityEngine;
using UnityEngine.UI;

public class WarningUi : MonoBehaviour
{
    public static WarningUi instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Object")]
    [SerializeField] public GameObject _thisObject;
    [Header("Info")]
    [SerializeField] public Text _hardInfo_txt;
    [SerializeField] public Text _innfo_txt;
    public void setupWarning(string hard,string info)
    {
        _hardInfo_txt.text = hard;
        _innfo_txt.text = info;
    }
    public void onClickClose()
    {
        //close
        StakeLayerController.instance.CloseUiLayerGameplay();
        //addtibuild
        _thisObject.SetActive(false);
    }
}
