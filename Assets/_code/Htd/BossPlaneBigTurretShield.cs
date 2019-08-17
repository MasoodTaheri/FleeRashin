using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneBigTurretShield : MonoBehaviour
{
    float baseScale;
    public float reflectingScale;
    public float halfDuration;
    bool reflectingBullet;
    float scale;

    private void Start()
    {
        baseScale = transform.localScale.x;
        scale = baseScale;
    }
    void Update()
    {
        if (reflectingBullet)
        {
            scale += (reflectingScale - baseScale) * Time.deltaTime / halfDuration;
            if (scale >= reflectingScale)
            {
                scale = reflectingScale;
                reflectingBullet = false;
            }

            transform.localScale = new Vector3(scale, scale, scale);
        }
        else if(transform.localScale.x != baseScale)
        {
            scale -= (reflectingScale - baseScale) * Time.deltaTime / halfDuration;
            if (scale <= baseScale)
                scale = baseScale;
        
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void Reflect()
    {
        reflectingBullet = true;
    }
}
