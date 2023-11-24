using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public bool isfacingleft;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Spine.AnimationState spineAnimationState;
    [SerializeField] private Spine.Skeleton skeleton;

    private void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    private void Update()
    {
#if UNITY_EDITOR
        testAnimationAllYak();
#endif
        skeletonAnimation.GetComponentInChildren<MeshRenderer>().sortingOrder = Mathf.FloorToInt(transform.position.y * -100);
        if (isfacingleft)
        {
            transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.GetChild(0).localScale = Vector3.one;
        }
    }
    public void PlayAnimation(string nameAnimation)
    {
        spineAnimationState.SetAnimation(0, nameAnimation, true);
    }
    GameObject isPlayObject;
    public void PlayAniamtionObjectINZone(string nameAnim,GameObject isObject)
    {
        //TrackEntry animationEntry = spineAnimationState.SetAnimation(0, nameAnim, false);
        TrackEntry animationEntry = spineAnimationState.SetAnimation(0, nameAnim, true);
        isObject.GetComponent<PlayAnimationInZone>().isplayAnimation = true;
        isObject.GetComponent<PlayAnimationInZone>().order = 99;
        isObject.GetComponent<PlayAnimationInZone>().PlayAnimation(isObject.GetComponent<PlayAnimationInZone>()._isNameAnimation_str);

        //if have use play loop comment 2 line down
        //isPlayObject = isObject;
        //animationEntry.Complete += animationEntry_Complete;
    }
    public void animationEntry_Complete(TrackEntry trackEntry)
    {
        spineAnimationState.SetAnimation(0, "action head", true);
        isPlayObject.GetComponent<PlayAnimationInZone>().isWalk = false;
        isPlayObject.GetComponent<PlayAnimationInZone>().isplayAnimation = false;
        isPlayObject.GetComponent<PlayAnimationInZone>().PlayAnimation("idle");
        isPlayObject = null;
    }
    public void AddAnimation(string nameAnimation)
    {
        spineAnimationState.AddAnimation(0, nameAnimation, true, 1f);
    }
    public void testAnimationAllYak()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
             //spineAnimationState.SetAnimation(0, "idle", true);
             spineAnimationState.SetAnimation(0, "idle", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
             //spineAnimationState.SetAnimation(0, "walk", true);
             spineAnimationState.SetAnimation(0, "walk", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
             //spineAnimationState.SetAnimation(0, "water", true);
             spineAnimationState.SetAnimation(0, "water", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
             //spineAnimationState.SetAnimation(0, "grow", true);
             spineAnimationState.SetAnimation(0, "grow", true);
        }        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
             //spineAnimationState.SetAnimation(0, "grow", true);
             spineAnimationState.SetAnimation(0, "action head", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //spineAnimationState.SetAnimation(0, "grow", true);
            spineAnimationState.SetAnimation(0, "back_idle", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //spineAnimationState.SetAnimation(0, "grow", true);
            spineAnimationState.SetAnimation(0, "_back_arcade", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //spineAnimationState.SetAnimation(0, "grow", true);
            spineAnimationState.SetAnimation(0, "_bas", true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //spineAnimationState.SetAnimation(0, "grow", true);
            spineAnimationState.SetAnimation(0, "_boxing_back", true);
        }
    }
}
