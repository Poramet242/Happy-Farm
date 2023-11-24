using System;
using UnityEngine;

public class StagePoint : MonoBehaviour
{
    [SerializeField] public Stagepoint Stage;
    [SerializeField] public bool _facingleft;
    [SerializeField] public int slotBlockPlant;
}

public enum Stagepoint
{
    Walk = 0,
    Idle = 1,
    Water = 2,
    ActionHead = 3,
    Grow = 4,
    Trimming = 5,
    Idle_back= 6,
    Arcade_back = 7,
    ShootBall = 8,
    PlayGame = 9,
    Boxing = 10,
    //CuttingPlant = 4,
    //Sitedown = 5,
    //ScratchingHead  = 8,
}

