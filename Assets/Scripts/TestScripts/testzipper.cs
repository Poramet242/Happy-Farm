using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testzipper : MonoBehaviour
{
    [Header("Gacha This Object")]
    public GameObject GachaAniamtionObject;
    public GameObject thisObjectCtr;
    public GameObject ParticleGachaCtr;
    public GameObject thisGacha;
    [Header("Addtibuild")]
    public Image bg;
    public Image gacha;
    public Slider slider;
    public GameObject gachaAnim;
    [Header("Camera transform")]
    public Camera cam;
    public Vector3 oldCam_transform;
    public Vector3 newCam_transform;
    [Header("Animation Slider")]
    public Slider animationSlider;
    public float current_aura_Time = 0.5f;
    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        oldCam_transform = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        GachaAniamtionObject.GetComponent<Animator>().enabled = false;
        setUpcameraToGacha();
    }
    public void setUpcameraToGacha()
    {
        GameObject tesmparticle = Instantiate(ParticleGachaCtr, cam.gameObject.transform);
        thisGacha = tesmparticle;
        cam.gameObject.transform.position = newCam_transform;
    }
    public void resetCameraToGameplay()
    {
        Destroy(thisGacha);
        cam.gameObject.transform.position = oldCam_transform;
    }
    void Update()
    {
        if (slider.value < slider.maxValue && slider.value != 0)
        {
            thisGacha.GetComponent<particleGachaCtr>().particale_plante.gameObject.SetActive(false);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura.gameObject.SetActive(true);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura2.gameObject.SetActive(false);
            animationSlider.gameObject.SetActive(false);
        }
        else if (slider.value == 0)
        {
            thisGacha.GetComponent<particleGachaCtr>().particale_plante.gameObject.SetActive(true);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura2.gameObject.SetActive(true);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura.gameObject.SetActive(false);

            current_aura_Time -= Time.deltaTime;
            if (current_aura_Time <= 0)
            {
                GachaAniamtionObject.GetComponent<Animator>().enabled = true;
                StartCoroutine(delayAnimation());
            }
            /* if (current_aura_Time <= 0)
                 particale_aura.gameObject.SetActive(false);
             else
                 current_aura_Time -= Time.deltaTime;

             if (current_gachaAnim_Time <= 0)
                 gachaAnim.GetComponent<Animator>().SetBool("Gacha", true);
             else
                 current_gachaAnim_Time -= Time.deltaTime;*/
        }
        else 
        {
            thisGacha.GetComponent<particleGachaCtr>().particale_plante.gameObject.SetActive(false);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura.gameObject.SetActive(false);
            thisGacha.GetComponent<particleGachaCtr>().particale_aura2.gameObject.SetActive(false);
            animationSlider.gameObject.SetActive(true);
        }
        bg.fillAmount = slider.value;
    }
    IEnumerator delayAnimation()
    {
        yield return new WaitForSeconds(1.5f);
        thisGacha.GetComponent<particleGachaCtr>().particale_plante.gameObject.SetActive(false);
        thisGacha.GetComponent<particleGachaCtr>().particale_aura.gameObject.SetActive(false);
        thisGacha.GetComponent<particleGachaCtr>().particale_aura2.gameObject.SetActive(false);
        animationSlider.gameObject.SetActive(false);
        resetCameraToGameplay();
        Destroy(thisObjectCtr);
        SpinLayerController.instance.ShowDefToGameplay();
    }
}
