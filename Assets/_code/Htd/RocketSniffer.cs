using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSniffer : RocketBase
{
    [SerializeField]
    protected float rotateSpeed;
    [SerializeField]
    protected float sensorRange;
    [SerializeField]
    float sensorAngle;
    
    protected override void Update()
    {
        base.Update();
        Rotate();
    }

    protected virtual void Rotate()
    {
        FindTarget();

        if (target != null)
        {
            Quaternion desireRotation = Quaternion.LookRotation(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desireRotation, Time.deltaTime * rotateSpeed);
        }
    }

    protected virtual void FindTarget()
    {
        List<GameObject> enemyList = new List<GameObject>();

        GameObject[] tempTag;
        for (int i = 0; i < targetableTags.Length; i++)
        {
            tempTag = GameObject.FindGameObjectsWithTag(targetableTags[i]);
            foreach (GameObject item in tempTag)
            {
                if (item.gameObject.GetInstanceID() == playerInstanceID)
                    continue;

                enemyList.Add(item);
            }
        }

        //angle
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 curEnemyPos = new Vector3(enemyList[i].transform.position.x, transform.position.y, enemyList[i].transform.position.z);
            Vector2 temp = new Vector2((curEnemyPos - transform.position).x, (curEnemyPos - transform.position).z);
            float angle = Mathf.Abs(Vector2.Angle(new Vector2(transform.forward.x, transform.forward.z), temp));

            if (angle > sensorAngle / 2 || angle > 360f - sensorAngle / 2)
                enemyList.RemoveAt(i--);
        }

        //distance
        for (int i = 0; i < enemyList.Count; i++)
        {
            Vector3 curEnemyPos = new Vector3(enemyList[i].transform.position.x, transform.position.y, enemyList[i].transform.position.z);
            if (Vector3.Distance(transform.position, curEnemyPos) > sensorRange)
                enemyList.RemoveAt(i--);
        }

        //closest target
        if (enemyList.Count > 0)
        {
            int closestTargetIndex = 0;
            for (int i = 1; i < enemyList.Count; i++)
            {
                Vector3 curEnemyPos = new Vector3(enemyList[i].transform.position.x, transform.position.y, enemyList[i].transform.position.z);
                Vector3 closestEnemyPos = new Vector3(enemyList[closestTargetIndex].transform.position.x, transform.position.y, enemyList[closestTargetIndex].transform.position.z);

                if (Vector3.Distance(transform.position, curEnemyPos) < Vector3.Distance(transform.position, closestEnemyPos))
                    closestTargetIndex = i;
            }

            target = enemyList[closestTargetIndex];

        }
        else
            target = null;
    }
}
