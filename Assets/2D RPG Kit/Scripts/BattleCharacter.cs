using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour {

    [HideInInspector]
    public Animator anim;

    //Game objects used by this code
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [Header("Initialization")]
    public Sprite defeatedSprite;
    public Sprite aliveSprite;
    public Sprite portrait;
    public Slider HPBar;
    public GameObject poisonUi;
    public GameObject silenceUi;
    public GameObject strUp;
    public GameObject strDwn;
    public GameObject defUp;
    public GameObject defDwn;

    [Header("Character Settings")]
    //For checking if this script is attached to a player game object. Else would be enemy game object
    public bool character;
    //Fill in the avaiilable skills for this character
    public string[] skills;

    public string characterName;
    public int currentHp, maxHP, currentSP, maxSP, agility, strength, defense, weaponStrength, armorStrength;

    [Space(10)]
    [Header("Character/Enemy Type")]
    public Type type;

    [Header("Enemy Difficulty Settings")]
    public int maxHpEasy;
    public int maxHpNormal;
    public int maxHpHard;
    [Space(10)]
    public int strengthEasy;
    public int strengthNormal;
    public int strengthHard;
    [Space(10)]
    public int defenseEasy;
    public int defenseNormal;
    public int defenseHard;

    [HideInInspector]
    public bool defeated;
    
    private bool fadeOut;
    [HideInInspector]
    public float fadeOutSpeed = 1f;
    private bool activeBattlerIndicator;

    [Space(10)]
    [Header("Buff/Debuff and ailments debugging")]
    public int strengtModifier;
    [Space(5)]
    public int defenseModifier;
    [Space(5)]
    public bool poisoned;
    [Space(5)]
    public bool silenced;


    // Use this for initialization
    void Awake ()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!character && GameManager.instance.easy)
        {
            currentHp = maxHpEasy;
            maxHP = maxHpEasy;
            strength = strengthEasy;
            defense = defenseEasy;
        }

        if (!character && GameManager.instance.normal)
        {
            currentHp = maxHpNormal;
            maxHP = maxHpNormal;
            strength = strengthNormal;
            defense = defenseNormal;
        }

        if (!character && GameManager.instance.hard)
        {
            currentHp = maxHpHard;
            maxHP = maxHpHard;
            strength = strengthHard;
            defense = defenseHard;
        }
    }
	
	// Update is called once per frame
	void Update () {
        


        if (fadeOut)
        {
            spriteRenderer.color = new Color(Mathf.MoveTowards(spriteRenderer.color.r, 1f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.g, 0f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.b, 0f, fadeOutSpeed * Time.deltaTime), Mathf.MoveTowards(spriteRenderer.color.a, 0f, fadeOutSpeed * Time.deltaTime));
            if(spriteRenderer.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
	}

    public void EnemyFade()
    {
        fadeOut = true;
    }
}

public enum Type
{
    None, //Don't use for battle characters
    All, //Don't use for battle characters
    Normal,
    Fire,
    Water,
    Ground,
    Flying
}
