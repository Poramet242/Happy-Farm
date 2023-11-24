using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardAnimation : MonoBehaviour
{
    public bool isHaver;
    [Header("Particle")]
    [SerializeField] private GameObject unitParticle;
    [SerializeField] private GameObject targetpaticle;
    [SerializeField] public ParticleSystem _plant_Eff;
    [Header("Coin")]
    [SerializeField] private GameObject content_obj;
    [SerializeField] private GameObject template_obj;
    [SerializeField] private GameObject target_obj;
    [SerializeField] private List<GameObject> allUnitCoin;
    [Header("Gem")]
    [SerializeField] private GameObject target_objGem;
    [SerializeField] private GameObject template_objGem;
    [SerializeField] private List<GameObject> allUnitGem;
    [Header("Exp")]
    [SerializeField] private GameObject target_objExp;
    [SerializeField] private GameObject template_objExp;
    [SerializeField] private List<GameObject> allUnitExp;
    [Header("Data aniamtion")]
    [Range(0f, 100f)]
    [SerializeField] private float speed = 5f;
    private int numcoine;
    private int numgem;
    private int numExp;
    private bool isMoveCoin;
    private bool isMoveGem;
    private void Start()
    {
        _plant_Eff.gameObject.SetActive(false);
    }
    public void setUpParticle(CharacterData characterData)
    {
        unitParticle = characterData.detail._unitParticle;
        GameObject unitparticleTemp = Instantiate(unitParticle, targetpaticle.transform);
        unitparticleTemp.SetActive(true);
        unitparticleTemp.GetComponent<ParticleSystem>().Play();
        StartCoroutine(resetParticleHaverPlane(unitparticleTemp));
    }
    IEnumerator resetParticleHaverPlane(GameObject tempParticle)
    {
        yield return new WaitForSeconds(1.5f);
        tempParticle.GetComponent<ParticleSystem>().Stop();
        Destroy(tempParticle);
    }
    public void setUpCoin(int coin)
    {
        allUnitCoin = new List<GameObject>();
        numcoine = coin;
        for (int i = 0; i < numcoine; i++)
        {
            GameObject unitCoin = Instantiate(template_obj, content_obj.transform);
            unitCoin.SetActive(true);
            // Get the content object's position and size
            Vector3 contentPos = content_obj.transform.position;
            Vector3 contentSize = content_obj.GetComponent<RectTransform>().sizeDelta;

            // Set the new position for the instantiated object
            Vector3 newPos = new Vector3(Random.Range(contentPos.x - contentSize.x / 2f, contentPos.x + contentSize.x / 2f),
                                         Random.Range(contentPos.y - contentSize.y / 2f, contentPos.y + contentSize.y / 2f),
                                         contentPos.z);

            // Set the new position for the instantiated object
            unitCoin.transform.position = newPos;
            allUnitCoin.Add(unitCoin);
        }
        isMoveCoin = true;
    }
    public void setUpGem(int gem)
    {
        allUnitGem = new List<GameObject>();
        numgem = gem;
        for (int i = 0; i < numgem; i++)
        {
            GameObject unitGem = Instantiate(template_objGem, content_obj.transform);
            unitGem.SetActive(true);
            // Get the content object's position and size
            Vector3 contentPos = content_obj.transform.position;
            Vector3 contentSize = content_obj.GetComponent<RectTransform>().sizeDelta;

            // Set the new position for the instantiated object
            Vector3 newPos = new Vector3(Random.Range(contentPos.x - contentSize.x / 2f, contentPos.x + contentSize.x / 2f),
                                         Random.Range(contentPos.y - contentSize.y / 2f, contentPos.y + contentSize.y / 2f),
                                         contentPos.z);

            // Set the new position for the instantiated object
            unitGem.transform.position = newPos;
            allUnitGem.Add(unitGem);
        }
        isMoveGem = true;
    }
    public void setUpExp(int exp)
    {
        allUnitExp = new List<GameObject>();
        numExp = exp;
        for (int i = 0; i < numExp; i++)
        {
            GameObject unitExp = Instantiate(template_objExp, content_obj.transform);
            unitExp.SetActive(true);

            Vector3 contentPos = content_obj.transform.position;
            Vector3 contentSize = content_obj.GetComponent<RectTransform>().sizeDelta;

            Vector3 newPos = new Vector3(Random.Range(contentPos.x - contentSize.x / 2f, contentPos.x + contentSize.x / 2f),
                                         Random.Range(contentPos.y - contentSize.y / 2f, contentPos.y + contentSize.y / 2f),
                                         contentPos.z);
            unitExp.transform.position = newPos;
            allUnitExp.Add(unitExp);
        }

    }
    private void Update()
    {
        if (isMoveCoin)
        {
            for (int i = 0; i < allUnitCoin.Count; i++)
            {
                allUnitCoin[i].transform.position = Vector3.Lerp(allUnitCoin[i].transform.position, target_obj.transform.position, speed * Time.deltaTime);
                StartCoroutine(removeCoinObject());
            }
            if (isMoveGem)
            {
                for (int i = 0; i < allUnitGem.Count; i++)
                {
                    allUnitGem[i].transform.position = Vector3.Lerp(allUnitGem[i].transform.position, target_objGem.transform.position, speed * Time.deltaTime);
                    StartCoroutine(removeGemObject());
                }
            }
            for (int i = 0; i < allUnitExp.Count; i++)
            {
                allUnitExp[i].transform.position = Vector3.Lerp(allUnitExp[i].transform.position, target_objExp.transform.position, speed * Time.deltaTime);
                StartCoroutine(removeExpGameObject());
            }
        }
        else
        {
            if(content_obj.transform.childCount > 0)
            {
                for (int i = 0; i < content_obj.transform.childCount; i++)
                {
                    Destroy(content_obj.transform.GetChild(i));
                }
            }
            else
            {
                return;
            }
        }
    }
    IEnumerator removeCoinObject()
    {
        yield return new WaitForSeconds(0.5f);
        isMoveCoin = false;
        for (int i = 0; i < allUnitCoin.Count; i++)
        {
            Destroy(allUnitCoin[i]);
        }
        allUnitCoin.Clear();
        isHaver = false;
    }
    IEnumerator removeGemObject()
    {
        yield return new WaitForSeconds(0.5f);
        isMoveGem = false;
        for (int i = 0; i < allUnitGem.Count; i++)
        {
            Destroy(allUnitGem[i]);
        }
        allUnitGem.Clear();
        isHaver = false;
    }
    IEnumerator removeExpGameObject()
    {
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < allUnitExp.Count; i++)
        {
            Destroy(allUnitExp[i]);
        }
        allUnitExp.Clear();
    }
    #region old reset
    public void resetGem(GameObject temp, GameObject target)
    {
        if (temp.transform.position == target.transform.position)
        {
            isMoveGem = false;
            for (int i = 0; i < allUnitGem.Count; i++)
            {
                Destroy(allUnitGem[i]);
            }
            allUnitGem.Clear();
            isHaver = false;
        }
    }
    public void resetCoin(GameObject temp, GameObject target)
    {
        if (temp.transform.position == target.transform.position)
        {
            isMoveCoin = false;
            for (int i = 0; i < allUnitCoin.Count; i++)
            {
                Destroy(allUnitCoin[i]);
            }
            allUnitCoin.Clear();
            isHaver = false;
        }
    }
    #endregion
}
