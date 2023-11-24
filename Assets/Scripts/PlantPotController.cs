using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class PlantPotController : MonoBehaviour
{
    public bool _animator;
    public SpriteRenderer plant;
    public SpriteRenderer pot;
    public GameObject plantPot_obj;
    public GameObject plantPot_anim;
    public int _zoneBlocks;
    [Header("Spine Controller")]
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Spine.AnimationState spineAnimationState;
    [SerializeField] private Spine.Skeleton skeleton;
    private void Start()
    {
        skeletonAnimation = plantPot_anim.transform.GetChild(0).GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        plantPot_obj.SetActive(true);
        plantPot_anim.SetActive(false);
        setupPositionPlatpot(_zoneBlocks, true);
        //plantPot_anim.SetActive(false);
    }
    public void PlayAnimation(string nameAnimation)
    {
        spineAnimationState.SetAnimation(0, nameAnimation, true);
        if (_animator)
        {
            plantPot_obj.SetActive(false);
            plantPot_anim.SetActive(true);
            setupPositionPlatpot(_zoneBlocks,false);
        }
        else
        {
            plantPot_obj.SetActive(true);
            plantPot_anim.SetActive(false);
        }
    }
    public void PlayAnimationOther(string anim)
    {
        TrackEntry animationEntry = spineAnimationState.SetAnimation(0, anim, false);
        animationEntry.Complete += animationEntry_Complete;
    }
    public void animationEntry_Complete(TrackEntry trackEntry)
    {
        spineAnimationState.SetAnimation(0, "animation", true);
    }
    public void setupPositionPlatpot(int block,bool checkStart)
    {
        int inblock = 0;
        if (checkStart)
        {
            if (block <= 3)
            {
                inblock = 45;
                plant.sortingOrder = inblock + 1;
                pot.sortingOrder = inblock;
            }
            else if (block > 3 && block <= 6)
            {
                inblock = 44;
                plant.sortingOrder = inblock + 1;
                pot.sortingOrder = inblock;

            }
            else if (block > 6)
            {
                inblock = 43;
                plant.sortingOrder = inblock + 1;
                pot.sortingOrder = inblock;
            }
        }
        else
        {
            if (block <= 3)
            {
                inblock = 45;
                skeletonAnimation.GetComponentInChildren<MeshRenderer>().sortingOrder = inblock;
            }
            else if (block > 3 && block <= 6)
            {
                inblock = 44;
                skeletonAnimation.GetComponentInChildren<MeshRenderer>().sortingOrder = inblock;
            }
            else if (block > 6)
            {
                inblock = 43;
                skeletonAnimation.GetComponentInChildren<MeshRenderer>().sortingOrder = inblock;
            }
        }
    }
}
