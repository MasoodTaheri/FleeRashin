using System.Collections.Generic;
using UnityEngine;

public class AntiaircraftTurret : Turret
{
    public bool preventFromShooting;

    protected virtual void Update()
    {
        List<Rigidbody> targets = (state == TurretState.Off) ? null : GetPossibleTargets();

        switch (state)
        {
            case TurretState.Shoot:

                if (target == null || Vector3.Distance(target.transform.position, shootPoint.transform.position) > shootRange)
                {
                    state = TurretState.SearchForTarget;
                }
                else
                {
                    Vector3 aimongPoint = new Vector3(target.transform.position.x, barrel.transform.position.y, target.transform.position.z);
                    aimingAid = CalculateAimingAid(target);
                    aimongPoint += target.velocity * aimingAid;  //need improvment
                    barrel.rotation = Quaternion.RotateTowards(barrel.rotation, Quaternion.LookRotation(aimongPoint - barrel.position), Time.deltaTime * rotateSpeed);

                    if (Time.time - lastShootTime > shootInterval && Quaternion.Angle(barrel.rotation, Quaternion.LookRotation(aimongPoint - barrel.position)) < 10f)
                    {
                        if(!preventFromShooting)
                            ShootBullet();
                    }
                }
                break;
            default:
                base.Update();
                break;
        }
    }

    protected override void ShootBullet()
    {
        GameObject Bullet1 = Instantiate(bulletPrefab) as GameObject;
        AntiaircraftRocket rocket = Bullet1.GetComponent<AntiaircraftRocket>();
        rocket.Initalize(bulletSpeed, bulletLifeTime, shootPoint.transform.position, barrel.rotation, bulletDamage);
        rocket.SetUpToLaunch(target.gameObject);
        lastShootTime = Time.time;
    }
}
