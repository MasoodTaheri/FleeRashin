using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlaneMovement : PlaneMovement
{
    [HideInInspector]
    public Vector3 destinationPos;
    [HideInInspector]
    public bool stopRotate;

    protected override void Update()
    {
        base.Update();

        if(!stopRotate)
            Rotate();
    }

    protected void Rotate()
    {
        Rightweight = 0;
        Leftweight = 0;

        if (Vector3.Angle(transform.forward, (destinationPos - transform.position)) > 2)
        {
            if (Vector3.Cross(transform.forward, (transform.position - destinationPos)).y > 0)
            {
                Steer(-1);
            }
            else
            {
                Steer(1);
            }
        }
    }
}
