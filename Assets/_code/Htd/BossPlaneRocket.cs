using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneRocket : PlayerPlaneRocket
{
    public GameObject flare;
    public void SetUpToLaunch()
    {
        transform.GetChild(1).gameObject.SetActive(true);
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    protected override void FindTarget()
    {
        if (flare != null)
        {
            target = flare;
            return;
        }

        List<GameObject> enemyList = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Playerbody");

        foreach (GameObject item in enemies)
        {
            if (item.gameObject.GetInstanceID() == playerInstanceID)
                continue;

            enemyList.Add(item);
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
