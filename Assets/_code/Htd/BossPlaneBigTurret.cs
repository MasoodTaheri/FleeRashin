using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneBigTurret : BossPlaneTurret
{
    [SerializeField]
    DestroyableObject detObj;
    [SerializeField]
    GameObject hpBar;
    [SerializeField]
    SpriteRenderer shield;
    [SerializeField]
    Collider[] colliders = new Collider[0];
    [SerializeField]
    string mortalTag;
    [SerializeField]
    int mortalLayer;

    protected override void Start()
    {
        base.Start();
        detObj.enabled = false;
        hpBar.SetActive(false);
        shield.gameObject.SetActive(true);

        foreach (Collider col in colliders)
            col.enabled = false;
    }

    public void MakeItMortal()
    {
        detObj.enabled = true;
        hpBar.SetActive(true);
        shield.gameObject.SetActive(false);
        gameObject.tag = mortalTag;
        gameObject.layer = mortalLayer;

        foreach (Collider col in colliders)
            col.enabled = true;
    }
}
