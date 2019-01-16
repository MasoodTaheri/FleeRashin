using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class ShootClass
{
    public GameObject bulletPrefab;
    public List<GameObject> bulltes;
    public float speed;
    public float lifeLength;
    //public float decreaseHealth;
    public abstract void Shoot(int SpawnerId);
    public abstract void Update();
    public List<GameObject> Spawner;
    protected void addBulletToList(GameObject bullet)
    {
        if (bulltes == null) bulltes = new List<GameObject>();
        //adding bullet to list
        bool isAddedToList = false;
        for (int i = 0; i < bulltes.Count; i++)
        {
            if (bulltes[i] == null)
            {
                bulltes[i] = bullet;
                isAddedToList = true;
                break;
            }
        }
        if (!isAddedToList)
        {
            isAddedToList = true;
            bulltes.Add(bullet);
        }
    }
}

[Serializable]
public class MachineGun : ShootClass
{
    public MachineGun()
    { if (bulltes == null) bulltes = new List<GameObject>(); }

    public override void Shoot(int spawnerID)
    {
        if (spawnerID != -1)
            ShootFromId(spawnerID);
        else
            for (int i = 0; i < Spawner.Count; i++)
                ShootFromId(i);
    }

    private void ShootFromId(int id)
    {
        GameObject tmp = GameObject.Instantiate(bulletPrefab, Spawner[id].transform.position
    , Spawner[id].transform.rotation) as GameObject;
        //GameObject.Destroy(tmp.gameObject, lifeLength);
        //Debug.Log(speed + "  " + lifeLength);
        tmp.transform.Find("Capsule").GetComponent<BulletAffect>().speed = speed;
        tmp.transform.Find("Capsule").GetComponent<BulletAffect>().lifelength = lifeLength;

        addBulletToList(tmp);

        //tmp.gameObject.transform.Find("Capsule")
        //    .GetComponent<ColliderCallback>().enter += OnCollide;
    }


    public override void Update()
    {
        //Debug.Log(bulltes.Count);
        //for (int i = 0; i < bulltes.Count; i++)
        //{
        //    if (bulltes[i] != null)
        //    {
        //        bulltes[i].transform.position += bulltes[i].transform.forward * speed * Time.deltaTime;
        //    }
        //}
    }
}




