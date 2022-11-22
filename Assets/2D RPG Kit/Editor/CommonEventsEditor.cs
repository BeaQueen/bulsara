using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CommonEvents))]
public class CommonEventsEditor : Editor
{
    private CommonEvents commonEventsTarget;
    private SerializedObject soTarget;
    
    //display
    private SerializedProperty activateScreenFade;
    private SerializedProperty fadeTime;
    private SerializedProperty blockGameMenu;
    private SerializedProperty hideTouchButtons;
    private SerializedProperty showTouchButtons;

    //events/quests
    private SerializedProperty markEventCompleteAfterFade;
    private SerializedProperty markEventComplete;
    private SerializedProperty eventToMark;
    private SerializedProperty markQuestCompleteAfterFade;
    private SerializedProperty markQuestComplete;
    private SerializedProperty questToMark;

    //player
    private SerializedProperty lockPlayer;
    private SerializedProperty hidePlayer;
    private SerializedProperty facePlayerDown;
    private SerializedProperty facePlayerLeft;
    private SerializedProperty facePlayerUp;
    private SerializedProperty facePlayerRight;
    private SerializedProperty transposePlayer;
    private SerializedProperty x;
    private SerializedProperty y;
    private SerializedProperty z;
    private SerializedProperty ChangePlayerSize;
    private SerializedProperty newSize;
    private SerializedProperty replacePlayer;
    private SerializedProperty playerAnimator;
    //private SerializedProperty currentPartyMemberSlot;
    private SerializedProperty nextPartyMemberSlot;

    //environment
    private SerializedProperty changeBGM;
    private SerializedProperty BGM;
    private SerializedProperty dayTime;
    private SerializedProperty nightTime;
    private SerializedProperty changeScene;
    private SerializedProperty scene;
    private SerializedProperty transitionTime;
    private SerializedProperty newPosition;

    private void OnEnable()
    {
        commonEventsTarget = (CommonEvents)target;
        soTarget = new SerializedObject(target);
        
        //display
        activateScreenFade = soTarget.FindProperty("activateScreenFade");
        fadeTime = soTarget.FindProperty("fadeTime");
        blockGameMenu = soTarget.FindProperty("blockGameMenu");
        hideTouchButtons = soTarget.FindProperty("hideTouchButtons");
        showTouchButtons = soTarget.FindProperty("showTouchButtons");

        //events/quests
        markEventCompleteAfterFade = soTarget.FindProperty("markEventCompleteAfterFade");
        markEventComplete = soTarget.FindProperty("markEventComplete");
        eventToMark = soTarget.FindProperty("eventToMark");
        markQuestCompleteAfterFade = soTarget.FindProperty("markQuestCompleteAfterFade");
        markQuestComplete = soTarget.FindProperty("markQuestComplete");
        questToMark = soTarget.FindProperty("questToMark");

        //player
        lockPlayer = soTarget.FindProperty("lockPlayer");
        hidePlayer = soTarget.FindProperty("hidePlayer");
        facePlayerDown = soTarget.FindProperty("facePlayerDown");
        facePlayerLeft = soTarget.FindProperty("facePlayerLeft");
        facePlayerUp = soTarget.FindProperty("facePlayerUp");
        facePlayerRight = soTarget.FindProperty("facePlayerRight");
        transposePlayer = soTarget.FindProperty("transposePlayer");
        x = soTarget.FindProperty("x");
        y = soTarget.FindProperty("y");
        z = soTarget.FindProperty("z");
        ChangePlayerSize = soTarget.FindProperty("ChangePlayerSize");
        newSize = soTarget.FindProperty("newSize");
        replacePlayer = soTarget.FindProperty("replacePlayer");
        playerAnimator = soTarget.FindProperty("playerAnimator");
        //currentPartyMemberSlot = soTarget.FindProperty("currentPartyMemberSlot");
        nextPartyMemberSlot = soTarget.FindProperty("nextPartyMemberSlot");

        //environment
        changeBGM = soTarget.FindProperty("changeBGM");
        BGM = soTarget.FindProperty("BGM");
        dayTime = soTarget.FindProperty("dayTime");
        nightTime = soTarget.FindProperty("nightTime");
        changeScene = soTarget.FindProperty("changeScene");
        scene = soTarget.FindProperty("scene");
        transitionTime = soTarget.FindProperty("transitionTime");
        newPosition = soTarget.FindProperty("newPosition");

    }

    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        commonEventsTarget.toolbar = GUILayout.Toolbar(commonEventsTarget.toolbar, new string[] { "Display", "Events/Quests", "Player", "Environment" });
        switch (commonEventsTarget.toolbar)
        {
            case 0:
                commonEventsTarget.currentTab = "Display";
                break;
            case 1:
                commonEventsTarget.currentTab = "Events/Quests";
                break;
            case 2:
                commonEventsTarget.currentTab = "Player";
                break;
            case 3:
                commonEventsTarget.currentTab = "Environment";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }

        EditorGUI.BeginChangeCheck();

        switch (commonEventsTarget.currentTab)
        {
            case "Display":
                EditorGUILayout.PropertyField(activateScreenFade);
                EditorGUILayout.PropertyField(fadeTime);
                EditorGUILayout.PropertyField(blockGameMenu);
                EditorGUILayout.PropertyField(hideTouchButtons);
                EditorGUILayout.PropertyField(showTouchButtons);
                break;
            case "Events/Quests":
                EditorGUILayout.PropertyField(markEventCompleteAfterFade);
                EditorGUILayout.PropertyField(markEventComplete);
                EditorGUILayout.PropertyField(eventToMark);
                EditorGUILayout.PropertyField(markQuestCompleteAfterFade);
                EditorGUILayout.PropertyField(markQuestComplete);
                EditorGUILayout.PropertyField(questToMark);
                break;
            case "Player":
                EditorGUILayout.PropertyField(lockPlayer);
                EditorGUILayout.PropertyField(hidePlayer);
                EditorGUILayout.PropertyField(facePlayerDown);
                EditorGUILayout.PropertyField(facePlayerLeft);
                EditorGUILayout.PropertyField(facePlayerUp);
                EditorGUILayout.PropertyField(facePlayerRight);
                EditorGUILayout.PropertyField(transposePlayer);
                EditorGUILayout.PropertyField(x);
                EditorGUILayout.PropertyField(y);
                EditorGUILayout.PropertyField(z);
                EditorGUILayout.PropertyField(ChangePlayerSize);
                EditorGUILayout.PropertyField(newSize);
                EditorGUILayout.PropertyField(replacePlayer);
                EditorGUILayout.PropertyField(playerAnimator);
                //EditorGUILayout.PropertyField(currentPartyMemberSlot);
                EditorGUILayout.PropertyField(nextPartyMemberSlot);
                break;
            case "Environment":
                EditorGUILayout.PropertyField(changeBGM);
                EditorGUILayout.PropertyField(BGM);
                EditorGUILayout.PropertyField(dayTime);
                EditorGUILayout.PropertyField(nightTime);
                EditorGUILayout.PropertyField(changeScene);
                EditorGUILayout.PropertyField(scene);
                EditorGUILayout.PropertyField(transitionTime);
                EditorGUILayout.PropertyField(newPosition);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
