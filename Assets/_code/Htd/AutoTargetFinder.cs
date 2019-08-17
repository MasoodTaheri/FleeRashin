using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(gun))]
public class AutoTargetFinder : MonoBehaviour
{
    [SerializeField]
    bool isActive;
    [SerializeField][Range(0,180)]
    float shootRangeAngle = 30f;
    [SerializeField][Range(0,10)]
    float shootRangeDistance = 5f;
    [SerializeField]
    string[] targetableTags = new string[0];
    [SerializeField]
    GameObject targetIndicatorPrefab;
    GameObject targetIndicator;

    gun _gun;
    List<GameObject> targets = new List<GameObject>();

    public bool justShoot;

    private void Start()
    {
        _gun = GetComponent<gun>();

        GameObject[] potentialTargets;
        targets.Clear();

        for (int i = 0; i < targetableTags.Length; i++)
        {
            potentialTargets = GameObject.FindGameObjectsWithTag(targetableTags[i]);

            foreach (GameObject item in potentialTargets)
            {
                if (item.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                targets.Add(item);
            }
        }
    }

    private void Update()
    {
        if (!isActive)
            return;

        if (targets.Count == 0)
            Start();

        GameObject target = AcquireTarget();
        if (target != null)
        {
            _gun.SetTarget(target);
            SetIndicatorPosition(target);
            _gun.forceShoot = true;
        }
        else
        {
            HideIndicator();
            _gun.forceShoot = false;
        }

        _gun.forceShoot = _gun.forceShoot || justShoot;
    }

    GameObject AcquireTarget()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null)
                targets.RemoveAt(i--);
        }

        //angle
        for (int i = 0; i < targets.Count; i++)
        {
            float angle = Mathf.Abs(Vector3.Angle(transform.forward, (CorrectPosition(targets[i].transform.position) - transform.position)));
            if (angle > shootRangeAngle / 2 || angle > 360f - shootRangeAngle / 2)
                targets.RemoveAt(i--);
        }
        //distance
        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(transform.position, CorrectPosition(targets[i].transform.position)) > shootRangeDistance)
                targets.RemoveAt(i--);
        }
        //nearest
        if (targets.Count > 0)
        {
            int closestTargetIndex = 0;
            for (int i = 1; i < targets.Count; i++)
            {
                if (Vector3.Distance(transform.position, CorrectPosition(targets[i].transform.position)) < Vector3.Distance(transform.position, CorrectPosition(targets[closestTargetIndex].transform.position)))
                    closestTargetIndex = i;
            }

            return targets[closestTargetIndex];

        }
        else
            return null;
    }

    Vector3 CorrectPosition(Vector3 input)
    {
        input = new Vector3(input.x, transform.position.y, input.z);
        return input;
    }

    void SetIndicatorPosition(GameObject _target)
    {
        if (targetIndicator == null)
            targetIndicator = Instantiate(targetIndicatorPrefab);

        targetIndicator.SetActive(true);
        targetIndicator.transform.SetParent(_target.transform, true);
        targetIndicator.transform.localPosition = Vector3.zero;
    }

    void HideIndicator()
    {
        if (targetIndicator != null)
            targetIndicator.SetActive(false);
    }

    public void AddTargetableTag(string newTag)
    {
        GameObject[] newTagPotentialTarget = GameObject.FindGameObjectsWithTag(newTag);

        foreach (GameObject item in newTagPotentialTarget)
        {
            if (item.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                continue;

            targets.Add(item);
        }
    }
}
