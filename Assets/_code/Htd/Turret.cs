using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public enum TurretState {Off, NeedAwakening, SearchForTarget, Shoot }

    [SerializeField]
    [Range(0,360)]
    protected float rotateSpeed;
    [SerializeField]
    [Range(0, 2)]
    protected float shootInterval = 0.5f;
    [SerializeField]
    protected bool needAwakening = true;
    [SerializeField]
    [Range(0, 50)]
    protected float awakeRange = 15f;
    [SerializeField]
    [Range(0, 50)]
    protected float shootRange = 10f;
    [SerializeField]
    [Range(0, 50)]
    protected float bulletSpeed;
    [SerializeField]
    [Range(0, 30)]
    protected float bulletLifeTime;
    [SerializeField]
    [Range(0, 30)]
    protected int bulletDamage;
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    protected Transform barrel;
    [SerializeField]
    protected GameObject shootPoint;
    [SerializeField]
    protected MeshRenderer[] turretParts = new MeshRenderer[0];
    [SerializeField]
    protected Material turretOnMaterial;
    [SerializeField]
    protected Material turretOffMaterial;
    [SerializeField]
    string[] focusableTargetTag = new string[0];
    [SerializeField]
    protected bool drawGizmos = true;
    [HideInInspector]
    public bool isAwake;
    protected float lastShootTime;
    protected float aimingAid;
    protected Rigidbody target;
    public TurretState state = TurretState.Off;

    Rigidbody specialTarget;

    protected virtual void Awake()
    {
    }

    protected virtual void Update()
    {
        List<Rigidbody> targets = (state == TurretState.Off) ? null : GetPossibleTargets();

        switch (state)
        {
            case TurretState.Off:

                state = (needAwakening) ? TurretState.NeedAwakening : TurretState.SearchForTarget;

                break;
            case TurretState.NeedAwakening:

                if (!isAwake)
                {
                    foreach (Rigidbody item in targets)
                    {
                        if (Vector3.Distance(item.transform.position, transform.position) <= awakeRange)
                        {
                            isAwake = true;

                            for (int i = 0; i < turretParts.Length; i++)
                                turretParts[i].material = turretOnMaterial;
                            if(GetComponent<BoxCollider>() != null)
                                GetComponent<BoxCollider>().enabled = true;
                            state = TurretState.SearchForTarget;
                        }
                    }
                }
                else
                    state = TurretState.SearchForTarget;

                break;
            case TurretState.SearchForTarget:

                int targetIndex = -1;

                for (int i = 0; i < targets.Count; i++)
                {
                    float currentItemDistance = Vector3.Distance(targets[i].transform.position, shootPoint.transform.position);
                    if (currentItemDistance < shootRange && 
                        (targetIndex == -1 || (targetIndex >= 0 && currentItemDistance < Vector3.Distance(targets[targetIndex].transform.position, shootPoint.transform.position))))
                        targetIndex = i;    
                }

                if (targetIndex >= 0)
                {
                    target = targets[targetIndex];
                    state = TurretState.Shoot;
                }

                break;
            case TurretState.Shoot:

                if (target == null || Vector3.Distance(target.transform.position, shootPoint.transform.position) > shootRange)
                {
                    state = TurretState.SearchForTarget;
                }
                else
                {
                    Vector3 aimongPoint = new Vector3(target.transform.position.x, shootPoint.transform.position.y, target.transform.position.z);
                    aimingAid = CalculateAimingAid(target);
                    aimongPoint += target.velocity * aimingAid;  //need improvment
                    barrel.rotation = Quaternion.RotateTowards(barrel.rotation, Quaternion.LookRotation(aimongPoint - barrel.position), Time.deltaTime * rotateSpeed);

                    if (Time.time - lastShootTime > shootInterval && Quaternion.Angle(barrel.rotation, Quaternion.LookRotation(aimongPoint - barrel.position)) < 10f)
                    {
                        ShootBullet();
                    }
                }
                break;
            default:
                break;
        }
    }

    protected virtual List<Rigidbody> GetPossibleTargets()
    {
        List<Rigidbody> result = new List<Rigidbody>();
        GameObject[] tagResult;

        foreach (string tag in focusableTargetTag)
        {
            tagResult = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject item in tagResult)
            {
                result.Add(item.GetComponent<Rigidbody>());
            }
        }

        return result;
    }

    protected virtual void ShootBullet()
    {
        GameObject Bullet1 = Instantiate(bulletPrefab) as GameObject;
        Bullet1.GetComponent<BulletCode>().Initalize(bulletSpeed, bulletLifeTime, shootPoint.transform.position, barrel.rotation, bulletDamage);
        lastShootTime = Time.time;
    }

    protected float CalculateAimingAid(Rigidbody targetRb)
    {
        float distance = Vector3.Distance(targetRb.transform.position, shootPoint.transform.position);
        return distance / bulletSpeed;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(shootPoint.transform.position, shootRange);

        if (needAwakening)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, awakeRange);
        }
    }

    public bool IsInShootRange(Vector3 pos)
    {
        if (Vector3.Distance(pos, transform.position) < Vector3.Distance(transform.position, shootPoint.transform.position) + shootRange)
            return true;
        else
            return false;
    }

    public void SetSpecialTarget(Rigidbody sTarget)
    {
        if (IsInShootRange(sTarget.transform.position))
        {
            target = specialTarget = sTarget;
            state = TurretState.Shoot;
        }
    }
}
