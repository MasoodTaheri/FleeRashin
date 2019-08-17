using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlane : MonoBehaviour
{
    [SerializeField]
    public BossPlaneRocketLauncher[] rocketLaunchers = new BossPlaneRocketLauncher[0];
    [SerializeField]
    public BossPlaneTurret[] turrets = new BossPlaneTurret[0];
    [SerializeField]
    public BossPlaneLaser laser;
    [SerializeField]
    public BossPlaneBigTurret bigTurret;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    float speed;
    [SerializeField]
    float movementRange;
    [SerializeField]
    Vector3 movementCenter;
    float angle = 0;
    [SerializeField]
    BoxCollider[] colliders = new BoxCollider[0];
    [SerializeField]
    DestroyableObject detObj;
    [SerializeField]
    GameObject healthbar;
    [SerializeField]
    Transform[] explosionPoint = new Transform[0];
    float lastTimeAnExplosionHappend;
    int remainingGuns;
    bool mortalMode;

    private void Start()
    {
        remainingGuns = turrets.Length + rocketLaunchers.Length + ((laser != null) ? 1 : 0) + ((bigTurret != null) ? 1 : 0);
        transform.position = new Vector3(movementCenter.x  + movementRange, transform.position.y, transform.position.z);
        detObj.OnDamageCallBack = OnDmageApplied;
        bigTurret.enabled = false;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        angle += ((180f * speed) / (Mathf.PI * movementRange)) * Mathf.Deg2Rad * Time.deltaTime;
        transform.position = new Vector3(movementRange*Mathf.Cos(angle), transform.position.y, movementRange * Mathf.Sin(angle));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -angle * Mathf.Rad2Deg, transform.eulerAngles.z); 
    }

    public void OnGunDisable()
    {
        remainingGuns--;
        if (remainingGuns == 2)
        {
            laser.MakeItMortal();
            laser.SetLaserActive();
            bigTurret.enabled = true;
            //FindObjectOfType<followplane>().ZoomOut();
        }

        if (remainingGuns > 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
        healthbar.SetActive(true);
        detObj.enabled = true;
        mortalMode = true;

        FindObjectOfType<AutoTargetFinder>().justShoot = true;
    }

    public void OnDestroyBoss()
    {
        for (int i = 0; i < explosionPoint.Length; i++)
        {
            ExplosionEffectController.instance.ShowExplosionEffect(explosionPoint[i].transform.position,null, 10f);
        }

        FindObjectOfType<AutoTargetFinder>().justShoot = false;
    }

    public void OnDmageApplied(float currentHPPercentage)
    {
        if (Time.time - lastTimeAnExplosionHappend < 0.5f)
            return;

        int explosiontCount = 0;
        explosiontCount += (currentHPPercentage < 0.6f) ? 1 : 0;
        explosiontCount += (currentHPPercentage < 0.3f) ? 1 : 0;
        List<Vector3> poses = new List<Vector3>();
        for (int i = 0; i < explosiontCount; i++)
        {
            Vector3 temp = explosionPoint[Random.Range(0, explosionPoint.Length)].position;

            if (poses.Contains(temp))
            {
                i--;
                continue;
            }

            poses.Add(temp);
        }

        foreach (Vector3 pos in poses)
        {
            ExplosionEffectController.instance.ShowExplosionEffect(pos, transform, 10);
        }

        lastTimeAnExplosionHappend = (explosiontCount > 0) ? Time.time : 0;
    }

    public void OnLaserDisable()
    {
        laser.SetLaserActive(false);
        //laser.enabled = false;
        bigTurret.enabled = true;
        bigTurret.MakeItMortal();
    }
}
