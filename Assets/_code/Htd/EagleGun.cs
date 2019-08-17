using System.Collections;
using UnityEngine;

public class EagleGun : MonoBehaviour
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
    GameObject barrel;
    [SerializeField]
    [Range(0, 360)] float barrelRotateSpeed;
    [Space(10)]
    [SerializeField]
    BulletCode bulletPrefab;
    [SerializeField]
    [Range(0, 20)] float bulletSpeed;
    [SerializeField]
    [Range(0, 15)] float bulletLifeTime;
    [SerializeField]
    [Range(0, 50)] int bulletDamage;

    float lastShootTime;
    bool isHolding;
    int bulletCount;
    [HideInInspector]
    public bool canShoot = true;


    private void Start()
    {
        lastShootTime = 0;
        bulletCount = clipSize;
    }

    private void Update()
    {
        if (!isEnable)
            return;

        if (isHolding && bulletCount > 0 && Time.time - lastShootTime > shootInterval)
        {
            Shoot();

            if (bulletCount <= 0)
                StartCoroutine(Reload());
        }
    }
    
    void Shoot()
    {
        if (!canShoot)
            return;

        BulletCode bullet = Instantiate<BulletCode>(bulletPrefab);
        bullet.Initalize(bulletSpeed, bulletLifeTime, shootPoint.transform.position, shootPoint.transform.rotation, bulletDamage);
        lastShootTime = Time.time;
        bulletCount--;
    }

    public void RotateBarrel(float value, bool isHolding)
    {
        if (!isEnable)
            return;
        this.isHolding = isHolding;
        float antiClockwise = (value < 0) ? 1 : 0;
        float Clockwise = (value > 0) ? 1 : 0;
        barrel.transform.Rotate(0, 0, barrelRotateSpeed * Time.deltaTime * (Clockwise - antiClockwise));
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        bulletCount = clipSize;
    }

    public Transform GetbarrelTransform()
    {
        return barrel.transform;
    }
}
