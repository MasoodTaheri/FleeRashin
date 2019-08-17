using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneLaserGun : MonoBehaviour
{
    [SerializeField]
    GameObject laserShotPrefab;
    [SerializeField]
    GameObject shootPoint;
    [SerializeField]
    SpriteRenderer chargedSprite;
    public int damage;

    Laser laser;

    bool isCharged;
    bool decharge;
    float dechargeDuration;
    float value;

    private void Update()
    {
        if (decharge)
        {
            value -= Time.deltaTime / dechargeDuration;

            if (value <= 0)
            {
                value = 0;
                decharge = false;
                isCharged = false;
            }

            chargedSprite.color = new Color(1f, 1f, 1f, value);
        }
    }

    public void Charge(float value)
    {
        chargedSprite.color = new Color(1, 1, 1, value);

        if(value == 1f)
            isCharged = true;
    }

    public void ShootLaser(float laserDuration)
    {
        if (!isCharged)
            return;

        laser = Instantiate(laserShotPrefab, shootPoint.transform.position, transform.rotation).GetComponent<Laser>();
        laser.SetDamage(damage);
        StartCoroutine(EndLaser(laserDuration));
    }

    IEnumerator EndLaser(float duration)
    {
        value = 1f;
        dechargeDuration = duration;
        Decharge();
        yield return new WaitForSeconds(duration);
        laser.End();
    }

    void Decharge()
    {
        decharge = true;
    }
}
