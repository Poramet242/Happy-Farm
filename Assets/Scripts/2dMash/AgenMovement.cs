using System;
using UnityEngine;
using UnityEngine.AI;

public class AgenMovement : MonoBehaviour
{
    public static AgenMovement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    [Header("Agen Data")]
    [SerializeField] public AssisstantDetail agentDetail;
    //[SerializeField] public RoninAnimController animController;
    [SerializeField] public CharacterAnimationController characterAnimationController;
    [Header("location target move")]
    [SerializeField] private float speedMove;
    [SerializeField] public Transform agentTarget;
    NavMeshAgent agent;
    [Header("Agen Stage")]
    [SerializeField] public Stagepoint agenStage;
    [SerializeField] private Stagepoint Nextstage;
    [SerializeField] public bool playAnimation;
    [SerializeField] private bool isMoving;
    [Header("Animation")]
    [SerializeField] private bool endAnimation;
    [SerializeField] private float timePlayAnimation;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.stoppingDistance = 0.2f;
        agent.speed = speedMove;
        setAgentPosition(agentTarget);
    }
    private void Update()
    {
        if (!isMoving)
        {
            setAgenyPlayAnimation(Nextstage);
        }
        else
        {
            if (agent.velocity.x < 0f)
            {
                characterAnimationController.isfacingleft = true;
            }
            else if (agent.velocity.x > 0f)
            {
                characterAnimationController.isfacingleft = false;
            }
            setAgentPosition(agentTarget);
        }
        if (endAnimation)
        {
            MovementController.instance.CalculateWaypoint(this);
            return;
        }
    }
    public void setupDataUnitDeail(AssisstantDetail unitDetail, CharacterAnimationController anim)
    {
        characterAnimationController = anim;
        agentDetail = unitDetail;
    }
    public void getAgenTarget(Transform destinationPos)
    {
        agentTarget = destinationPos;
    }

    public void setTargetStagepoint(Stagepoint stageTemp)
    {
        agenStage = Stagepoint.Walk;
        Nextstage = stageTemp;
        isMoving = true;
        playAnimation = false;
        endAnimation = false;
        timePlayAnimation = UnityEngine.Random.Range(10f, 12f);
    }
    public void setAgentPosition(Transform target)
    {
        agent.SetDestination(target.position);
        if (Vector3.Distance(transform.position, target.position) < agent.stoppingDistance)
        {
            agenStage = Nextstage;
            //CalculateWaypoint
            if (!agent.isStopped)
            {
                isMoving = agent.isStopped;
                playAnimation = true;
            }
        }
    }
    public void setAgenyPlayAnimation(Stagepoint stagepoint)
    {
        //TODO Controller to animation agen
        //function play animation to next stage point 
        //animController.setAnimationDisplay(stagepoint);
        timePlayAnimation -= Time.deltaTime;
        if (timePlayAnimation < 0)
        {
            //isMoving = agent.isStopped;
            playAnimation = false;
            endAnimation = true;
            // opne switch and play animation loop
            switch (stagepoint)
            {
                case Stagepoint.ShootBall:
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isWalk = false;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isplayAnimation = false;
                    agentTarget.GetComponent<PlayAnimationInZone>().order = 10;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().PlayAnimation("idle");
                    break;
                case Stagepoint.PlayGame:
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isWalk = false;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isplayAnimation = false;
                    agentTarget.GetComponent<PlayAnimationInZone>().order = 10;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().PlayAnimation("idle");
                    break;
                case Stagepoint.Boxing:
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isWalk = false;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().isplayAnimation = false;
                    agentTarget.GetComponent<PlayAnimationInZone>().order = 10;
                    agentTarget.gameObject.GetComponent<PlayAnimationInZone>().PlayAnimation("idle");
                    break;
            }
            return;
        }
        else if (timePlayAnimation != 0 && playAnimation)
        {
            // play the animation once
            characterAnimationController.isfacingleft = agentTarget.GetComponent<StagePoint>()._facingleft;
            if ((agentTarget.GetComponent<StagePoint>().slotBlockPlant - 1) >= 0)
            {
                for (int i = 0; i < StakeLayerController.instance.tsc.slotsList.Count; i++)
                {
                    if (((agentTarget.GetComponent<StagePoint>().slotBlockPlant - 1) == i))
                    {
                        if (StakeLayerController.instance.tsc.slotsList[i].myData == null ||
                            StakeLayerController.instance.tsc.slotsList[i].myData.detail == null ||
                            StakeLayerController.instance.tsc.slotsList[i].myData.unitData == null)
                        {
                            characterAnimationController.PlayAnimation(EnumToString(Stagepoint.ActionHead));
                            break;
                        }
                        else
                        {
                            characterAnimationController.PlayAnimation(EnumToString(stagepoint));
                        }
                    }
                }
            }
            else if ((agentTarget.GetComponent<StagePoint>().slotBlockPlant - 1) < 0)
            {
                if (stagepoint == Stagepoint.ShootBall)
                {
                    characterAnimationController.PlayAniamtionObjectINZone(EnumToString(Stagepoint.ShootBall), agentTarget.gameObject);
                }
                else if (stagepoint == Stagepoint.PlayGame)
                {
                    characterAnimationController.PlayAniamtionObjectINZone(EnumToString(Stagepoint.PlayGame), agentTarget.gameObject);
                }
                else if (stagepoint == Stagepoint.Boxing)
                {
                    characterAnimationController.PlayAniamtionObjectINZone(EnumToString(Stagepoint.Boxing), agentTarget.gameObject);
                }
                else
                {
                    characterAnimationController.PlayAnimation(EnumToString(stagepoint));
                }
            }
            playAnimation = false;
        }
    }
    public string EnumToString(Stagepoint @enum)
    {
        return @enum switch
        {
            Stagepoint.Walk => "walk",
            Stagepoint.Idle => "idle",
            Stagepoint.Water => "water",
            Stagepoint.ActionHead => "action head",
            Stagepoint.Grow => "grow",
            Stagepoint.Trimming => "trimming",
            Stagepoint.Idle_back => "back_idle",
            Stagepoint.Arcade_back => "_back_arcade",
            Stagepoint.ShootBall => "_bas",
            Stagepoint.PlayGame => "_back_arcade",
            Stagepoint.Boxing => "_boxing_back",
        };
    }
}
