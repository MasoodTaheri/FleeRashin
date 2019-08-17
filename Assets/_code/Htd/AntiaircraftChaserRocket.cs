using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaircraftChaserRocket : AntiaircraftRocket
{
    GameObject flare;
    GameObject currentTarget;

    protected override void Update()
    {
        base.Update();

        if (flare != null)
            currentTarget = flare;
        else
            currentTarget = fixedTarget;

        if (currentTarget != null)
        {
            Vector3 relativePos = currentTarget.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);
        }
    }

    public void SetFlare(GameObject _flare)
    {
        flare = _flare;
    }
}
