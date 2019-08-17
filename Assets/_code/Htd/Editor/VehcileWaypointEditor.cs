using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VehicleWaypoints))]
public class VehcileWaypointEditor : Editor
{
    VehicleWaypoints vehicleWaypoints;
    public override void OnInspectorGUI()
    { 
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create Road"))
        {
            vehicleWaypoints.CreatRoad();
        }
    }

    void OnEnable()
    {
        vehicleWaypoints = (VehicleWaypoints)target;
    }
}
