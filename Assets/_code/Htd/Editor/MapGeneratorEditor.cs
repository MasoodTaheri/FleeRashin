
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    MapGenerator mapGenerator;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Creat Ground"))
        {
            mapGenerator.CreatGround();
        }
    }

    void OnEnable()
    {
        mapGenerator = (MapGenerator)target;
    }
}
