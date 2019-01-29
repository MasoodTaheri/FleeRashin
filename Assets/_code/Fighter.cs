using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : EnemyPlane
{

    public List<GameObject> DamageParticleStep;
    public int DamageStep = -1;
    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected void OnCollisionEnter(Collision other)
    {
        //Debug.Log("EnemyPlane hit " + other.gameObject.tag);

        if (other.gameObject.tag == "Star")
        {
            GameObject.Destroy(other.gameObject);
        }
        else
        {
            if (other.gameObject.tag == "Playerbody")
            {
                playermanager.PlanePlayer.destroy();
                GameObject.Destroy(this.gameObject);
            }
            else
                DamageStepInc();

        }

    }
    void DamageStepInc()
    {
        if (DamageStep + 1 >= DamageParticleStep.Count)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        if (DamageStep > 0)
            DamageParticleStep[DamageStep].SetActive(false);
        DamageStep++;
        DamageParticleStep[DamageStep].SetActive(true);
    }
}
