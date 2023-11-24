using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSkillController : MonoBehaviour
{
    public static IconSkillController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [SerializeField] public UnitSkill InfoSkill;
    [SerializeField] public List<GameObject> Skill_icon;
    [SerializeField] public GameObject Info_panel;
    [SerializeField] public GameObject Bg_panel;
    [SerializeField] public Text NameSkill_text;
    [SerializeField] public Text InfoSkill_text;

    public void Initialize()
    {
        for (int i = 0; i < Skill_icon.Count; i++)
        {
            Skill_icon[i].SetActive(false);
        }
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    setupActivedSkillIcon(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]);
                }
            }
            else
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    if (ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]._skill == UnitSkill.Sale_PriceGachaPlant)
                    {
                        setupActivedSkillIcon(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]);

                    }
                }
            }
        }
    }
    public void setupActivedSkillIcon(AssisstantDetail assisstantDetail)
    {
        for (int i = 0; i < Skill_icon.Count; i++)
        {
            if (Skill_icon[i].GetComponent<SkilliconDisplay>().skill == assisstantDetail._skill)
            {
                Skill_icon[i].SetActive(true);
            }
        }
    }
    public void setupUnActivedSkillIcon(AssisstantDetail assisstantDetail)
    {
        for (int i = 0; i < Skill_icon.Count; i++)
        {
            if (Skill_icon[i].GetComponent<SkilliconDisplay>().skill == assisstantDetail._skill)
            {
                Skill_icon[i].SetActive(false);
            }
        }
        for (int i = 0; i < ZoneUnitObject.instance.unitDataZones.Count; i++)
        {
            if (ZoneUnitObject.instance.unitDataZones[i].ZoneType == PlayerObject.instance._zone)
            {
                for (int a = 0; a < ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone.Count; a++)
                {
                    setupActivedSkillIcon(ZoneUnitObject.instance.unitDataZones[i]._assisstantDetailThisZone[a]);
                }
            }
        }
    }
}
