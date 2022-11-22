using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreatePrefabsMenu : Editor
{
    private const string RpgKitFolderName = "2D RPG Kit";
    private static readonly string BasePathToRequiredPrefabs = $"Assets/{RpgKitFolderName}/Prefabs";

    [MenuItem("GameObject / 2D RPG Kit Objects / Player Start", false, 1)]
    private static void CreatePlayerStart()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Player Start.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Battle Area", false, 1)]
    private static void CreateBattleArea()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Battle Area.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Chest", false, 1)]
    private static void CreateChest()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Chest.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Common Events", false, 1)]
    private static void CreateCommonEvents()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Common Events.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Complete Quest", false, 1)]
    private static void CreateCompleteQuest()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Complete Quest.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Quest Object Activator", false, 1)]
    private static void CreateQuestObjectActivator()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Quest Object Activator.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Event Object Activator", false, 1)]
    private static void CreateEventObjectActivator()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Event Object Activator.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Inn Keeper", false, 1)]
    private static void CreateInnKeeper()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Inn Keeper.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Shop Keeper", false, 1)]
    private static void CreateShopKeeper()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Shop Keeper.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / NPC", false, 1)]
    private static void CreateNPC()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/NPC.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Pushable Block", false, 1)]
    private static void CreatePushableBlock()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Pushable Block.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Save Point", false, 1)]
    private static void CreateSavePoint()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Save Point.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Auto Save", false, 1)]
    private static void CreateAutoSave()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Auto Save.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("GameObject / 2D RPG Kit Objects / Teleport", false, 1)]
    private static void CreateTeleport()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Objects/Teleport To.prefab", typeof(GameObject));
        GameObject clone = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        Selection.activeObject = clone;
        clone.transform.position = Vector3.one;
    }

    [MenuItem("2D RPG Kit / Game Manager")]
    private static void OpenGameManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Game Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Main/Game Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Audio Manager")]
    private static void OpenAudioManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Audio Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Main/Audio Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Battle Manager")]
    private static void OpenBattleManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Battle Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Main/Battle Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / Control Manager")]
    private static void OpenControlManager()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("Control Manager(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Main/Control Manager.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }        
    }

    [MenuItem("2D RPG Kit / UI")]
    private static void OpenUI()
    {
        if (EditorApplication.isPlaying)
        {
            GameObject prefab = GameObject.Find("UI(Clone)");
            Selection.activeObject = prefab;
        }
        else
        {
            Object prefab = AssetDatabase.LoadAssetAtPath($"{BasePathToRequiredPrefabs}/Main/UI.prefab", typeof(GameObject));
            Selection.activeObject = prefab;
        }
    }
}
