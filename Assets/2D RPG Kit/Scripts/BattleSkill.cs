using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleSkill {

    public string skillName;
    public bool attackAll;
    public bool heal;
    public bool healStatusEffects;
    public bool strengthModifier;
    public bool defenseModifier;
    public bool canPoison;
    public bool canSilence;
    public int skillPower;
    public int skillCost;
    public string description;

    [Space(10)]
    public Effectiveness[] strongAgainst;
    public Effectiveness[] normalAgainst;
    public Effectiveness[] weakAgainst;
    [Space(10)]
    public AttackEffect effect;
}

public enum Effectiveness
{
    None, //Don't use for battle characters
    All, //Don't use for battle characters
    Normal,
    Fire,
    Water,
    Ground,
    Flying
}
