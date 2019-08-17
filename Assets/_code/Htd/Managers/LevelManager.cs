using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    enum PlaneType { Harrier, Raptor, Eagle}
    const string raptorPrefabAddress = "Raptor_Plane";
    const string harrierPrefabAddress = "Harrier_Plane";
    const string eaglePrefabAddress = "Eagle_Plane";
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }
    [SerializeField]
    PlaneType planeType;
    public Transform startingPoint;
    [SerializeField]
    GameObject[] movementBoundaryArray = new GameObject[0];
    [SerializeField]
    float outOfBoundaryTimeLimit;
    List<GameObject> movementBoundaryList = new List<GameObject>();
    Vector3 medianPoint;
    [SerializeField]
    bool checkBoundaryPoints;
    float outOfboundaryDuration;
    bool isGameOver;

    PlaneBase playerPlane;
    public PlaneBase PlayerPlane { get { return playerPlane; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (planeType == PlaneType.Eagle)
            FindObjectOfType<InGamePanel>().SetRightJoystickActive(true);
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (!IsInBoundary())
        {
            outOfboundaryDuration += Time.deltaTime;
            Debug.Log("You're out of boundary!!!! (" + (outOfBoundaryTimeLimit - outOfboundaryDuration) + ")");
        }
        else if(outOfboundaryDuration > 0)
            outOfboundaryDuration = 0;

        if (outOfboundaryDuration > outOfBoundaryTimeLimit)
            GameOver();
    }

    public void StartGame()
    {
        isGameOver = false;
        InstantiatePlane();
        CheckBoundaryPoints();
    }

    void InstantiatePlane()
    {
        string planePrefabAddress;

        switch (planeType)
        {
            case PlaneType.Harrier:
                planePrefabAddress = harrierPrefabAddress;
                break;
            case PlaneType.Raptor:
                planePrefabAddress = raptorPrefabAddress;
                break;
            case PlaneType.Eagle:
            default:
                planePrefabAddress = eaglePrefabAddress;
                break;
        }

        playerPlane = ((GameObject)Instantiate(Resources.Load(planePrefabAddress), startingPoint.position, startingPoint.rotation)).GetComponent<PlaneBase>();
        Camera.main.GetComponent<CameraMovement>().AddTarget(playerPlane.gameObject);
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);

        UIManager.instance.ChangePanel(BasePanel.PanelType.GameOver);
    }

    private void OnDrawGizmos()
    {
        if (checkBoundaryPoints)
        {
            checkBoundaryPoints = false;
            CheckBoundaryPoints();
        }

        if (movementBoundaryList.Count > 0)
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < movementBoundaryList.Count; i++)
                Gizmos.DrawLine(movementBoundaryList[i].transform.position, movementBoundaryList[(i + 1) % movementBoundaryList.Count].transform.position);
        }

        if (planeType != PlaneType.Eagle)
        return;

        EagleWayPoint[] waypoints = FindObjectsOfType<EagleWayPoint>();

        if (waypoints.Length > 0)
        {
            EagleWayPoint temp;

            for (int i = 0; i < waypoints.Length; i++)
            {
                for (int j = i; j < waypoints.Length; j++)
                {
                    if (waypoints[j].index < waypoints[i].index)
                    {
                        temp = waypoints[i];
                        waypoints[i] = waypoints[j];
                        waypoints[j] = temp;
                    }
                }
            }

            Gizmos.color = Color.blue;
            Vector3 startPoint = startingPoint.position;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawLine(startPoint, waypoints[i].transform.position);
                startPoint = waypoints[i].transform.position;
            }
        }
    }

    void CheckBoundaryPoints()
    {
        if (movementBoundaryArray.Length < 3)
        {
            Debug.LogWarning("Level doesn't have boundary!!");
            return;
        }

        movementBoundaryList.Clear();
        List<GameObject> tempList = new List<GameObject>();
        for (int i = 0; i < movementBoundaryArray.Length; i++)
        {
            tempList.Add(movementBoundaryArray[i]);
            medianPoint += movementBoundaryArray[i].transform.position;
        }

        medianPoint /= movementBoundaryArray.Length;
        movementBoundaryList.Add(tempList[0]);
        tempList.RemoveAt(0);

        while (tempList.Count > 0)
        {
            int vindex = 0;

            for (int i = 0; i < tempList.Count; i++)
            {
                if (Vector3.Distance(movementBoundaryList[movementBoundaryList.Count - 1].transform.position, tempList[i].transform.position) <
                    Vector3.Distance(movementBoundaryList[movementBoundaryList.Count - 1].transform.position, tempList[vindex].transform.position))
                {
                    vindex = i;
                }
            }

            movementBoundaryList.Add(tempList[vindex]);
            tempList.RemoveAt(vindex);
        }

    }

    bool IsInBoundary()
    {
        if (movementBoundaryList.Count > 0)
        {
            bool isInPolygon = false;

            for (int i = 0; i < movementBoundaryList.Count; i++)
            {
                isInPolygon |= PointInTriangle(playerPlane.transform.position, movementBoundaryList[i].transform.position, movementBoundaryList[(i + 1) % movementBoundaryList.Count].transform.position, medianPoint);
                if (isInPolygon)
                    break;
            }

            return isInPolygon;
        }
        else
            return true;
    }
    
    bool PointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float d1, d2, d3;
        bool has_neg, has_pos;

        d1 = sign(pt, v1, v2);
        d2 = sign(pt, v2, v3);
        d3 = sign(pt, v3, v1);

        has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(has_neg && has_pos);
    }

    float sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
    }
}
