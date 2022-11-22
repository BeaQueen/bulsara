using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameManager gameManagerTarget;
    private SerializedObject soTarget;

    //Initialization
    private SerializedProperty characterStatus;
    private SerializedProperty characterSlots;

    //items
    private SerializedProperty existingItems;

    //inventory
    private SerializedProperty itemsHeld;
    private SerializedProperty equipItemsHeld;
    private SerializedProperty currentGold;

    //debugging
    private SerializedProperty cutSceneActive;
    private SerializedProperty gameMenuOpen;
    private SerializedProperty dialogActive;
    private SerializedProperty fadingBetweenAreas;
    private SerializedProperty shopActive;
    private SerializedProperty battleActive;
    private SerializedProperty saveMenuActive;
    private SerializedProperty innActive;
    private SerializedProperty itemCharChoiceMenu;
    private SerializedProperty loadPromt;
    private SerializedProperty quitPromt;
    private SerializedProperty itemMenu;
    private SerializedProperty equipMenu;
    private SerializedProperty statsMenu;
    private SerializedProperty skillsMenu;
    private SerializedProperty confirmCanMove;
    private SerializedProperty easy;
    private SerializedProperty normal;
    private SerializedProperty hard;
    private SerializedProperty infiniteHP;
    private SerializedProperty infiniteSP;
    private SerializedProperty infiniteGold;
    private SerializedProperty noEncounters;

    private void OnEnable()
    {
        gameManagerTarget = (GameManager)target;
        soTarget = new SerializedObject(target);
        
        //Initialization
        characterStatus = soTarget.FindProperty("characterStatus");
        characterSlots = soTarget.FindProperty("characterSlots");

        //items
        existingItems = soTarget.FindProperty("existingItems");

        //inventory
        itemsHeld = soTarget.FindProperty("itemsHeld");
        equipItemsHeld = soTarget.FindProperty("equipItemsHeld");
        currentGold = soTarget.FindProperty("currentGold");

        //debugging
        cutSceneActive = soTarget.FindProperty("cutSceneActive");
        gameMenuOpen = soTarget.FindProperty("gameMenuOpen");
        dialogActive = soTarget.FindProperty("dialogActive");
        fadingBetweenAreas = soTarget.FindProperty("fadingBetweenAreas");
        shopActive = soTarget.FindProperty("shopActive");
        battleActive = soTarget.FindProperty("battleActive");
        saveMenuActive = soTarget.FindProperty("saveMenuActive");
        innActive = soTarget.FindProperty("innActive");
        itemCharChoiceMenu = soTarget.FindProperty("itemCharChoiceMenu");
        loadPromt = soTarget.FindProperty("loadPromt");
        quitPromt = soTarget.FindProperty("quitPromt");
        itemMenu = soTarget.FindProperty("itemMenu");
        equipMenu = soTarget.FindProperty("equipMenu");
        statsMenu = soTarget.FindProperty("statsMenu");
        skillsMenu = soTarget.FindProperty("skillsMenu");
        confirmCanMove = soTarget.FindProperty("confirmCanMove");
        easy = soTarget.FindProperty("easy");
        normal = soTarget.FindProperty("normal");
        hard = soTarget.FindProperty("hard");
        infiniteHP = soTarget.FindProperty("infiniteHP");
        infiniteSP = soTarget.FindProperty("infiniteSP");
        infiniteGold = soTarget.FindProperty("infiniteGold");
        noEncounters = soTarget.FindProperty("noEncounters");
    }

    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        gameManagerTarget.toolbar = GUILayout.Toolbar(gameManagerTarget.toolbar, new string[] { "Initialization", "Items", "Inventory", "Debugging" });
        switch (gameManagerTarget.toolbar)
        {
            case 0:
                gameManagerTarget.currentTab = "Initialization";
                break;
            case 1:
                gameManagerTarget.currentTab = "Items";
                break;
            case 2:
                gameManagerTarget.currentTab = "Inventory";
                break;
            case 3:
                gameManagerTarget.currentTab = "Debugging";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }

        EditorGUI.BeginChangeCheck();

        switch (gameManagerTarget.currentTab)
        {
            case "Initialization":
                EditorGUILayout.PropertyField(characterStatus);
                EditorGUILayout.PropertyField(characterSlots);
                break;
            case "Items":
                EditorGUILayout.PropertyField(existingItems);
                break;
            case "Inventory":
                EditorGUILayout.PropertyField(itemsHeld);
                EditorGUILayout.PropertyField(equipItemsHeld);
                EditorGUILayout.PropertyField(currentGold);
                break;
            case "Debugging":
                EditorGUILayout.PropertyField(cutSceneActive);
                EditorGUILayout.PropertyField(gameMenuOpen);
                EditorGUILayout.PropertyField(dialogActive);
                EditorGUILayout.PropertyField(fadingBetweenAreas);
                EditorGUILayout.PropertyField(shopActive);
                EditorGUILayout.PropertyField(battleActive);
                EditorGUILayout.PropertyField(saveMenuActive);
                EditorGUILayout.PropertyField(innActive);
                EditorGUILayout.PropertyField(itemCharChoiceMenu);
                EditorGUILayout.PropertyField(loadPromt);
                EditorGUILayout.PropertyField(quitPromt);
                EditorGUILayout.PropertyField(itemMenu);
                EditorGUILayout.PropertyField(equipMenu);
                EditorGUILayout.PropertyField(statsMenu);
                EditorGUILayout.PropertyField(skillsMenu);
                EditorGUILayout.PropertyField(confirmCanMove);
                EditorGUILayout.PropertyField(easy);
                EditorGUILayout.PropertyField(normal);
                EditorGUILayout.PropertyField(hard);
                EditorGUILayout.PropertyField(infiniteHP);
                EditorGUILayout.PropertyField(infiniteSP);
                EditorGUILayout.PropertyField(infiniteGold);
                EditorGUILayout.PropertyField(noEncounters);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
