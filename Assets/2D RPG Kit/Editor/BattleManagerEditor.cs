using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BattleManager))]
public class BattleManagerEditor : Editor
{
    private BattleManager battleManagerTarget;
    private SerializedObject soTarget;

    //initialization
    private SerializedProperty battleScene; 
    private SerializedProperty battlePrompts;
    private SerializedProperty targetCharacterMenu;
    private SerializedProperty battleMenu;
    private SerializedProperty targetEnemyMenu;
    private SerializedProperty skillMenu;
    private SerializedProperty itemMenu;
    private SerializedProperty CharacterSlot;
    private SerializedProperty currentTurnIndicator;
    private SerializedProperty hilightedBattleItem;
    private SerializedProperty portrait;
    private SerializedProperty battleUseButtonText;
    private SerializedProperty battleItemName;
    private SerializedProperty battleItemDescription;
    private SerializedProperty targetCharacterName;
    private SerializedProperty characterName;
    private SerializedProperty characterHP;
    private SerializedProperty HPSlider;
    private SerializedProperty characterSP;
    private SerializedProperty SPSlider;
    private SerializedProperty characterLevel;
    private SerializedProperty targetEnemyButtons;
    private SerializedProperty battleMusicIntro;
    private SerializedProperty battleMusic;
    private SerializedProperty victoryMusicIntro;
    private SerializedProperty victoryMusic;
    private SerializedProperty characterPositions;
    private SerializedProperty enemyPositions;
    private SerializedProperty poison;
    private SerializedProperty silence;
    private SerializedProperty strUp;
    private SerializedProperty strDwn;
    private SerializedProperty defUp;
    private SerializedProperty defDwn;

    //ui
    private SerializedProperty touchBackButton;
    private SerializedProperty targetEnemyMenuButton0;
    private SerializedProperty targetEnemyMenuButton1;
    private SerializedProperty targetEnemyMenuButton2;
    private SerializedProperty targetEnemyMenuButton3;
    private SerializedProperty targetEnemyMenuButton4;
    private SerializedProperty targetEnemyMenuButton5;
    private SerializedProperty objTargetMenuButton0;
    private SerializedProperty objTargetMenuButton1;
    private SerializedProperty objTargetMenuButton2;
    private SerializedProperty objTargetMenuButton3;
    private SerializedProperty objTargetMenuButton4;
    private SerializedProperty objTargetMenuButton5;
    private SerializedProperty attackButton;
    private SerializedProperty skillButton;
    private SerializedProperty itemButton;
    private SerializedProperty retreatButton;
    private SerializedProperty skillButton0;
    private SerializedProperty itemButton0;
    private SerializedProperty useItemButton;
    private SerializedProperty targetCharacterButton1;

    //prefabs
    private SerializedProperty characterPrefabs;
    private SerializedProperty enemyPrefabs;
    private SerializedProperty enemyAttackEffect;
    private SerializedProperty characterTurnIndicator;
    private SerializedProperty theDamageNumber;
    private SerializedProperty playerStats;

    //skills
    private SerializedProperty skillList;
    private SerializedProperty skillCost;
    private SerializedProperty skillDescriptionText;
    private SerializedProperty skillButtons;
    private SerializedProperty skillButtonsB;

    //general
    private SerializedProperty retreatRate;
    private SerializedProperty gameOverScene;

    //debugging
    private SerializedProperty currentTurn;
    private SerializedProperty waitForTurn;
    private SerializedProperty itemButtons;
    private SerializedProperty itemButtonsB;
    private SerializedProperty itemSprite;
    private SerializedProperty activeItem;
    private SerializedProperty rewardXP;
    private SerializedProperty rewardGold;
    private SerializedProperty rewardItems;
    private SerializedProperty rewardEquipItems;
    private SerializedProperty noRetreat;

