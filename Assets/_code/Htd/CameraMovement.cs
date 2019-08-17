using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    Vector2 orthoSizeRange;
    [SerializeField]
    [Range(0, 0.5f)]
    float marginSpace;
    [SerializeField]
    [Range(0, 1)]
    float cameraDelay;
    [SerializeField]
    float yOffset;

    List<GameObject> targets = new List<GameObject>();
    Vector4 mainArea;
    Vector3 cameraVelocity = Vector3.one;
    float xToyConvertCoefficient;
    float optimalSize;
    
    private void Start()
    {
        Camera.main.transform.eulerAngles = new Vector3(90f, 0, 0);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = orthoSizeRange.x;
        mainArea = new Vector4(Screen.width * marginSpace, Screen.height * marginSpace, Screen.width * (1 - 2 * marginSpace), Screen.height * (1 - 2 * marginSpace));
        xToyConvertCoefficient = (float)Screen.height / (float)Screen.width;
    }

    private void FixedUpdate()
    {
        CheckTargets();

        if (targets.Count == 0)
            return;

        SetCameraPos();
        SetCameraSize();
    }

    void SetCameraPos()
    {   
        //position
        Vector3 newPos = Vector3.zero;
        Vector2 max = new Vector2(targets[0].transform.position.x, targets[0].transform.position.z);
        Vector2 min = new Vector2(targets[0].transform.position.x, targets[0].transform.position.z);

        foreach (GameObject target in targets)
        {
            min = new Vector2(Mathf.Min(min.x, target.transform.position.x), Mathf.Min(min.y, target.transform.position.z));
            max = new Vector2(Mathf.Max(max.x, target.transform.position.x), Mathf.Max(max.y, target.transform.position.z));
        }

        newPos = new Vector3(min.x + max.x, 0, min.y + max.y) / 2f;
        newPos = new Vector3(newPos.x, yOffset, newPos.z);

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref cameraVelocity, cameraDelay);
    }

    void SetCameraSize()
    {
        float maxDiff_x = float.NegativeInfinity;
        float maxDiff_y = float.NegativeInfinity;

        for (int i = 0; i < targets.Count; i++)
        {
            for (int j = 0; j < targets.Count; j++)
            {
                maxDiff_x = Mathf.Max(maxDiff_x, Mathf.Abs(targets[i].transform.position.x - targets[j].transform.position.x));
                maxDiff_y = Mathf.Max(maxDiff_y, Mathf.Abs(targets[i].transform.position.z - targets[j].transform.position.z));
            }
        }

        float greatestDiff = Mathf.Max(maxDiff_y, maxDiff_x * xToyConvertCoefficient);
        optimalSize = (greatestDiff * (1 + 2 * marginSpace)) / 2f;
        optimalSize = Mathf.Max(orthoSizeRange.x, optimalSize);
        optimalSize = Mathf.Min(orthoSizeRange.y, optimalSize);

        Camera.main.orthographicSize += (optimalSize - Camera.main.orthographicSize) * Time.fixedDeltaTime / cameraDelay;
    }

    public void AddTarget(GameObject newTarget)
    {
        targets.Add(newTarget);
    }

    void CheckTargets()
    {
        //test
        //if (targets.Count == 0 && LevelManager.Instance.PlayerPlane != null)
        //{
        //    AddTarget(LevelManager.Instance.PlayerPlane.gameObject);
        //}

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] == null || !targets[i].activeInHierarchy)
                targets.RemoveAt(i--);
        }
    }
}
