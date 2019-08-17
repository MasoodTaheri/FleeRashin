using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(MissionManager))]
public class MissionDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MissionManager myScript = target as MissionManager;
        
        for (int i = 0; i < myScript.missions.Count; i++)
        {
            EditorGUILayout.Separator();
            Rect rect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(rect, Color.black);
            EditorGUILayout.LabelField("Mission " + i + ":");   
            //isShowing[i] = EditorGUILayout.BeginToggleGroup("Mission " + i + ":", isShowing[i]);

            myScript.missions[i].type = (MissionManager.MissionType)EditorGUILayout.EnumPopup("\tMission Type :", myScript.missions[i].type);
            
            myScript.missions[i].description = EditorGUILayout.TextField("\tDescription:", myScript.missions[i].description);
            myScript.missions[i].missionTimeLimit = EditorGUILayout.FloatField("\tMission Time Limit:",myScript.missions[i].missionTimeLimit);

            switch (myScript.missions[i].type)
            {
                case MissionManager.MissionType.Destruction:

                    for (int j = 0; j < myScript.missions[i].targets.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();

                        myScript.missions[i].targets[j] = EditorGUILayout.ObjectField("\tTarget " + j + ":", myScript.missions[i].targets[j], typeof(GameObject), true) as GameObject;

                        if (GUILayout.Button("Delete Target"))
                        {
                            Undo.RecordObject(target, "Delete Target");
                            myScript.missions[i].targets.RemoveAt(j--);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (GUILayout.Button("New Target"))
                    {
                        Undo.RecordObject(target, "Add Target");
                        myScript.missions[i].targets.Add(null);
                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();

                    break;
                case MissionManager.MissionType.Escort:

                    for (int j = 0; j < myScript.missions[i].targets.Count; j++)
                    {

                        EditorGUILayout.LabelField("\tEscortee " + j + ":");
                        EditorGUILayout.BeginHorizontal();

                        for (int k = 0; k < 10; k++)
                            EditorGUILayout.Space();

                        myScript.missions[i].targets[j] = EditorGUILayout.ObjectField(myScript.missions[i].targets[j], typeof(GameObject), true) as GameObject;
                        myScript.missions[i].points[j].point = EditorGUILayout.ObjectField(myScript.missions[i].points[j].point, typeof(Transform), true) as Transform;
                        myScript.missions[i].points[j].range = EditorGUILayout.FloatField(myScript.missions[i].points[j].range);

                        if (GUILayout.Button("Delete Escortee"))
                        {
                            Undo.RecordObject(target, "Delete Escortee");
                            myScript.missions[i].points.RemoveAt(j);
                            myScript.missions[i].targets.RemoveAt(j--);
                        }

                        EditorGUILayout.EndHorizontal();

                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (GUILayout.Button("New Escortee"))
                    {
                        Undo.RecordObject(target, "Add Escortee");
                        myScript.missions[i].targets.Add(null);
                        myScript.missions[i].points.Add(new DestinationPoint());

                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    break;
                case MissionManager.MissionType.Identification:

                    for (int j = 0; j < myScript.missions[i].points.Count; j++)
                    {

                        EditorGUILayout.LabelField("\tPoint " + j + ":");
                        EditorGUILayout.BeginHorizontal();

                        for (int k = 0; k < 10; k++)
                            EditorGUILayout.Space();
                        
                        myScript.missions[i].points[j].point = EditorGUILayout.ObjectField(myScript.missions[i].points[j].point, typeof(Transform), true) as Transform;
                        myScript.missions[i].points[j].range = EditorGUILayout.FloatField(myScript.missions[i].points[j].range);
                        myScript.missions[i].scanDuration[j] = EditorGUILayout.FloatField(myScript.missions[i].scanDuration[j]);

                        if (GUILayout.Button("Delete Point"))
                        {
                            Undo.RecordObject(target, "Delete Point");
                            myScript.missions[i].points.RemoveAt(j);
                            myScript.missions[i].currentScanDuration.RemoveAt(j);
                            myScript.missions[i].scanDuration.RemoveAt(j--);
                        }

                        EditorGUILayout.EndHorizontal();

                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (GUILayout.Button("New point"))
                    {
                        Undo.RecordObject(target, "Add Point");
                        myScript.missions[i].points.Add(new DestinationPoint());
                        myScript.missions[i].scanDuration.Add(0);
                        myScript.missions[i].currentScanDuration.Add(0);

                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    break;
                case MissionManager.MissionType.RideTo:

                    for (int j = 0; j < myScript.missions[i].points.Count; j++)
                    {

                        EditorGUILayout.LabelField("\tPoint " + j + ":");
                        EditorGUILayout.BeginHorizontal();
                        
                        for (int k = 0; k < 10; k++)
                            EditorGUILayout.Space();

                        myScript.missions[i].points[j].point = EditorGUILayout.ObjectField(myScript.missions[i].points[j].point, typeof(Transform), true) as Transform;
                        myScript.missions[i].points[j].range = EditorGUILayout.FloatField(myScript.missions[i].points[j].range);

                        if (GUILayout.Button("Delete Point"))
                        {
                            Undo.RecordObject(target, "Delete Point");
                            myScript.missions[i].points.RemoveAt(j);
                        }

                        EditorGUILayout.EndHorizontal();

                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.Space();
                    if (myScript.missions[i].points.Count < 1 && GUILayout.Button("New point"))
                    {
                        Undo.RecordObject(target, "Add Point");
                        myScript.missions[i].points.Add(new DestinationPoint());

                    }
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    break;
                case MissionManager.MissionType.DroppingCargo:
                    break;
                default:
                    break;
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Remove Mission"))
            {
                Undo.RecordObject(target, "Remove Mission");
                myScript.missions.RemoveAt(i--);
                continue;
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            
            //EditorGUILayout.EndToggleGroup();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("missions"), true);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
       
        if (GUILayout.Button("New Mission"))
        {
            Undo.RecordObject(target, "Add Mission");
            myScript.missions.Add(new MissionData());
        }
    }
}