    private void OnEnable()
    {
        battleManagerTarget = (BattleManager)target;
        soTarget = new SerializedObject(target);

        //initialization
        battleScene = soTarget.FindProperty("battleScene");
        battlePrompts = soTarget.FindProperty("battlePrompts");
        targetCharacterMenu = soTarget.FindProperty("targetCharacterMenu");
        battleMenu = soTarget.FindProperty("battleMenu");
        targetEnemyMenu = soTarget.FindProperty("targetEnemyMenu");
        skillMenu = soTarget.FindProperty("skillMenu");
        itemMenu = soTarget.FindProperty("itemMenu");
        CharacterSlot = soTarget.FindProperty("CharacterSlot");
        currentTurnIndicator = soTarget.FindProperty("currentTurnIndicator");
        hilightedBattleItem = soTarget.FindProperty("hilightedBattleItem");
        portrait = soTarget.FindProperty("portrait");
        battleUseButtonText = soTarget.FindProperty("battleUseButtonText");
        battleItemName = soTarget.FindProperty("battleItemName");
        battleItemDescription = soTarget.FindProperty("battleItemDescription");
        targetCharacterName = soTarget.FindProperty("targetCharacterName");
        characterName = soTarget.FindProperty("characterName");
        characterHP = soTarget.FindProperty("characterHP");
        HPSlider = soTarget.FindProperty("HPSlider");
        characterSP = soTarget.FindProperty("characterSP");
        SPSlider = soTarget.FindProperty("SPSlider");
        characterLevel = soTarget.FindProperty("characterLevel");
        targetEnemyButtons = soTarget.FindProperty("targetEnemyButtons");
        battleMusicIntro = soTarget.FindProperty("battleMusicIntro");
        battleMusic = soTarget.FindProperty("battleMusic");
        victoryMusicIntro = soTarget.FindProperty("victoryMusicIntro");
        victoryMusic = soTarget.FindProperty("victoryMusic");
        characterPositions = soTarget.FindProperty("characterPositions");
        enemyPositions = soTarget.FindProperty("enemyPositions");
        poison = soTarget.FindProperty("poison");
        silence = soTarget.FindProperty("silence");
        strUp = soTarget.FindProperty("strUp");
        strDwn = soTarget.FindProperty("strDwn");
        defUp = soTarget.FindProperty("defUp");
        defDwn = soTarget.FindProperty("defDwn");

        //ui
        touchBackButton = soTarget.FindProperty("touchBackButton");
        targetEnemyMenuButton0 = soTarget.FindProperty("targetEnemyMenuButton0");
        targetEnemyMenuButton1 = soTarget.FindProperty("targetEnemyMenuButton1");
        targetEnemyMenuButton2 = soTarget.FindProperty("targetEnemyMenuButton2");
        targetEnemyMenuButton3 = soTarget.FindProperty("targetEnemyMenuButton3");
        targetEnemyMenuButton4 = soTarget.FindProperty("targetEnemyMenuButton4");
        targetEnemyMenuButton5 = soTarget.FindProperty("targetEnemyMenuButton5");
        objTargetMenuButton0 = soTarget.FindProperty("objTargetMenuButton0");
        objTargetMenuButton1 = soTarget.FindProperty("objTargetMenuButton1");
        objTargetMenuButton2 = soTarget.FindProperty("objTargetMenuButton2");
        objTargetMenuButton3 = soTarget.FindProperty("objTargetMenuButton3");
        objTargetMenuButton4 = soTarget.FindProperty("objTargetMenuButton4");
        objTargetMenuButton5 = soTarget.FindProperty("objTargetMenuButton5");
        attackButton = soTarget.FindProperty("attackButton");
        skillButton = soTarget.FindProperty("skillButton");
        itemButton = soTarget.FindProperty("itemButton");
        retreatButton = soTarget.FindProperty("retreatButton");
        skillButton0 = soTarget.FindProperty("skillButton0");
        itemButton0 = soTarget.FindProperty("itemButton0");
        useItemButton = soTarget.FindProperty("useItemButton");
        targetCharacterButton1 = soTarget.FindProperty("targetCharacterButton1");

        //prefabs
        characterPrefabs = soTarget.FindProperty("characterPrefabs");
        enemyPrefabs = soTarget.FindProperty("enemyPrefabs");
        enemyAttackEffect = soTarget.FindProperty("enemyAttackEffect");
        characterTurnIndicator = soTarget.FindProperty("characterTurnIndicator");
        theDamageNumber = soTarget.FindProperty("theDamageNumber");
        playerStats = soTarget.FindProperty("playerStats");

        //skills
        skillList = soTarget.FindProperty("skillList");
        skillCost = soTarget.FindProperty("skillCost");
        skillDescriptionText = soTarget.FindProperty("skillDescriptionText");
        skillButtons = soTarget.FindProperty("skillButtons");
        skillButtonsB = soTarget.FindProperty("skillButtonsB");

        //general
        retreatRate = soTarget.FindProperty("retreatRate");
        gameOverScene = soTarget.FindProperty("gameOverScene");

        //debugging
        currentTurn = soTarget.FindProperty("currentTurn");
        waitForTurn = soTarget.FindProperty("waitForTurn");
        itemButtons = soTarget.FindProperty("itemButtons");
        itemButtonsB = soTarget.FindProperty("itemButtonsB");
        itemSprite = soTarget.FindProperty("itemSprite");
        activeItem = soTarget.FindProperty("activeItem");
        rewardXP = soTarget.FindProperty("rewardXP");
        rewardGold = soTarget.FindProperty("rewardGold");
        rewardItems = soTarget.FindProperty("rewardItems");
        rewardEquipItems = soTarget.FindProperty("rewardEquipItems");
        noRetreat = soTarget.FindProperty("noRetreat");

    }

