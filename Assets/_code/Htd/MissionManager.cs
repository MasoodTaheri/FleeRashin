using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DestinationPoint
{
    public Transform point;
    public float range;
    [SerializeField]
    Vector3 pointPos;
    public Vector3 PointPos { get { return pointPos; } }


    public DestinationPoint(Vector3 _pointPos, float _range)
    {
        pointPos = _pointPos;
        range = _range;
    }

    public DestinationPoint() { }

    public bool IsInRange(Vector3 pos)
    {
        if (Vector3.Distance(pos, pointPos) < range)
            return true;
        return false;
    }
}

[System.Serializable]
public class MissionData
{
    [HideInInspector]
    public MissionManager.MissionType type;
    [HideInInspector]
    public string description;
    [HideInInspector]
    public List<DestinationPoint> points = new List<DestinationPoint>();
    [HideInInspector]
    public List<GameObject> targets = new List<GameObject>();
    [HideInInspector]
    public List<float> scanDuration = new List<float>();
    [HideInInspector]
    public List<float> currentScanDuration = new List<float>();
    [HideInInspector]
    public float missionTimeLimit;
    public UnityEvent onMissionStart;
    public UnityEvent onMissionEnd;
}

public class MissionManager : MonoBehaviour
{
    public enum MissionType { Destruction, Escort, Identification, RideTo, DroppingCargo}
    enum FailType { TimesUp, EscorteeDied, PlayerDied}

    public List<MissionData> missions = new List<MissionData>();
    public MissionData CurrentMission
    {
        get
        {
            if (missions.Count == 0 || currentMissionIndex >= missions.Count)
                return null;
            else
                return missions[currentMissionIndex];
        }
    }

    public static MissionManager instance;
    int currentMissionIndex = 0;
    bool isDone;
    bool isStarted;
    bool isGameOver;
    float missionDuration = 0;

    private void Start()
    {
        instance = this;
    }

    public void StartMissionManager()
    {
        if (isStarted || missions.Count == 0)
        {
            Debug.LogError("We have a problem starting \"Mission Manager\".");
            return;
        }

        isStarted = true;
        isGameOver = false;
        missions[currentMissionIndex].onMissionStart.Invoke();
        // Deal with UI
        UpdateUI();
    }

    private void Update()
    {
        if (!isStarted || missions.Count == 0 || currentMissionIndex >= missions.Count)
            return;

        if (LevelManager.Instance.PlayerPlane == null)
        {
            OnMissionFailed(FailType.PlayerDied);
            return;
        }

        if (missions[currentMissionIndex].type == MissionType.Identification)
        {
            for (int i = 0; i < missions[currentMissionIndex].points.Count; i++)
            {
                if (missions[currentMissionIndex].currentScanDuration[i] < missions[currentMissionIndex].scanDuration[i] && 
                    Vector3.Distance(missions[currentMissionIndex].points[i].point.position, LevelManager.Instance.PlayerPlane.transform.position) < missions[currentMissionIndex].points[i].range)
                    missions[currentMissionIndex].currentScanDuration[i] += Time.deltaTime;
                else
                    missions[currentMissionIndex].currentScanDuration[i] = 0;
            }
        }

        switch (missions[currentMissionIndex].type)
        {
            case MissionType.Destruction:

                isDone = true;
                for (int i = 0; i < missions[currentMissionIndex].targets.Count; i++)
                    isDone &= (missions[currentMissionIndex].targets[i] == null);
                UpdateUI();
                break;
            case MissionType.Escort:

                isDone = true;
                for (int i = 0; i < missions[currentMissionIndex].targets.Count; i++)
                {
                    isDone &= (Vector3.Distance(missions[currentMissionIndex].targets[i].transform.position, missions[currentMissionIndex].points[i].point.position) < missions[currentMissionIndex].points[i].range);
                    if (missions[currentMissionIndex].targets[i] == null)
                    {
                        OnMissionFailed(FailType.EscorteeDied);
                        return;
                    }
                }
                break;
            case MissionType.Identification:

                isDone = true;
                for (int i = 0; i < missions[currentMissionIndex].targets.Count; i++)
                    isDone &= (missions[currentMissionIndex].currentScanDuration[i] >= missions[currentMissionIndex].scanDuration[i]);
                break;
            case MissionType.RideTo:

                isDone = true;
                for (int i = 0; i < missions[currentMissionIndex].points.Count; i++)
                    isDone &= (Vector3.Distance(missions[currentMissionIndex].points[i].point.position, LevelManager.Instance.PlayerPlane.transform.position) < missions[currentMissionIndex].points[i].range);
                break;
            case MissionType.DroppingCargo:
                break;
            default:
                break;
        }

        if (IsTimeEnded())
            OnMissionFailed(FailType.TimesUp);
        else if (isDone)
            OnMissionDone();
    }

