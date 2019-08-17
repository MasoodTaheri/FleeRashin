using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VihecleMovement))]
public class TruckAI : MonoBehaviour
{
    VihecleMovement vMovement;
    List<Turret> turrets = new List<Turret>();
    Turret preventMovingTurret;

    private void Start()
    {
        vMovement = GetComponent<VihecleMovement>();
        GetAllTurrets();
    }

    private void Update()
    {
        if (preventMovingTurret == null)
        {
            vMovement.stop = false;
            CheckAllTurret();
        }
    }

    void CheckAllTurret()
    {
        Vector3 stoppingPos = FindStoppingPos();

        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i] == null)
            {
                turrets.RemoveAt(i--);
                continue;
            }

            if (turrets[i].IsInShootRange(stoppingPos))
            {
                preventMovingTurret = turrets[i];
                print(preventMovingTurret.gameObject.name);
                vMovement.stop = true;
                break;
            }
        }
    }

    void GetAllTurrets()
    {
        Turret[] tempArray = FindObjectsOfType<Turret>();
        foreach (Turret item in tempArray)
        {
            if (item.GetComponent<Rigidbody>() != null)
            {
                turrets.Add(item);
            }
        }
    }

    Vector3 FindStoppingPos()
    {
        return vMovement.Get2XStoppingPos();
    }
}
