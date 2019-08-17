using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public bool shootRocket;
    public LevelRocketLauncher.RocketType rocketType;
    public LevelRocketLauncher.RocketStartPos launchPos;
    public GameObject[] objects = new GameObject[0];
    bool isTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.tag == LevelManager.Instance.PlayerPlane.tag)
        {
            isTriggered = true;

            foreach (GameObject item in objects)
            {
                if (item != null)
                    item.SetActive(true);
            }

            if (shootRocket)
            {
                FindObjectOfType<LevelRocketLauncher>().LaunchRocket(rocketType, launchPos);
            }
        }
    }
}