    bool IsTimeEnded()
    {
        missionDuration += Time.deltaTime;

        if (missions[currentMissionIndex].missionTimeLimit > 0 && missionDuration > missions[currentMissionIndex].missionTimeLimit)
            return true;
        else
            return false;
    }

    void OnMissionDone()
    {
        missionDuration = 0;
        missions[currentMissionIndex].onMissionEnd.Invoke();
        currentMissionIndex++;

        if (currentMissionIndex < missions.Count)
            missions[currentMissionIndex].onMissionStart.Invoke();
        else
        {
            isGameOver = true;
            // victory
        }

        // Deal with UI
        UpdateUI();
    }

    void OnMissionFailed(FailType failType)
    {
        isGameOver = true;

    }

    void UpdateUI()
    {
        if ((!isStarted || isGameOver) && FindObjectOfType<InGamePanel>() != null)
        {
            FindObjectOfType<InGamePanel>().SetMission("Done :-)");
            return;
        }

        string uiText = missions[currentMissionIndex].type.ToString();

        int maxCount = 0;
        int currentCount = 0;

        switch (missions[currentMissionIndex].type)
        {
            case MissionType.Destruction:
                maxCount = missions[currentMissionIndex].targets.Count;

                if (maxCount > 1)
                {
                    foreach (GameObject item in missions[currentMissionIndex].targets)
                        if (item == null)
                            currentCount++;

                    uiText += " (" + currentCount + "/" + maxCount + ")" ;
                }

                break;
            case MissionType.Escort:
                maxCount = missions[currentMissionIndex].targets.Count;

                if (maxCount > 1)
                {
                    for (int i = 0; i < maxCount; i++)
                        if (Vector3.Distance(missions[currentMissionIndex].targets[i].transform.position, missions[currentMissionIndex].points[i].point.position) < missions[currentMissionIndex].points[i].range)
                            currentCount++;

                    uiText += " (" + currentCount + "/" + maxCount + ")";
                }
                break;
            case MissionType.Identification:
                maxCount = missions[currentMissionIndex].targets.Count;

                if (maxCount > 1)
                {
                    for (int i = 0; i < maxCount; i++)
                        if (missions[currentMissionIndex].currentScanDuration[i] >= missions[currentMissionIndex].scanDuration[i])
                            currentCount++;

                    uiText += " (" + currentCount + "/" + maxCount + ")";
                }
                break;
            case MissionType.RideTo:
            case MissionType.DroppingCargo:
            default:
                break;
        }
        
        if(FindObjectOfType<InGamePanel>() != null)
            FindObjectOfType<InGamePanel>().SetMission(uiText);
    }

    public Dictionary<bool, string> GetMissionsState()
    {
        if (!isStarted)
            return null;

        Dictionary<bool, string> result = new Dictionary<bool, string>();
        for (int i = 0; i < missions.Count; i++)
        {
            result.Add((currentMissionIndex >= i), missions[i].type.ToString());
        }

        return result;
    }
}
