using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaglePlane : PlaneBase
{
    [SerializeField]
    EagleGun rightGun;
    [SerializeField]
    EagleGun leftGun;
    [SerializeField]
    GameObject rocketPrefab;
    [SerializeField]
    Sprite secondaryWeaponUISprite;
    [SerializeField]
    int rocketCount;
    [SerializeField]
    float rocketCooldownDuration;
    float rocketCooldown;
    [SerializeField]
    string[] targetableTags = new string[0];

    EagleWayPoint[] waypointArray;
    int waypointIndex = 0;
    
    InGamePanel uiPanel;
    InGamePanel UIPanel
    {
        get
        {
            if (uiPanel == null)
                uiPanel = FindObjectOfType<InGamePanel>();
            return uiPanel;
        }
    }

    private void Awake()
    {
        desObj = GetComponent<DestroyableObject>();
        pm = GetComponent<PlaneMovement>();
    }

    private void Start()
    {
        UpdateUI();
        UIPanel.SetSecondaryWeaponImage(secondaryWeaponUISprite);

        //Find all waypoints and sort them by theire index
        waypointArray = FindObjectsOfType<EagleWayPoint>();
        EagleWayPoint temp;

        for (int i = 0; i < waypointArray.Length; i++)
        {
            for (int j = i; j < waypointArray.Length; j++)
            {
                if (waypointArray[j].index < waypointArray[i].index)
                {
                    temp = waypointArray[i];
                    waypointArray[i] = waypointArray[j];
                    waypointArray[j] = temp;
                }
            }
        }

        if (waypointIndex < waypointArray.Length)
            SetEagleForwardFoewardSpeed(waypointArray[waypointIndex].eagleForwardSpeed);
    }

    private void Update()
    {

        if (rocketCount > 0 && rocketCooldown > 0)
        {
            rocketCooldown -= Time.deltaTime;
            UpdateUI();
        }

        Move();

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootSecondaryWeapon();
        }
    }

    void Move()
    {
        if (waypointArray == null || waypointIndex >= waypointArray.Length)
            return;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(waypointArray[waypointIndex].transform.position - transform.position), Time.deltaTime * pm.RotateSpeed);

        if (Vector3.Distance(transform.position, waypointArray[waypointIndex].transform.position) < waypointArray[waypointIndex].reachRadius)
        {
            waypointIndex++;
            if (waypointIndex < waypointArray.Length)
                SetEagleForwardFoewardSpeed(waypointArray[waypointIndex].eagleForwardSpeed);
        }
    }

    void SetEagleForwardFoewardSpeed(float newSpeed)
    {
        if (newSpeed <= 0)
            return;

        pm.forwardSpeed = newSpeed;
    }

    public override Transform GetLeftSteerObject()
    {
        return leftGun.GetbarrelTransform();
    }

    public override Transform GetRightSteerObject()
    {
        return rightGun.GetbarrelTransform();
    }

    public override void ShootSecondaryWeapon()
    {
        if (rocketCount <= 0 || rocketCooldown > 0)
            return;

        Vector3 rotation = Vector3.zero; 
        GameObject rocket;
        for (int i = 0; i < 8; i++)
        {
            rocket = Instantiate(rocketPrefab, transform.position + transform.forward * 1f, transform.rotation) as GameObject;
            rocket.transform.eulerAngles = rotation;
            rocket.GetComponent<RocketBase>().SetRocketData(gameObject.GetInstanceID(), targetableTags);
            rotation.y += 45;
        }

        rocketCooldown = rocketCooldownDuration;
        rocketCount--;
        UpdateUI();
    }

    public override void SteerLeft(float value, bool isHolding = false)
    {
        leftGun.RotateBarrel(value, isHolding);
    }

    public override void SteerRight(float value, bool isHolding = false)
    {
        rightGun.RotateBarrel(value, isHolding);
    }

    void UpdateUI()
    {
        UIPanel.SetSecondaryWeaponAmmuCount(rocketCount);
        UIPanel.SetSecondaryWeaponCooldown(1 - (rocketCooldown / rocketCooldownDuration));
    }
}
