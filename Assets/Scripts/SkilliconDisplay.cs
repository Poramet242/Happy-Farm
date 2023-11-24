using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkilliconDisplay : MonoBehaviour
{
    [SerializeField] public UnitSkill skill;
    [SerializeField] public string nameSkill;
    [SerializeField] public string infoSkill;

    public void onClickInfoSkill()
    {
        IconSkillController.instance.InfoSkill = skill;
        IconSkillController.instance.Info_panel.SetActive(true);
        Transform infoPanelTransform = IconSkillController.instance.Info_panel.transform;
        Vector3 offset = new Vector3(0f, -120f, 0f); // Define a suitable offset
        infoPanelTransform.position = transform.position + offset; // Set position with offset
        IconSkillController.instance.Bg_panel.SetActive(true);
        IconSkillController.instance.NameSkill_text.text = nameSkill;
        IconSkillController.instance.InfoSkill_text.text = infoSkill;
    }
}
