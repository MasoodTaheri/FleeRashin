using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropController : PowerupButtons
{
    [SerializeField]
    int count;

    public override void DoAction()
    {
        if (count <= 0)
            return;

        count--;
        playermanager.PlanePlayer.DropBomb();
    }

    public override int getvar()
    {
        return count;
    }
}
