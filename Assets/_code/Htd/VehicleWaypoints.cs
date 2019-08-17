using UnityEngine;
using System.Collections.Generic;

public class VehicleWaypoints : MonoBehaviour
{
    public Waypoint firstWaypoint;
    public Waypoint[] waypoints = new Waypoint[0];
    public bool loop;

    public Waypoint GetWaypoint(int index)
    {
        if (index >= waypoints.Length)
            return null;
        return waypoints[index];
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        usedWaypoints.Clear();
        DrawGizmosLines(firstWaypoint);
    }

    private void DrawGizmosLines(Waypoint waypoint)
    {
        if (usedWaypoints.Contains(waypoint))
            return;

        usedWaypoints.Add(waypoint);

        for (int i = 0; i < waypoint.nextWaypoints.Length; i++)
        {
            Gizmos.DrawLine(waypoint.transform.position, waypoint.nextWaypoints[i].transform.position);
        }

        for (int i = 0; i < waypoint.nextWaypoints.Length; i++)
        {
            DrawGizmosLines(waypoint.nextWaypoints[i]);
        }
    }

    List<Waypoint> usedWaypoints = new List<Waypoint>();
    List<Waypoint> roadWaypoints = new List<Waypoint>();

    public void CreatRoad()
    {
        usedWaypoints.Clear();
        roadWaypoints.Clear();
        CreatRoads(firstWaypoint);
    }
    
    void CreatRoads(Waypoint waypoint)
    {
        if (usedWaypoints.Contains(waypoint) || waypoint.nextWaypoints.Length == 0)
        {
            roadWaypoints.Add(waypoint);
            CreatThisRoad(roadWaypoints);
            roadWaypoints.Clear();
        }
        else
        {
            usedWaypoints.Add(waypoint);
            for (int i = 0; i < waypoint.nextWaypoints.Length; i++)
            {
                roadWaypoints.Add(waypoint);
                CreatRoads(waypoint.nextWaypoints[i]);
            }
        }
        
    }
    
    void CreatThisRoad(List<Waypoint> nodes)
    {
        GameObject road = new GameObject();
        road.transform.SetParent(transform, false);
        LineRenderer lr = road.AddComponent<LineRenderer>();
        
        lr.positionCount = nodes.Count;

        for (int i = 0; i < nodes.Count; i++)
            lr.SetPosition(i, nodes[i].transform.position);

        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;
        lr.numCornerVertices = 6;
        lr.numCapVertices = 6;
        //lr.loop = loop;
    }

}
