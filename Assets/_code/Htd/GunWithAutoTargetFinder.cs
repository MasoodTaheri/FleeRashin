using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GunWithAutoTargetFinder : MonoBehaviour
{
    [SerializeField]
    bool isEnable = true;
    [Space(10)]
    [SerializeField]
    GameObject shootPoint;
    [SerializeField]
    [Range(1, 30)] int clipSize = 1;
    [SerializeField]
    [Range(0, 2)] float shootInterval;
    [SerializeField]
    [Range(0, 30)] float reloadTime;
    [Space(10)]
    [SerializeField]
    [Range(0, 20)] float maxDistance;
    [SerializeField]
    [Range(0, 90)] float minAngle;
    [SerializeField]
    GameObject barrel;
    [SerializeField]
    [Range(0, 360)] float barrelRotateSpeed;
    [SerializeField]
    [Range(0, 360)] float maxAngle;
    [Space(10)]
    [SerializeField]
    BulletCode bulletPrefab;
    [SerializeField]
    [Range(0, 20)] float bulletSpeed;
    [SerializeField]
    [Range(0, 15)] float bulletLifeTime;
    [SerializeField]
    [Range(0, 50)] int bulletDamage;
    [Space(20)]
    [SerializeField]
    Animator barrelAnimator;
    [SerializeField]
    string triggerName;
    [Space(20)]
    [SerializeField]
    GunWithAutoTargetFinder syncGun;

    float lastShootTime;
    int bulletCount;
    Rigidbody target;
    [HideInInspector]
    public Rigidbody forceTarget;
    [HideInInspector]
    public bool canShoot = true;
    //[HideInInspector]
    public string[] focusableTargetTag = new string[0];

    Action<GameObject> onTargetFoundCallback;
    public Action<GameObject> OnTargetFoundCallback { set { onTargetFoundCallback = value; } }

    private void Start()
    {
        lastShootTime = 0;
        bulletCount = clipSize;

        if (syncGun != null)
            syncGun.canShoot = false;
    }

    private void Update()
    {
        if (!isEnable)
            return;
        
        if (syncGun != null)
            syncGun.canShoot = false;

        if (forceTarget == null)
            target = FindTarget();
        else
            target = forceTarget;

        RotateBarrelTowardTarget();

        if (CanShootTarget() && (syncGun == null || (syncGun != null && syncGun.CanShootTarget())))
        {
            if (syncGun != null)
                syncGun.canShoot = true;

            Shoot();

            if (bulletCount <= 0)
                StartCoroutine(Reload());
        }
    }

    Rigidbody FindTarget()
    {
        if (target != null && Vector3.Distance(shootPoint.transform.position, CorrectPosition(target.transform.position)) < maxDistance &&
            Vector3.Angle(transform.forward, CorrectPosition(target.transform.position) - shootPoint.transform.position) < maxAngle / 2)
        {
            if (syncGun != null)
                syncGun.forceTarget = target;

            return target;
        }

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
            if (Vector3.Distance(shootPoint.transform.position, CorrectPosition(result[i].transform.position)) > maxDistance)
                result.RemoveAt(i--);
        // MaxAngle Filter
        for (int i = 0; i < result.Count; i++)
            if (Vector3.Angle(transform.forward, CorrectPosition(result[i].transform.position) - shootPoint.transform.position) > maxAngle / 2f)
                result.RemoveAt(i--);
        // Nearest Target
        temp = (result.Count > 0) ? result[0] : null;
        for (int i = 1; i < result.Count; i++)
            if (Vector3.Distance(shootPoint.transform.position, CorrectPosition(result[i].transform.position)) < Vector3.Distance(shootPoint.transform.position, CorrectPosition(temp.transform.position)))
                temp = result[i];

        if (onTargetFoundCallback != null)
            onTargetFoundCallback.Invoke(temp.gameObject);

        if (syncGun != null)
            syncGun.forceTarget = temp;

        return temp;
    }

    void RotateBarrelTowardTarget()
    {
        Vector3 aimongPoint;
        float aimingAid;

        if (target != null)
        {
            aimongPoint = CorrectPosition(target.transform.position);
            aimingAid = CalculateAimingAid();
            aimongPoint += target.velocity * aimingAid;  //need improvment
        }
        else
        {
            aimongPoint = transform.forward + barrel.transform.position;
        }

        barrel.transform.rotation = Quaternion.RotateTowards(barrel.transform.rotation, Quaternion.LookRotation(aimongPoint - barrel.transform.position), Time.deltaTime * barrelRotateSpeed);
        
        if (barrel.transform.localEulerAngles.y > maxAngle / 2 && barrel.transform.localEulerAngles.y < 180f)
            barrel.transform.localEulerAngles = new Vector3(barrel.transform.localEulerAngles.x, maxAngle / 2, barrel.transform.localEulerAngles.z);
        else if (barrel.transform.localEulerAngles.y < 360f-maxAngle / 2 && barrel.transform.localEulerAngles.y > 180f)
            barrel.transform.localEulerAngles = new Vector3(barrel.transform.localEulerAngles.x, -maxAngle / 2, barrel.transform.localEulerAngles.z);
    }

    void Shoot()
    {
        if (!canShoot)
            return;

        if (barrelAnimator != null)
        {
            barrelAnimator.SetTrigger(triggerName);
        }

        BulletCode bullet = Instantiate<BulletCode>(bulletPrefab);
        bullet.Initalize(bulletSpeed, bulletLifeTime, shootPoint.transform.position, barrel.transform.rotation, bulletDamage);
        //test
        if (bullet.GetComponent<PlayerPlaneRocket>() != null)
            bullet.GetComponent<PlayerPlaneRocket>().focusableTargetTag = focusableTargetTag;
        //endtest
        lastShootTime = Time.time;
        bulletCount--;
    }

    Vector3 CorrectPosition(Vector3 input)
    {
        input.y = shootPoint.transform.position.y;
        return input;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        bulletCount = clipSize;
    }

    protected float CalculateAimingAid()
    {
        float distance = Vector3.Distance(target.transform.position, shootPoint.transform.position);
        return distance / bulletSpeed;
    }

    public bool CanShootTarget()
    {
        if (target != null && bulletCount > 0 && Time.time - lastShootTime > shootInterval &&
            Vector3.Angle(barrel.transform.forward, CorrectPosition(target.transform.position) + CalculateAimingAid() * target.velocity - shootPoint.transform.position) < minAngle)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        if (shootPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(shootPoint.transform.position, shootPoint.transform.position + (shootPoint.transform.forward.normalized * maxDistance));
        }
    }
}
