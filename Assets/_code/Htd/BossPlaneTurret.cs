using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneTurret : MonoBehaviour
{
    float rotateSpeed;
    [SerializeField]
    Transform barrel;
    protected GameObject target;
    //public float shootRange = 10f;
    [SerializeField]
    [Range(0, 90)] float barrelRotateSpeed;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    GameObject[] shootPoint;
    [SerializeField]
    Animator[] barrelAnimator;
    [SerializeField]
    float bulletSpeed;
    [SerializeField]
    float bulletLifeTime;
    [SerializeField]
    int bulletDamage;
    [Range(0,2f)]
    [SerializeField]
    float shootCooldown;
    protected float lastShootTime;
    int remainingBullet;
    [Range(0, 20f)]
    [SerializeField]
    float reloadInterval;
    protected float lastReloadTime;
    [Range(0, 20f)]
    [SerializeField]
    float readyToShootInterval;
    [Range(0, 20)]
    [SerializeField]
    int clipSize;
    [SerializeField]
    bool startwithEmptyClip;
    int barrelIndex = 0;

    protected virtual void Start()
    {
        if (startwithEmptyClip)
            StartCoroutine(Reload());
        else
        {
            remainingBullet = clipSize;
            lastReloadTime = Time.time;
        }
    }
    private void Update()
    {
        if (target == null)
        {
            if (FindObjectOfType<DefaultPlayerPlane>() == null)
                return;

            target = FindObjectOfType<DefaultPlayerPlane>().gameObject;
        }

        //barrel.eulerAngles = Quaternion.LookRotation(target.transform.position - barrel.position).eulerAngles;
        barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, Quaternion.LookRotation(target.transform.position - barrel.transform.position), Time.deltaTime * barrelRotateSpeed);

        if (Time.time - lastShootTime > shootCooldown && remainingBullet > 0 && Time.time - lastReloadTime > readyToShootInterval)
        {
            ShootBullet(barrelIndex);
            barrelIndex = (barrelIndex + 1) % barrelAnimator.Length;
            lastShootTime = Time.time;
        }
    }

    protected void ShootBullet(int barrelIndex)
    {
        GameObject Bullet1 = Instantiate(bulletPrefab) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(bulletSpeed, bulletLifeTime, shootPoint[barrelIndex].transform.position, barrel.rotation, bulletDamage);
        barrelAnimator[barrelIndex].SetTrigger("Shoot");
        remainingBullet--;

        if (remainingBullet <= 0)
            StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadInterval - (clipSize-1) * shootCooldown);
        remainingBullet = clipSize;
        lastReloadTime = Time.time;
    }
}