    public override void OnInspectorGUI()
    {
        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        battleManagerTarget.toolbar = GUILayout.Toolbar(battleManagerTarget.toolbar, new string[] { "Initialization", "UI", "Prefabs", "Skills", "General", "Debugging" });
        switch (battleManagerTarget.toolbar)
        {
            case 0:
                battleManagerTarget.currentTab = "Initialization";
                break;
            case 1:
                battleManagerTarget.currentTab = "UI";
                break;
            case 2:
                battleManagerTarget.currentTab = "Prefabs";
                break;
            case 3:
                battleManagerTarget.currentTab = "Skills";
                break;
            case 4:
                battleManagerTarget.currentTab = "General";
                break;
            case 5:
                battleManagerTarget.currentTab = "Debugging";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);
        }

        EditorGUI.BeginChangeCheck();

        switch (battleManagerTarget.currentTab)
        {
            case "Initialization":
                EditorGUILayout.PropertyField(battleScene);
                EditorGUILayout.PropertyField(battlePrompts);
                EditorGUILayout.PropertyField(targetCharacterMenu);
                EditorGUILayout.PropertyField(battleMenu);
                EditorGUILayout.PropertyField(targetEnemyMenu);
                EditorGUILayout.PropertyField(skillMenu);
                EditorGUILayout.PropertyField(itemMenu);
                EditorGUILayout.PropertyField(CharacterSlot);
                EditorGUILayout.PropertyField(currentTurnIndicator);
                EditorGUILayout.PropertyField(hilightedBattleItem);
                EditorGUILayout.PropertyField(portrait);
                EditorGUILayout.PropertyField(battleUseButtonText);
                EditorGUILayout.PropertyField(battleItemName);
                EditorGUILayout.PropertyField(battleItemDescription);
                EditorGUILayout.PropertyField(targetCharacterName);
                EditorGUILayout.PropertyField(characterName);
                EditorGUILayout.PropertyField(characterHP);
                EditorGUILayout.PropertyField(HPSlider);
                EditorGUILayout.PropertyField(characterSP);
                EditorGUILayout.PropertyField(SPSlider);
                EditorGUILayout.PropertyField(characterLevel);
                EditorGUILayout.PropertyField(targetEnemyButtons);
                EditorGUILayout.PropertyField(battleMusicIntro);
                EditorGUILayout.PropertyField(battleMusic);
                EditorGUILayout.PropertyField(victoryMusicIntro);
                EditorGUILayout.PropertyField(victoryMusic);
                EditorGUILayout.PropertyField(characterPositions);
                EditorGUILayout.PropertyField(enemyPositions);
                EditorGUILayout.PropertyField(skillDescriptionText);
                EditorGUILayout.PropertyField(poison);
                EditorGUILayout.PropertyField(silence);
                EditorGUILayout.PropertyField(strUp);
                EditorGUILayout.PropertyField(strDwn);
                EditorGUILayout.PropertyField(defUp);
                EditorGUILayout.PropertyField(defDwn);
                break;
            case "UI":
                EditorGUILayout.PropertyField(touchBackButton);
                EditorGUILayout.PropertyField(targetEnemyMenuButton0);
                EditorGUILayout.PropertyField(targetEnemyMenuButton1);
                EditorGUILayout.PropertyField(targetEnemyMenuButton2);
                EditorGUILayout.PropertyField(targetEnemyMenuButton3);
                EditorGUILayout.PropertyField(targetEnemyMenuButton4);
                EditorGUILayout.PropertyField(targetEnemyMenuButton5);
                EditorGUILayout.PropertyField(objTargetMenuButton0);
                EditorGUILayout.PropertyField(objTargetMenuButton1);
                EditorGUILayout.PropertyField(objTargetMenuButton2);
                EditorGUILayout.PropertyField(objTargetMenuButton3);
                EditorGUILayout.PropertyField(objTargetMenuButton4);
                EditorGUILayout.PropertyField(objTargetMenuButton5);
                EditorGUILayout.PropertyField(attackButton);
                EditorGUILayout.PropertyField(skillButton);
                EditorGUILayout.PropertyField(itemButton);
                EditorGUILayout.PropertyField(retreatButton);
                EditorGUILayout.PropertyField(skillButton0);
                EditorGUILayout.PropertyField(itemButton0);
                EditorGUILayout.PropertyField(useItemButton);
                EditorGUILayout.PropertyField(targetCharacterButton1);
                break;
            case "Prefabs":
                EditorGUILayout.PropertyField(characterPrefabs);
                EditorGUILayout.PropertyField(enemyPrefabs);
                EditorGUILayout.PropertyField(enemyAttackEffect);
                EditorGUILayout.PropertyField(characterTurnIndicator);
                EditorGUILayout.PropertyField(theDamageNumber);
                EditorGUILayout.PropertyField(playerStats);
                break;
            case "Skills":
                EditorGUILayout.PropertyField(skillList);
                EditorGUILayout.PropertyField(skillCost);                
                EditorGUILayout.PropertyField(skillButtons);
                EditorGUILayout.PropertyField(skillButtonsB);
                break;
            case "General":
                EditorGUILayout.PropertyField(retreatRate);
                EditorGUILayout.PropertyField(gameOverScene);
                break;
            case "Debugging":
                EditorGUILayout.PropertyField(currentTurn);
                EditorGUILayout.PropertyField(waitForTurn);
                EditorGUILayout.PropertyField(itemButtons);
                EditorGUILayout.PropertyField(itemButtonsB);
                EditorGUILayout.PropertyField(itemSprite);
                EditorGUILayout.PropertyField(activeItem);
                EditorGUILayout.PropertyField(rewardXP);
                EditorGUILayout.PropertyField(rewardGold);
                EditorGUILayout.PropertyField(rewardItems);
                EditorGUILayout.PropertyField(rewardEquipItems);
                EditorGUILayout.PropertyField(noRetreat);
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
        }
    }
}
