using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneGUITilemap))]
public class SceneGUITilemapInspector : Editor
{
    GameObject[] gos;


    void OnSceneGUI()
    {
        EditorGUI.BeginChangeCheck();

        Handles.BeginGUI();

        GUILayout.BeginArea(new Rect(20, 20, 400, 60));

        var rect = EditorGUILayout.BeginVertical();
        GUI.color = new Color(1.0f, 1.0f, 1.0f, 255);
        GUI.Box(rect, GUIContent.none);

        GUI.color = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Tilemap");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Background"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 1");
            Selection.objects = gos;            
        }

        if (GUILayout.Button("Ground 1"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 2");
            Selection.objects = gos;
        }

        if (GUILayout.Button("Ground 2"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 3");
            Selection.objects = gos;
        }

        if (GUILayout.Button("Solid 1"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 4");
            Selection.objects = gos;
        }

        if (GUILayout.Button("Solid 2"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 5");
            Selection.objects = gos;
        }

        if (GUILayout.Button("Foreground"))
        {
            gos = GameObject.FindGameObjectsWithTag("Layer 6");
            Selection.objects = gos;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();


        GUILayout.EndArea();

        Handles.EndGUI();
        
    }
    
}