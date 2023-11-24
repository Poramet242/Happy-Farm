using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayAnimationInZone : MonoBehaviour
{
    [SerializeField] private Stagepoint isStage;
    [SerializeField] public bool isWalk;
    [SerializeField] public bool isplayAnimation;
    [SerializeField] public string _isNameAnimation_str;
    [SerializeField] public GameObject isObject;
    [SerializeField] public GameObject isSprite;
    [Header("Spine")]
    [SerializeField] public int order = 0;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private Spine.AnimationState spineAnimationState;
    [SerializeField] private Spine.Skeleton skeleton;
    private void Awake()
    {
        isStage = GetComponent<StagePoint>().Stage;
    }
    private void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }
    private void Update()
    {
        skeletonAnimation.GetComponentInChildren<MeshRenderer>().sortingOrder = order;
        if (isplayAnimation)
        {
            if (isStage == Stagepoint.Boxing)
            {
                return;
            }
            isObject.SetActive(true);
            isSprite.SetActive(false);
        }
        else
        {
            if (isStage == Stagepoint.Boxing)
            {
                return;
            }
            isObject.SetActive(false);
            isSprite.SetActive(true);
        }
    }
    public void PlayAnimation(string nameAnimation)
    {
        spineAnimationState.SetAnimation(0, nameAnimation, true);
    }
    public void onclickPlayAniamtion()
    {
        if (isWalk)
        {
            return;
        }
        isWalk = true;
        MovementController.instance.setPlayAnimationInZone(this.transform);
    }
}
