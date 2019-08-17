using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VihecleMovement))]
public class VehicleWithTurretAI : MonoBehaviour
{
    VihecleMovement vMovement;
    Turret turret;
    Rigidbody specialTarget;

    private void Start()
    {
        vMovement = GetComponent<VihecleMovement>();
        turret = GetComponentInChildren<Turret>();
    }

    private void Update()
    {
        if (specialTarget != null && turret.IsInShootRange(specialTarget.transform.position))
        {
            vMovement.stop = true;
            turret.SetSpecialTarget(specialTarget);
        }
        else
            vMovement.stop = false;
    }

    public void SetSpecialTarget(Rigidbody target)
    {
        specialTarget = target;
    }
}
