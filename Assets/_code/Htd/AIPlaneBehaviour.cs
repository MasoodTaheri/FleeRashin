using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
[RequireComponent(typeof(AIPlaneMovement))]
public class AIPlaneBehaviour : MonoBehaviour
{
    public enum EnemyPlaneState { Patrol, TakeOff, chasingEnemy, Evade, FlyToward}
    AIPlaneMovement pM;
    public GameObject target;
    public Vector3 targetPos;
    EnemyPlaneState state = EnemyPlaneState.Patrol;

    public float sensorRange;
    public string[] targetableTags = new string[0];

    DestinationPoint patrolPoint;
    List<DestinationPoint> patrolSubPoints = new List<DestinationPoint>();
    int patrolSubPointIndex;
    const int patrolPointCount = 128;

    AIGroupController agc;
    
    private void Start()
    {
        pM = GetComponent<AIPlaneMovement>();

        ChangeState(EnemyPlaneState.Patrol);
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyPlaneState.Patrol:
                
                if (patrolSubPoints[patrolSubPointIndex].IsInRange(transform.position))
                {
                    patrolSubPointIndex = (patrolSubPointIndex + 1) % patrolSubPoints.Count;
                    pM.destinationPos = patrolSubPoints[patrolSubPointIndex].PointPos;
                }

                break;
            case EnemyPlaneState.TakeOff:
                break;
            case EnemyPlaneState.chasingEnemy:
                if (target == null)
                    ChangeState(EnemyPlaneState.Patrol);
                else
                {
                    pM.destinationPos = target.transform.position;
                }
                break;
            case EnemyPlaneState.Evade:
                break;
            case EnemyPlaneState.FlyToward:
                pM.destinationPos = targetPos;
                
                break;
            default:
                break;
        }

        CheckSensor();
    }

    public void ChangeState(EnemyPlaneState nextState)
    {
        switch (nextState)
        {
            case EnemyPlaneState.Patrol:

                patrolPoint = new DestinationPoint(transform.position, 4);
                SetPatrolSubPoints();
                patrolSubPointIndex = patrolPointCount / 2;
                pM.destinationPos = patrolSubPoints[patrolSubPointIndex].PointPos;
                state = EnemyPlaneState.Patrol;
                break;
            case EnemyPlaneState.TakeOff:
                break;
            case EnemyPlaneState.chasingEnemy:
                state = EnemyPlaneState.chasingEnemy;
                break;
            case EnemyPlaneState.Evade:
                break;
            case EnemyPlaneState.FlyToward:
                state = EnemyPlaneState.FlyToward;
                break;
            default:
                break;
        }
    }

    void SetPatrolSubPoints()
    {
        float range = patrolPoint.range / 4;
        patrolSubPoints.Clear();
        Vector3 point = new Vector3();
        for (int i = 0; i < patrolPointCount; i++)
        {
            point.x = patrolPoint.PointPos.x + Mathf.Cos((Mathf.PI / (patrolPointCount / 2)) * i) * patrolPoint.range;
            point.y = transform.position.y;
            point.z = patrolPoint.PointPos.z + Mathf.Sin((Mathf.PI / (patrolPointCount / 2)) * i) * patrolPoint.range;

            patrolSubPoints.Add(new DestinationPoint(point, range));
        }
    }

    void CheckSensor()
    {
        foreach (string tag in targetableTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject potentialTarget in potentialTargets)
            {
                if (Vector3.Distance(transform.position, potentialTarget.transform.position) < sensorRange)
                {
                    //target = potentialTarget;
                    agc.EnemySpotted(potentialTarget);
                    //ChangeState(EnemyPlaneState.chasingEnemy);
                    return;
                }
            }
        }
    }

    public void SetAIGroupController(AIGroupController _agc) { agc = _agc;}
}
