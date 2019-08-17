using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class RocketData
{
    public LevelRocketLauncher.RocketType type;
    public GameObject prefab;
    public Color color;
    public RocketData(){}
}

public class LevelRocketLauncher : MonoBehaviour
{
    public enum RocketStartPos { Top, TopLeft, Left, BottomLeft, Bottom, BottomRight, Right, TopRight, Random}
    public enum RocketType { FastChaser, SlowChaser, Sniffer, Straight}
    [SerializeField]
    RocketData[] rockets = new RocketData[0];
    [SerializeField]
    float uiIndicatorDuration = 1.5f;
    Camera mainCamera;
    Vector2 orthoSize;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void LaunchRocket(RocketType type, RocketStartPos startPos)
    {
        RocketData rocketData = new RocketData();
        foreach (RocketData item in rockets)
        {
            if (item.type == type)
            {
                rocketData = item;
                break;
            }
        }

        Vector2 dangerUIIndicatorPos = Vector2.zero;
        dangerUIIndicatorPos.x = Mathf.Cos(Mathf.PI * (0.5f + (int)startPos * 0.25f));

        if (startPos == RocketStartPos.Top || startPos == RocketStartPos.TopLeft || startPos == RocketStartPos.TopRight)
            dangerUIIndicatorPos.y = 1;
        else if (startPos == RocketStartPos.Bottom || startPos == RocketStartPos.BottomLeft || startPos == RocketStartPos.BottomRight)
            dangerUIIndicatorPos.y = -1;

        FindObjectOfType<InGamePanel>().ShowRocketIndicator(dangerUIIndicatorPos.x, dangerUIIndicatorPos.y, uiIndicatorDuration, rocketData.color);
        StartCoroutine(LaunchRocketNow(rocketData, startPos));
    }

    IEnumerator LaunchRocketNow(RocketData data, RocketStartPos startPos)
    {
        yield return new WaitForSeconds(uiIndicatorDuration);

        Vector3 launchPos = GetLaunchPos(startPos);
        GameObject go = Instantiate(data.prefab, launchPos, Quaternion.LookRotation(mainCamera.transform.position - launchPos)) as GameObject;
        string[] tags = { LevelManager.Instance.PlayerPlane.tag };
        go.GetComponent<RocketBase>().SetRocketData(0, tags, LevelManager.Instance.PlayerPlane.gameObject);
    }

    Vector3 GetLaunchPos(RocketStartPos startPos)
    {
        orthoSize.y = mainCamera.orthographicSize;
        orthoSize.x = ((float)Screen.width / Screen.height) * orthoSize.y;

        if (startPos == RocketStartPos.Random)
            startPos = (RocketStartPos)Random.Range(0, 7);

        Vector3 launchPosition = mainCamera.transform.position;
        launchPosition.x += Mathf.Cos(Mathf.PI * (0.5f + (int)startPos * 0.25f)) * orthoSize.x;
        launchPosition.y = -6f;
        
        if (startPos == RocketStartPos.Top || startPos == RocketStartPos.TopLeft || startPos == RocketStartPos.TopRight)
            launchPosition.z += orthoSize.y;
        else if(startPos == RocketStartPos.Bottom || startPos == RocketStartPos.BottomLeft || startPos == RocketStartPos.BottomRight)
            launchPosition.z -= orthoSize.y;

        //launchPosition.x += Mathf.Cos(Mathf.PI * (0.5f + (int)startPos * 0.25f)) * orthoSize.x;
        //launchPosition.y = -6f;
        //launchPosition.z += Mathf.Sin(Mathf.PI * (0.5f + (int)startPos * 0.25f)) * orthoSize.y;
        return launchPosition;
    }
}
