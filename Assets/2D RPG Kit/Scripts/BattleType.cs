using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleType {

    [Tooltip("Enter up to four enemies for this team")]
    public string[] enemies;
    [Tooltip("The amount of XP the player gets for defeating this team")]
    public int rewardXP;
    [Tooltip("The amount of Gold the player gets for defeating this team")]
    public int rewardGold;
    [Tooltip("Items dropped by this enemies")]
    public string[] rewardItems;
    [Tooltip("Equipment dropped by this enemies")]
    public string[] rewardEquipItems;
}
