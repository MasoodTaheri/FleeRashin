using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelMapReader))]
public class LevelMapReaderEditor : Editor
{
    LevelMapReader levelMapReader;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Place object"))
        {
            levelMapReader.PlaceObject();
        }

        if (GUILayout.Button("Next Cluster"))
        {
            levelMapReader.Test();
        }
    }

    void OnEnable()
    {
        levelMapReader = (LevelMapReader)target;
    }
}
