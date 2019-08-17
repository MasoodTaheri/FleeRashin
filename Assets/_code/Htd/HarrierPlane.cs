using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarrierPlane : PlaneBase
{
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

    protected GunWithAutoTargetFinder[] guns;
    InGamePanel uiPanel;
    InGamePanel UIPanel
    {
        get {
            if (uiPanel == null)
                uiPanel = FindObjectOfType<InGamePanel>();
            return uiPanel;
        }
    }

    private void Awake()
    {
        desObj = GetComponent<DestroyableObject>();
        guns = GetComponents<GunWithAutoTargetFinder>();
        pm = GetComponent<PlaneMovement>();

        foreach (GunWithAutoTargetFinder gun in guns)
            gun.focusableTargetTag = targetableTags;
    }

    private void Start()
    {
        UpdateUI();
        UIPanel.SetSecondaryWeaponImage(secondaryWeaponUISprite);
    }

    private void Update()
    {
        if (rocketCount > 0 && rocketCooldown > 0)
        {
            rocketCooldown -= Time.deltaTime;
            UpdateUI();
        }

        CheckInputs();
    }

    void CheckInputs()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pm.Steer(PlaneMovement.PlaneSteerDirection.Right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            pm.Steer(PlaneMovement.PlaneSteerDirection.Left);
        }
        else
            pm.Steer(0f);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootSecondaryWeapon();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pm.forwardSpeed *= 5;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            pm.forwardSpeed /= 5;
        }
    }

    public override void SteerLeft(float value, bool isHolding = false)
    {
        pm.Steer(value);
    }
    
    public override Transform GetLeftSteerObject()
    {
        return transform;
    }

    public override void ShootSecondaryWeapon()
    {
        if (rocketCount <= 0 || rocketCooldown > 0)
            return;

        GameObject rocket = Instantiate(rocketPrefab, transform.position + transform.forward * 1f, transform.rotation) as GameObject;
        rocket.GetComponent<RocketBase>().SetRocketData(gameObject.GetInstanceID(), targetableTags);
        rocketCooldown = rocketCooldownDuration;
        rocketCount--;
        UpdateUI();
    }

    void UpdateUI()
    {
        UIPanel.SetSecondaryWeaponAmmuCount(rocketCount);
        UIPanel.SetSecondaryWeaponCooldown(1 - (rocketCooldown / rocketCooldownDuration));
    }

    private void OnDestroy()
    {
        if(LevelManager.Instance != null)
            LevelManager.Instance.GameOver();
    }

    public override void SteerRight(float value, bool isHolding = false)
    {
        throw new System.NotImplementedException();
    }

    public override Transform GetRightSteerObject()
    {
        throw new System.NotImplementedException();
    }
}
