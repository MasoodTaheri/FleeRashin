using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneRocket : BulletCode
{
    protected GameObject target;
    protected ParticleSystem ps;

    public float rotateSpeed;
    public float sensorRange;
    public float sensorAngle;
    [HideInInspector]
    public int playerInstanceID;
    public string[] focusableTargetTag = new string[0];


    Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    float Randomsgn(float min, float max)
    {
        float sign = Mathf.Sign(Random.Range(-100.0f, 100.0f));
        float number = Random.Range(min, max);

        return sign * number;
    }
    
    protected virtual void  FindTarget()
    {
        List<GameObject> enemyList = new List<GameObject>();
        
        GameObject[] tempTag;
        for (int i = 0; i < focusableTargetTag.Length; i++)
        {
            tempTag = GameObject.FindGameObjectsWithTag(focusableTargetTag[i]);
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

    protected override void Move()
    {
        FindTarget();

        if (target != null)
        {
            Vector3 curEnemyPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            Vector3 relativePos = curEnemyPos - transform.position;
            //Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        }

        rb.velocity = transform.forward * Speed;
    }

    void OnDestroy()
    {
        if(this.enabled)
            GameObject.Destroy(GameObject.Instantiate(HitParticle, transform.position, Quaternion.identity) as GameObject, 5);
    }

    private void OnDrawGizmo()
    { 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sensorRange);

        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

}
