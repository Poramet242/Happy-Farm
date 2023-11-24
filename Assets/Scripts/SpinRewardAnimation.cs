using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRewardAnimation : MonoBehaviour
{
    [SerializeField] public GameObject animation_obj;
    [SerializeField] public GameObject ParticleGachaCtr;

    [SerializeField] public GameObject ParticleGachaCtr_NFT;
    public void setUpAnimationSpin(bool check, bool checkNFT)
    {
        if (check)
        {
            return;
        }
        else
        {
            if (checkNFT)
            {
                GameObject tempObj = Instantiate(animation_obj, this.gameObject.transform);
                tempObj.SetActive(true);
                SpinLayerController.instance.CloseObjectTogachaGameplay();
                tempObj.GetComponent<testzipper>().ParticleGachaCtr = ParticleGachaCtr_NFT;
            }
            else
            {
                GameObject tempObj = Instantiate(animation_obj, this.gameObject.transform);
                tempObj.SetActive(true);
                SpinLayerController.instance.CloseObjectTogachaGameplay();
                tempObj.GetComponent<testzipper>().ParticleGachaCtr = ParticleGachaCtr;
            }
        }
    }
}
