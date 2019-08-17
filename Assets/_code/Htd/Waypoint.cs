using UnityEngine;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    public Waypoint[] nextWaypoints = new Waypoint[0];
    public float customMaxSpeed = 3f;
    public GameObject[] preventMovementObjects = new GameObject[0];
    public bool move = true;
    List<Waypoint> availableWaypoints = new List<Waypoint>();

    private void Update()
    {
        if (preventMovementObjects.Length == 0)
            return;

        move = false;

        for (int i = 0; i < preventMovementObjects.Length; i++)
            if (preventMovementObjects[i] != null)
                return;

        move = true;
    }

    public Waypoint GetNextWaypoint()
    {
        if (availableWaypoints.Count == 0)
            foreach (Waypoint item in nextWaypoints)
                availableWaypoints.Add(item);

        if (availableWaypoints.Count == 0)
            return null;
        else
        {
            int wIndex = Random.Range(0, availableWaypoints.Count);
            Waypoint result = availableWaypoints[wIndex];
            availableWaypoints.RemoveAt(wIndex);
            return result;
        }
    }
}
