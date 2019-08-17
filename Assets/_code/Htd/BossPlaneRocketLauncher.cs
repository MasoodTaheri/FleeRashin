using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlaneRocketLauncher : MonoBehaviour
{
    [SerializeField]
    GameObject rocketPrefab;
    [SerializeField]
    Transform[] RocketHolders = new Transform[0];
    [SerializeField]
    Transform barrel;
    [SerializeField]
    Animator barrelAnimator;
    [SerializeField]
    [Range(0,2f)]
    float launchInterval;
    [SerializeField]
    [Range(0, 20f)]
    float reloadInterval;
    [SerializeField]
    [Range(0, 20f)]
    float readyToLaunchInterval;
    float lastlaunchTime = 0;
    float lastReloadTime = 0;
    GameObject target;
    List<BossPlaneRocket> rockets = new List<BossPlaneRocket>();
    bool firstTime = true;
    private void Start()
    {
        //readyToLaunchInterval -= (RocketHolders.Length - 1) * launchInterval;
        lastlaunchTime = Time.time;
    }

    private void Update()
    {
        if (target == null)
        {
            if (FindObjectOfType<DefaultPlayerPlane>() != null)
                target = FindObjectOfType<DefaultPlayerPlane>().gameObject;
            else
                return;
        }

        //if (rockets.Count > 0)
        //    barrel.eulerAngles = Vector3.Lerp(barrel.eulerAngles, Quaternion.LookRotation(target.transform.position - barrel.position).eulerAngles, Mathf.Min((Time.time - lastReloadTime), 1f));
        barrel.eulerAngles = Quaternion.LookRotation(target.transform.position - barrel.position).eulerAngles;

        if (rockets.Count <= 0 && Time.time - lastlaunchTime > reloadInterval)
        {
            Reload();
        }

        if (rockets.Count > 0 && Time.time - lastReloadTime > readyToLaunchInterval && Time.time - lastlaunchTime > launchInterval)
        {
            LaunchRocket();
        }
    }

    void Reload()
    {
        for (int i = 0; i < RocketHolders.Length; i++)
        {
            GameObject go = Instantiate(rocketPrefab, RocketHolders[i].position, barrel.rotation, RocketHolders[i]);
            rockets.Add(go.GetComponent<BossPlaneRocket>());
            go.GetComponent<BoxCollider>().enabled = false;
        }

        lastReloadTime = Time.time;
        barrelAnimator.SetBool("Reload", true);
    }

    void LaunchRocket()
    {
        //rockets[0].dontSetPos = true;
        rockets[0].GetComponent<BoxCollider>().enabled = true;
        rockets[0].enabled = true;
        rockets[0].SetUpToLaunch();
        rockets[0].transform.SetParent(null, true);
        lastlaunchTime = Time.time;
        rockets.RemoveAt(0);

        if (rockets.Count == 0)
        {
            barrelAnimator.SetBool("Reload", false);
            if (firstTime)
            {
                readyToLaunchInterval -= (RocketHolders.Length - 1) * launchInterval;
                firstTime = false;
            }
        }
    }

}
