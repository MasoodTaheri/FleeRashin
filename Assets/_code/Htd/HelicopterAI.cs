using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HelicopterMovement))]
public class HelicopterAI : MonoBehaviour
{
    [SerializeField]
    GameObject guardingPoint;
    [SerializeField]
    float guardingRange;
    [SerializeField]
    string[] focusableTargetTag = new string[0];
    [SerializeField]
    bool drawGizmos;
    HelicopterMovement hm;
    Rigidbody target;
    GunWithAutoTargetFinder[] guns;

    void Start()
    {
        hm = GetComponent<HelicopterMovement>();
        guns = GetComponents<GunWithAutoTargetFinder>();
    }
    
    void Update()
    {
        if (guardingPoint == null)
            return;

        target = FindTarget();
        if (target != null)
        {
            hm.dontMove = true;
            hm.RotateTowardTo(target.gameObject);
        }
        else
            hm.dontMove = false;

        foreach (GunWithAutoTargetFinder gun in guns)
        {
            gun.forceTarget = target;
        }
    }

    Rigidbody FindTarget()
    {
        if (target != null && Vector3.Distance(guardingPoint.transform.position, target.transform.position) < guardingRange)
            return target;

        List<Rigidbody> result = new List<Rigidbody>();
        GameObject[] tagResult;
        Rigidbody temp;

        foreach (string tag in focusableTargetTag)
        {
            tagResult = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject item in tagResult)
            {
                temp = item.GetComponent<Rigidbody>();

                if (temp != null && item.GetInstanceID() != transform.GetInstanceID())
                    result.Add(temp);
            }
        }
        // MaxDistance Filter
        for (int i = 0; i < result.Count; i++)
            if (Vector3.Distance(guardingPoint.transform.position, result[i].transform.position) > guardingRange)
                result.RemoveAt(i--);

        // Nearest Target
        temp = (result.Count > 0) ? result[0] : null;
        for (int i = 1; i < result.Count; i++)
            if (Vector3.Distance(guardingPoint.transform.position, result[i].transform.position) < Vector3.Distance(guardingPoint.transform.position, temp.transform.position))
                temp = result[i];

        return temp;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guardingPoint.transform.position, guardingRange);
    }
}
