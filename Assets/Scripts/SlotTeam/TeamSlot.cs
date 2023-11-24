using UnityEngine;
using UnityEngine.UI;

public class TeamSlot : MonoBehaviour
{
    public int slotIndex{get; private set;}
    private TeamSlotController tsc;
    [SerializeField] public GameObject _plantpot_obj;
    [Header("Price Slot")]
    [SerializeField] public int _priceThisSlot;
    [SerializeField] public string _blockID;
    [SerializeField] public string _areaID;
    [SerializeField] public Text price_text;
    [SerializeField] public moneyType _moneyType;
    [Header("Adtibuild")]
    public bool isLock = true;
    public bool hasDataInSlot = false;
    public CharacterData myData;
    public Transform rootParent;
    public GameObject unlockBtn,plantBtn,haverseBtn,deletingBtn;
    [Header("Stake Zone")]
    public ParticleSystem StakeFx;

    public void Setup(int index, bool isLock, TeamSlotController slotCon)
    {
        //Debug.Log(this.name + "setup on index " + index);
        this.isLock = isLock;
        tsc = slotCon;
        slotIndex = index;
        //Debug.Log(this.name + "my index " + slotIndex);
    }

    public void AddUnitToSlot(CharacterData unitData)
    {
        if(isLock) return;
        if(hasDataInSlot)
        {
            deleteUnitDataInSlot();
        }
        myData = unitData;
        myData.detail._unitPos = slotIndex;
        hasDataInSlot = true;
        SetUpCharacter();
    }

    public void deleteUnitDataInSlot()
    {
        if(isLock) return;
        if (myData == null) return;  
        if (myData.detail != null)
        {
            myData.detail._unitPos = -1;
            myData = null;
        }
        hasDataInSlot = false;
        SetUpCharacter();
    }

    public void SetUpCharacter()
    {
        if(!hasDataInSlot || isLock)
        {
            if(rootParent.childCount > 0)
                Destroy(rootParent.GetChild(0).gameObject);
        }
        else
        {
            GameObject prefab = myData.detail._unitPrefab;
            //Edit: show game object in inspactor to get data
            _plantpot_obj = Instantiate(prefab, rootParent.position,Quaternion.identity,rootParent);
            _plantpot_obj.transform.localScale = Vector3.one;
            //SpriteRenderer[] renderers = go.GetComponentsInChildren<SpriteRenderer>();
            //for (int i = 0; i < renderers.Length; i++)
            //{
            //    Image temp = renderers[i].gameObject.AddComponent<Image>();
            //    temp.sprite = renderers[i].sprite;
            //    temp.color = renderers[i].color;
            //    temp.raycastTarget = false;
            //    renderers[i].enabled = false;
            //}
            //Animator animator = go.GetComponent<Animator>();
            //if(animator != null)
            //{
            //    SetUpAnimator(animator);
            //}
        }
    }

    private void SetUpAnimator(Animator anim)
    {
        anim.enabled = false;
        //yield return new WaitForSeconds(0.5f);
        anim.enabled = true;
        anim.Rebind();
        //yield break;
    }

    public void HideAllBtn()
    {
        unlockBtn.SetActive(isLock);
        plantBtn.SetActive(!isLock && !hasDataInSlot);
        haverseBtn.SetActive(false);
        deletingBtn.SetActive(false);
    }

    public void ShowOptionBtn()
    {
        if(isLock || !hasDataInSlot) return;
        haverseBtn.SetActive(true);
        deletingBtn.SetActive(true);
    }

    public void OnSlotClick()
    {
        if(isLock || !hasDataInSlot) return;
        tsc.OnSlotClick(slotIndex);
    }
    public void OnUnlockBtnClick()
    {
        if(!isLock) return;
        tsc.OnSlotUnlockBtnClick(slotIndex);
    }
    public void OnPlantBtnClick()
    {
        tsc.OnSlotPlantBtnClick(slotIndex);
    }
    public void OnHaverseBtnClick()
    {
        tsc.OnSlotHaverseBtnClick(slotIndex);
    }
    public void OnDeleteBtnClick()
    {
        tsc.OnSlotDeleteBtnClick(slotIndex);
    }

    public void StakeFxSet(bool isOn)
    {
        StakeFx.gameObject.SetActive(isOn);
        if(isOn)
        {
            StakeFx.Play();
        }
    }
}
