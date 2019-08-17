using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class OfflineManager : MonoBehaviour
{
    [SerializeField]
    GameObject activeInH;
    [SerializeField]
    CameraMovement fCamera;
    [SerializeField]
    float cameraAndBossMinDistance;

    //GameObject activeInH;
    public bool addedToCamera;

    public void StartBossScneario()
    {
        //if (activeInH != null)
        //{
        //    activeInH.GetComponent<UIIndicator>().DestroyUIIndecator();
        //    GameObject.Destroy(activeInH);
        //}

        //fCamera = FindObjectOfType<CameraMovement>();
        //activeInH = Instantiate(boss);
    }
    private void Update()
    {
        if (!addedToCamera && activeInH != null && Vector3.Distance(playermanager.PlanePlayer.transform.position, activeInH.transform.position) < cameraAndBossMinDistance)
        {
            BossPlane boss = activeInH.GetComponent<BossPlane>();
            fCamera.AddTarget(boss.laser.gameObject);
            fCamera.AddTarget(boss.bigTurret.gameObject);
            fCamera.AddTarget(boss.turrets[0].gameObject);
            fCamera.AddTarget(boss.turrets[1].gameObject);
            fCamera.AddTarget(boss.rocketLaunchers[0].gameObject);
            fCamera.AddTarget(boss.rocketLaunchers[1].gameObject);
            addedToCamera = true;
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
