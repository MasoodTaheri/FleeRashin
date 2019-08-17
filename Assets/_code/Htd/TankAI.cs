using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAI : MonoBehaviour
{
    VihecleMovement vMovement;
    GunWithAutoTargetFinder mainGun;
    GameObject target;
    Rigidbody rb;

    private void Start()
    {
        vMovement = GetComponent<VihecleMovement>();
        rb = GetComponent<Rigidbody>();
        mainGun = GetComponentInChildren<GunWithAutoTargetFinder>();
        mainGun.OnTargetFoundCallback = OnTargetFound;
    }

    private void Update()
    {
        if (target != null && rb.velocity.magnitude > 0)
        {
            mainGun.enabled = false;
            vMovement.stop = true;
        }
        else if (target != null && rb.velocity.magnitude == 0)
        {
            mainGun.enabled = true;
            vMovement.stop = true;
        }
        else
            vMovement.stop = false;
    }

    void OnTargetFound(GameObject target)
    {
        this.target = target;
    }
}
