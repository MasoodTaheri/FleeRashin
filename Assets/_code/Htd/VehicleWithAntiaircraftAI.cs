using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VihecleMovement))]
public class VehicleWithAntiaircraftAI : MonoBehaviour
{
    VihecleMovement vMovement;
    AntiaircraftTurret aacTurret;

    private void Start()
    {
        vMovement = GetComponent<VihecleMovement>();
        aacTurret = GetComponentInChildren<AntiaircraftTurret>();
    }

    private void Update()
    {
        if (aacTurret.state == Turret.TurretState.Shoot && vMovement.GetComponent<Rigidbody>().velocity.magnitude > 0)
        {
            aacTurret.preventFromShooting = true;
            vMovement.stop = true;
        }
        else if(aacTurret.state == Turret.TurretState.Shoot && vMovement.GetComponent<Rigidbody>().velocity.magnitude == 0)
        {
            aacTurret.preventFromShooting = false;
            vMovement.stop = true;
        }
        else
            vMovement.stop = false;
    }
}
