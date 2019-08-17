using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneLaser : MonoBehaviour
{
    enum LaserState {Off, On, Charging, TransferingPower, ChargingGuns, ShootGuns, GettingReadyForRecharge};

    [SerializeField]
    BossPlaneLaserGun[] leftWingLaserGuns = new BossPlaneLaserGun[0];
    [SerializeField]
    BossPlaneLaserGun[] righttWingLaserGuns = new BossPlaneLaserGun[0];
    [SerializeField]
    SpriteRenderer[] lineParts = new SpriteRenderer[0];
    [SerializeField]
    SpriteRenderer chargerSprite;
    [SerializeField]
    Animator[] chargerAnimator = new Animator[0];
    [SerializeField]
    DestroyableObject detObj;
    [SerializeField]
    GameObject hpBar;
    [SerializeField]
    [Range(0, 20)] float gettingReadyForRechargeDuration;
    [SerializeField]
    [Range(0, 20)] float chargingDuration;
    [SerializeField]
    [Range(0, 2)] float shootInterval;
    [SerializeField]
    [Range(0, 5)] float laserDuration;
    [SerializeField]
    int laserDamage;
    [SerializeField]
    string mortalTag;
    [SerializeField]
    bool invertShootOrder;

    float chargeValue;
    LaserState state = LaserState.Off;
    float lastShootTime;
    int focusedGunIndex;

    private void Start()
    {
        detObj.OnDamageCallBack = OnDamageApplied;
        detObj.GetComponent<BoxCollider>().enabled = false;
        detObj.enabled = false;
        hpBar.SetActive(false);

        Color offColor = new Color(1, 1, 1, 0);

        foreach (SpriteRenderer item in lineParts)
            item.color = offColor;

        chargerSprite.color = offColor;

        foreach (Animator item in chargerAnimator)
            item.enabled = false;

        foreach (BossPlaneLaserGun gun in leftWingLaserGuns)
            gun.damage = laserDamage;
        foreach (BossPlaneLaserGun gun in righttWingLaserGuns)
            gun.damage = laserDamage;

    }

    private void Update()
    {
        if (chargerSprite == null)
            state = LaserState.Off;

        switch (state)
        {
            case LaserState.On:

                foreach (Animator item in chargerAnimator)
                    item.enabled = true;

                focusedGunIndex = (invertShootOrder) ? leftWingLaserGuns.Length - 1 : 0;
                chargeValue = 0;
                state = LaserState.Charging;
                break;
            case LaserState.Charging:

                if (chargeValue < 1f)
                {
                    chargeValue += Time.deltaTime / chargingDuration;
                    if (chargeValue >= 1f)
                    {
                        chargeValue = 1f;
                        state = LaserState.TransferingPower;

                        foreach (Animator item in chargerAnimator)
                            item.enabled = false;
                    }

                    chargerSprite.color = new Color(1, 1, 1, chargeValue);
                }
                break;
            case LaserState.TransferingPower:
                if (chargeValue > 0)
                {
                    chargeValue -= 2 * Time.deltaTime / chargingDuration;
                    if (chargeValue <= 0)
                    {
                        chargeValue = 0;
                        state = LaserState.ChargingGuns;
                    }

                    chargerSprite.color = new Color(1, 1, 1, chargeValue);
                    Color lineColor = new Color(1, 1, 1, 1f - chargeValue);

                    foreach (SpriteRenderer item in lineParts)
                        item.color = lineColor;

                }
                break;
            case LaserState.ChargingGuns:
                chargeValue += 2 * Time.deltaTime / chargingDuration;
                if (chargeValue >= 1)
                {
                    chargeValue = 1;
                    state = LaserState.ShootGuns;
                }

                Color linesColor = new Color(1, 1, 1, 1f - chargeValue);
                foreach (SpriteRenderer item in lineParts)
                    item.color = linesColor;
                foreach (BossPlaneLaserGun item in leftWingLaserGuns)
                    item.Charge(chargeValue);
                foreach (BossPlaneLaserGun item in righttWingLaserGuns)
                    item.Charge(chargeValue);

                break;
            case LaserState.ShootGuns:

                if (Time.time - lastShootTime > shootInterval)
                {
                    leftWingLaserGuns[focusedGunIndex].ShootLaser(laserDuration);
                    righttWingLaserGuns[focusedGunIndex].ShootLaser(laserDuration);
                    focusedGunIndex += (invertShootOrder) ? -1 : 1;
                    lastShootTime = Time.time;

                    if ((invertShootOrder && focusedGunIndex < 0) || (!invertShootOrder && focusedGunIndex >= leftWingLaserGuns.Length))
                        state = LaserState.GettingReadyForRecharge;

                }
                break;
            case LaserState.GettingReadyForRecharge:
                StartCoroutine(GettingReadyForRecharge());
                break;
            case LaserState.Off:
            default:
                foreach (SpriteRenderer line in lineParts)
                    line.color = new Color(1, 1, 1, 0);
                foreach (BossPlaneLaserGun gun in leftWingLaserGuns)
                    gun.Charge(0);
                foreach (BossPlaneLaserGun gun in righttWingLaserGuns)
                    gun.Charge(0);
                break;
        }
    }

    IEnumerator GettingReadyForRecharge()
    {
        yield return new WaitForSeconds(gettingReadyForRechargeDuration);
        state = LaserState.On;
    }

    public void SetLaserActive(bool value = true)
    {
        state = (value) ? LaserState.On : LaserState.Off;
    }

    public void OnDamageApplied(float hpPercentage)
    {
        if (hpPercentage <= 0)
        {
            GetComponent<BossPlane>().OnLaserDisable();
        }
    }

    public void MakeItMortal()
    {
        detObj.enabled = true;
        hpBar.SetActive(true);
        detObj.GetComponent<BoxCollider>().enabled = true;
        detObj.tag = mortalTag;
    }
}
