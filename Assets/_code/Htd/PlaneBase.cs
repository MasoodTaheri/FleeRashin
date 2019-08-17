using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaneBase : MonoBehaviour
{
    protected DestroyableObject desObj;
    protected PlaneMovement pm;

    public abstract void SteerLeft(float value, bool isHolding = false);

    public abstract void SteerRight(float value, bool isHolding = false);

    public abstract Transform GetLeftSteerObject();

    public abstract Transform GetRightSteerObject();

    public abstract void ShootSecondaryWeapon();
}
