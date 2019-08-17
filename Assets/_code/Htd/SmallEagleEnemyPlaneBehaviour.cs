using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEagleEnemyPlaneBehaviour : AIPlaneMovement
{
    [SerializeField]
    string[] targetableTags = new string[0];
    [SerializeField]
    GunWithAutoTargetFinder[] guns = new GunWithAutoTargetFinder[0];
    [SerializeField]
    Vector3 destinationOffset;
    

    private void Awake()
    {
        foreach (GunWithAutoTargetFinder gun in guns)
            gun.focusableTargetTag = targetableTags;
    }

    protected override void Update()
    {
        //if (LevelManager.Instance.PlayerPlane != null)
        //    destinationPos = LevelManager.Instance.PlayerPlane.transform.position + destinationOffset;

        //MoveForward();
        MaintainRotation();
        MaintainPosition();
    }

    void MaintainRotation()
    {
        if(LevelManager.Instance.PlayerPlane != null)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, LevelManager.Instance.PlayerPlane.transform.rotation, Time.deltaTime * RotateSpeed);
    }

    void MaintainPosition()
    {
        if (LevelManager.Instance.PlayerPlane != null)
        {

            Vector3 randomPos = new Vector3(Mathf.Sin(Time.time * 0.5f), 0, Mathf.Cos(Time.time * 0.5f));
            transform.position += ((LevelManager.Instance.PlayerPlane.transform.position + destinationOffset + randomPos) - transform.position).normalized * forwardSpeed * Time.deltaTime;
        }
        else
        {
            MoveForward();
        }
    }
}
