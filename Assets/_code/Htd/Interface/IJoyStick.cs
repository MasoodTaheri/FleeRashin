using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJoyStick
{
    void SteerRight(float value);
    void SteerLeft(float value);
    Transform GetSteerObject();
}
