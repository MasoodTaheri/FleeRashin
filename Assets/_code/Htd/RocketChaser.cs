using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketChaser : RocketSniffer
{
    protected override void FindTarget()
    {
        if (target != null)
            return;

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
