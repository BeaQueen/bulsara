using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private AudioManager audioManagerTarget;
    private SerializedObject soTarget;

    private SerializedProperty sfx;
    private SerializedProperty bgm;

    private void OnEnable()
    {
        audioManagerTarget = (AudioManager)target;
        soTarget = new SerializedObject(target);

        sfx = soTarget.FindProperty("sfx");
        bgm = soTarget.FindProperty("bgm");
    }

    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        audioManagerTarget.toolbar = GUILayout.Toolbar(audioManagerTarget.toolbar, new string[] { "Sound Effects", "Background Music" });
        switch (audioManagerTarget.toolbar)
        {
            case 0:
                audioManagerTarget.currentTab = "Sound Effects";
                break;
            case 1:
                audioManagerTarget.currentTab = "Background Music";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }

        EditorGUI.BeginChangeCheck();

        switch (audioManagerTarget.currentTab)
        {
            case "Sound Effects":
                EditorGUILayout.PropertyField(sfx);
                break;
            case "Background Music":
                EditorGUILayout.PropertyField(bgm);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
