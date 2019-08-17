using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    // b -> bottom           t -> top
    public GameObject cloudPrefab;
    public int bLayerCloudCount;
    public Vector2 bLayerScaleRange;
    public float bLayerRegenerateMinDistance;
    public int tLayerCloudCount;
    public Vector2 tLayerScaleRange;
    public float tLayerRegenerateMinDistance;

    List<GameObject> bLayerClouds = new List<GameObject>();
    List<GameObject> tLayerClouds = new List<GameObject>();
    Camera mainCamera;
    float cameraOrthoSize;
    int bLayerLastIndex { get { return bLayerClouds.Count - 1; } }
    int tLayerLastIndex { get { return tLayerClouds.Count - 1; } }
    Vector3 cloudEulerAngles = new Vector3(90f, 0, 0);
    Vector3 cameraLastFramePosition;
    //Rect acceptableCloudArea;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraOrthoSize = mainCamera.orthographicSize;
        //acceptableCloudArea = new Rect(mainCamera.transform.position.x - 2.5f * cameraOrthoSize, mainCamera.transform.position.z - 1.5f * cameraOrthoSize, 5f * cameraOrthoSize, 3f * cameraOrthoSize);
        GenerateClouds(ref bLayerClouds, bLayerCloudCount);
        GenerateClouds(ref tLayerClouds, tLayerCloudCount);

        cameraLastFramePosition = mainCamera.transform.position;
    }

    private void Update()
    {
        CheckClouds(bLayerClouds);
        CheckClouds(tLayerClouds);

        Vector3 additionTLayerCloudMovement = (mainCamera.transform.position - cameraLastFramePosition) * 0.5f;

        foreach (GameObject cloud in tLayerClouds)
            cloud.transform.position -= additionTLayerCloudMovement;

        cameraLastFramePosition = mainCamera.transform.position;
    }

    void CheckClouds(List<GameObject> list)
    {
        foreach (GameObject cloud in list)
        {
            if (Mathf.Abs(cloud.transform.position.x - mainCamera.transform.position.x) > 2.5f * cameraOrthoSize ||
                Mathf.Abs(cloud.transform.position.z - mainCamera.transform.position.z) > 1.5f * cameraOrthoSize)
            {
                RepositionCloud(cloud);
            }
        }
    }

    void RepositionCloud(GameObject cloud)
    {

        Vector3 temp = mainCamera.transform.position - cloud.transform.position;
        temp.y = 0;
        cloud.transform.position += temp * 1.9f + new Vector3(Mathf.Cos(Time.time) * bLayerRegenerateMinDistance * 0.5f, 0, Mathf.Sin(Time.time) * bLayerRegenerateMinDistance * 0.5f);

        float scale = Random.Range(bLayerScaleRange.x, bLayerScaleRange.y);
        cloud.transform.localScale = new Vector3(scale, scale, scale);
    }

    void GenerateClouds(ref List<GameObject> list, int count)
    {
        Vector3 startCloudPos = mainCamera.transform.position + new Vector3(-2.5f * cameraOrthoSize, -10, -1.5f * cameraOrthoSize);
        Vector3 newCloudPos;
        int counter = 10;

        for (int i = 0; i < count; i++)
        {
            counter = 10;
            do
            {
                counter--;
                newCloudPos = startCloudPos + new Vector3(Random.Range(0, 5f * cameraOrthoSize), 0, Random.Range(0, 3f * cameraOrthoSize));
            } while (!IsNearAnotherCloud(newCloudPos, list) && counter > 0);

            list.Add(InstantiateCloud(newCloudPos));
        }
    }

    GameObject InstantiateCloud(Vector3 pos)
    {
        GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform, true);
        go.transform.eulerAngles = cloudEulerAngles;
        float scale = Random.Range(bLayerScaleRange.x, bLayerScaleRange.y);
        go.transform.localScale = new Vector3(scale, scale, scale);
        return go;
    }

    bool IsNearAnotherCloud(Vector3 pos, List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            if (Vector3.Distance(pos, item.transform.position) < bLayerRegenerateMinDistance)
            {
                return true;
            }
        }

        return false;
    }
}
