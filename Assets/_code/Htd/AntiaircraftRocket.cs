using UnityEngine;

public class AntiaircraftRocket : PlayerPlaneRocket
{
    [SerializeField]
    [Range(0,2)]
    float changeHieghtDuration;
    [SerializeField]
    Vector3 startScale;
    protected GameObject fixedTarget;
    Vector3 endScale;

    protected override void Update()
    {
        base.Update();

        if (transform.localScale.x - endScale.x < 0)
        {
            transform.localScale += (endScale - startScale) * Time.deltaTime / changeHieghtDuration;
        }
    }

    public void SetUpToLaunch(GameObject _target)
    {
        endScale = transform.localScale;
        transform.localScale = startScale;
        fixedTarget = _target;
        transform.GetChild(1).gameObject.SetActive(true);
        ps = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    protected override void FindTarget()
    {
        target = fixedTarget;
    }
}
