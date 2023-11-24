using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    public static MovementController instance;
    [Header("Data Assistants")]
    [SerializeField] public UnitDetail _assisDetail;
    [SerializeField] public List<GameObject> _assisObj = new List<GameObject>();
    [SerializeField] public List<AgenMovement> _assisObj_agent = new List<AgenMovement>();
    [Header("Waypoint")]
    [SerializeField] public List<Transform> waypoint = new List<Transform>();
    List<Transform> availableSpawnPoints = new List<Transform>();
    List<Transform> countWaypoint = new List<Transform>();
    [Header("Object")]
    [SerializeField] private AgenMovement unitPrefabs;
    [SerializeField] private Transform unitPatent;
    [SerializeField] private int maxUnit;
    //[Header("Unit Spawn")]
    //[SerializeField] private List<GameObject> unitList = new List<GameObject>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {

    }
    private void Update()
    {
        for (int i = 0; i < _assisObj.Count; i++)
        {
            if (_assisObj[i] == null)
            {
                _assisObj.RemoveAt(i);
            }
        }
    }

    public IEnumerator setupInstanceAssistants(AssisstantDetail detail)
    {
        yield return SpawnUnit(detail._unitPrefab, detail, () => { });
    }
    public void clearDataAssistant(AssisstantDetail unitDetail)
    {
        for (int i = 0; i < _assisObj.Count; i++)
        {
            if (_assisObj[i].GetComponent<AgenMovement>().agentDetail._unitTokenID == unitDetail._unitTokenID)
            {
                Destroy(_assisObj[i]);
            }
        }
    }
    public IEnumerator SpawnUnit(GameObject temp, AssisstantDetail unitDetail,Action callback)
    {
        // randomly select a spawn point from the available spawn points
        if (availableSpawnPoints.Count == 0)
        {
            availableSpawnPoints = waypoint;
            countWaypoint = waypoint;
        }
        int randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
        Transform spawnTransform = availableSpawnPoints[randomIndex];
        for (int a = 0; a < _assisObj_agent.Count; a++)
        {
            // select a new random index and spawnTransform if the current one is already assigned
            if (availableSpawnPoints.Count == 1)
            {
                // no available spawn points left, break out of the loop
                break;
            }
            while (_assisObj_agent.Any(agent => agent.agentTarget == spawnTransform))
            {
                randomIndex = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
                spawnTransform = availableSpawnPoints[randomIndex];
            }
        }
        // instantiate the object at the selected spawn point
        AgenMovement unitTemp = Instantiate(unitPrefabs, spawnTransform.position, Quaternion.identity, unitPatent);
        unitTemp.name = unitDetail.name;
        GameObject _assistants = Instantiate(temp, unitTemp.transform);
        unitTemp.setupDataUnitDeail(unitDetail, _assistants.GetComponent<CharacterAnimationController>());
        unitTemp.getAgenTarget(spawnTransform);
        unitTemp.setTargetStagepoint(spawnTransform.GetComponent<StagePoint>().Stage);
        _assisObj.Add(unitTemp.gameObject);
        _assisObj_agent.Add(unitTemp);
        callback?.Invoke();
        yield break;
        #region old SpawnUnit
        /*// loop to instantiate the objects
        //for (int i = 0; i < 1; i++)
        //{
        // randomly select a spawn point from the available spawn points
        if (availableSpawnPoints.Count ==0)
            {
                availableSpawnPoints = waypoint;
                countWaypoint = waypoint;
            }
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnTransform = availableSpawnPoints[randomIndex];
           // if (_assisObj_agent.Count > 1)
            //{
                for (int a = 0; a < _assisObj_agent.Count; a++)
                {
                    //if (spawnTransform == _assisObj_agent[i].agentTarget)
                   // {
                        // select a new random index and spawnTransform if the current one is already assigned
                        if (availableSpawnPoints.Count == 1)
                        {
                            // no available spawn points left, break out of the loop
                            break;
                        }
                        while (_assisObj_agent.Any(agent => agent.agentTarget == spawnTransform))
                        {
                            randomIndex = Random.Range(0, availableSpawnPoints.Count);
                            spawnTransform = availableSpawnPoints[randomIndex];
                        }
                   // }
                }
           // }
            // instantiate the object at the selected spawn point
            AgenMovement unitTemp = Instantiate(unitPrefabs, spawnTransform.position, Quaternion.identity, unitPatent);
            unitTemp.name = unitDetail.name;
            GameObject _assistants = Instantiate(temp, unitTemp.transform);
            unitTemp.setupDataUnitDeail(unitDetail, _assistants.GetComponent<CharacterAnimationController>());
            unitTemp.getAgenTarget(spawnTransform);
            unitTemp.setTargetStagepoint(spawnTransform.GetComponent<StagePoint>().Stage);
            _assisObj.Add(unitTemp.gameObject);
            _assisObj_agent.Add(unitTemp);
        //CalculateWaypoint(unitTemp);
        //remove the selected spawn point from the available spawn points
        //availableSpawnPoints.RemoveAt(randomIndex);
        //}*/
        #endregion
    }
    public void CalculateWaypoint(AgenMovement tempAgen)
    {
        int go = UnityEngine.Random.Range(0, waypoint.Count);
        Transform nextMove = waypoint[go];

        for (int a = 0; a < _assisObj_agent.Count; a++)
        {
            if (nextMove == _assisObj_agent[a].agentTarget)
            {
                // If the current waypoint is already assigned to another agent, 
                // select a new random waypoint that is not currently assigned
                while (_assisObj_agent.Any(agent => agent.agentTarget == nextMove))
                {
                    go = UnityEngine.Random.Range(0, waypoint.Count);
                    nextMove = waypoint[go];
                }
            }
        }

        tempAgen.getAgenTarget(nextMove);
        tempAgen.characterAnimationController.PlayAnimation("walk");
        tempAgen.setTargetStagepoint(nextMove.GetComponent<StagePoint>().Stage);
    }

    public void setPlayAnimationInZone(Transform transform)
    {
        if(_assisObj_agent.Count <= 0)
        {
            return;
        }
        AgenMovement agenMovement = _assisObj_agent[UnityEngine.Random.Range(0, _assisObj_agent.Count)];
        PlayAnimationObject(agenMovement, transform);
    }

    public void PlayAnimationObject(AgenMovement tempAgen, Transform target)
    {
        tempAgen.getAgenTarget(target);
        tempAgen.characterAnimationController.PlayAnimation("walk");
        tempAgen.setTargetStagepoint(target.GetComponent<StagePoint>().Stage);
    }
}
