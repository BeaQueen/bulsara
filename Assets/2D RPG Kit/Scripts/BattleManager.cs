using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class BattleManager : MonoBehaviour
{
    [HideInInspector]
    public int toolbar;
    [HideInInspector]
    public string currentTab;

    [HideInInspector]
    public bool usable;
    Navigation customNav = new Navigation();
    [HideInInspector]
    public int buttonValue;

    //Make instance of this script to be able reference from other scripts!
    public static BattleManager instance;

    
    //For checking if a battle is active
    private bool battleActive;

    //Initiates correct battle background image
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    [HideInInspector]
    public Sprite battleBG;

    //For correct calculation of whether heal HP or SP with items
    [HideInInspector]
    public bool affectHP = false;
    [HideInInspector]
    public bool affectSP = false;

    [Header("Initialization")]
    //Game objects used by this code
    public GameObject battleScene;
    public BattleNotification battlePrompts;
    public GameObject targetCharacterMenu;
    public GameObject battleMenu;
    public GameObject targetEnemyMenu;
    public GameObject skillMenu;
    public GameObject itemMenu;
    public GameObject[] CharacterSlot;
    public GameObject[] currentTurnIndicator;
    public ReadHilightedButton[] hilightedBattleItem;
    public Image[] portrait;
    public List<CharacterStatus> playerStats;

    //Text objects used by this code
    public Text battleUseButtonText;
    public Text battleItemName;
    public Text battleItemDescription;
    public Text[] targetCharacterName;
    public Text[] characterName;
    public Text[] characterHP;
    public Slider[] HPSlider;
    public Text[] characterSP;
    public Slider[] SPSlider;
    public Text[] characterLevel;


    //For initiation of the correct number of characters and enemies
    [HideInInspector]
    public List<BattleCharacter> activeBattlers = new List<BattleCharacter>();

    //For initiation of the correct number of enemie buttons in the enemy target menu
    public BattleTargetButton[] targetEnemyButtons;

    //Music
    public int battleMusicIntro;
    public int battleMusic;
    public int victoryMusicIntro;
    public int victoryMusic;

    [Header("Menu Buttons")]
    //This holds the touch back button for mobile input
    public GameObject touchBackButton;

    //These are being used for highlighting the correct target enemy button
    public Button targetEnemyMenuButton0;
    public Button targetEnemyMenuButton1;
    public Button targetEnemyMenuButton2;
    public Button targetEnemyMenuButton3;
    public Button targetEnemyMenuButton4;
    public Button targetEnemyMenuButton5;

    //These are being used for showing the correct number of target enemy buttons depending on how many monsters you are fighting with
    public GameObject objTargetMenuButton0;
    public GameObject objTargetMenuButton1;
    public GameObject objTargetMenuButton2;
    public GameObject objTargetMenuButton3;
    public GameObject objTargetMenuButton4;
    public GameObject objTargetMenuButton5;

    //These are being used for highlighting the correct menu button for non-mobile input
    public Button attackButton;
    public Button skillButton;
    public Button itemButton;
    public Button retreatButton;
    public Button skillButton0;
    public Button itemButton0;
    public Button useItemButton;
    public Button targetCharacterButton1;

    [Header("Battle Positions")]
    //Positions of characters & enemies
    public Transform[] characterPositions;
    public Transform[] enemyPositions;

    [Header("Battle Prefabs")]
    //References to character & enemy prefabs
    public List<BattleCharacter> characterPrefabs;
    public BattleCharacter[] enemyPrefabs;

    [Header("Battle Effects")]
    //References to battle effect prefabs
    public GameObject enemyAttackEffect;
    public GameObject characterTurnIndicator;
    public DamageNumber theDamageNumber;

    [Header("Battle Turns")]
    //For indication of the current turn
    public int currentTurn;
    public bool waitForTurn;

    [Header("Skills")]
    //Initiates a list of all available skills
    public BattleSkill[] skillList;
    public int skillCost;
    public Text skillDescriptionText;

    //For displaying the correct skill of each character
    public SelectSkill[] skillButtons;
    public Button[] skillButtonsB;

    [Header("Items")]
    //For displaying held items
    public ItemButton[] itemButtons;
    public Button[] itemButtonsB;
    public Image itemSprite;

    //For checking the currently selected item
    public Item activeItem;

    [Header("Battle Settings")]
    //Probability to retreat
    public int retreatRate = 35;
    private bool retreating;

    //Name of the game over scene
    public string gameOverScene;

    [Header("Rewards")]
    //Initialisation of rewards for the current/last battle
    public int rewardXP;
    public int rewardGold;
    public string[] rewardItems;
    public string[] rewardEquipItems;

    [Header("General")]
    //For checking if you are able to retreat from the current battle
    public bool noRetreat;
    public bool unbeatable;

    //For the DelayCo() so the function knows which character is selected when using items
    int selectCharForItem;

    //For using skillon player characters
    public int selectedSkillChar;
    public int selectedSkillCost;
    public int selectedSkillPower;
    public string selectedSkill;
    public bool healStatusEffect;

    //Status effects objects
    public GameObject[] poison;
    public GameObject[] silence;
    public GameObject[] strUp;
    public GameObject[] strDwn;
    public GameObject[] defUp;
    public GameObject[] defDwn;

    // Use this for initialization
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {   
        //Check if any of these menus are active to be able to cancle out of them
        if(itemMenu.activeInHierarchy || targetEnemyMenu.activeInHierarchy || skillMenu.activeInHierarchy)
        {
            if(Input.GetButtonDown("RPGCanclePC") || Input.GetButtonDown("RPGCancleJoy") || CrossPlatformInputManager.GetButtonDown("RPGCancleTouch"))
                
                if(targetCharacterMenu.activeInHierarchy)
                {
                    //Highlight the correct button for non-mobile input when closing the menu
                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = itemButton0;
                        GameMenu.instance.SelectFirstButton();
                    }
                    AudioManager.instance.PlaySFX(3);
                    targetCharacterMenu.SetActive(false);

                    if (!skillMenu.activeInHierarchy)
                    {
                        ShowItems();
                    }
                    else
                    {
                        if (ControlManager.instance.mobile == false)
                        {
                            GameMenu.instance.btn = skillButton0;
                            GameMenu.instance.SelectFirstButton();
                        }

                        for (int i = 0; i < skillButtons.Length; i++)
                        {
                            skillButtonsB[i].interactable = true;
                        }

                    }
                    
                    battleMenu.SetActive(true);
                    
                }
                else
                {
                    AudioManager.instance.PlaySFX(3);
                    //Highlight the correct button for non-mobile input when closing the menu
                    if (ControlManager.instance.mobile == false)
                    {
                        GameMenu.instance.btn = attackButton;
                        GameMenu.instance.SelectFirstButton();
                    }                    

                    //Close the menus
                    itemMenu.SetActive(false);

                    attackButton.interactable = true;
                    skillButton.interactable = true;
                    itemButton.interactable = true;
                    retreatButton.interactable = true;

                    //targetEnemyMenu.SetActive(false);
                    activeBattlers[currentTurn].currentSP += skillCost;
                    skillCost = 0;

                    if (!targetEnemyMenu.activeInHierarchy)
                    {
                        skillMenu.SetActive(false);
                        battleMenu.SetActive(true);
                    }

                    if (targetEnemyMenu.activeInHierarchy)
                    {
                        targetEnemyMenu.SetActive(false);

                        if (skillMenu.activeInHierarchy)
                        {
                            for (int i = 0; i < skillButtonsB.Length; i++)
                            {
                                skillButtonsB[i].interactable = true;
                            }

                            if (ControlManager.instance.mobile == false)
                            {
                                GameMenu.instance.btn = skillButton0;
                                GameMenu.instance.SelectFirstButton();
                            }
                        }
                        

                        if (!skillMenu.activeInHierarchy)
                        {
                            battleMenu.SetActive(true);

                            if (ControlManager.instance.mobile == false)
                            {
                                GameMenu.instance.btn = attackButton;
                                GameMenu.instance.SelectFirstButton();
                            }
                        }
                    }

                    
                    //battleMenu.SetActive(true);
                }
        }

        //Check if a battle is active, display the battle menu and show turn indicator
        if (battleActive)
        {
            if (waitForTurn)
            {
                Instantiate(characterTurnIndicator, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
                if (activeBattlers[currentTurn].character && !targetEnemyMenu.activeInHierarchy)
                {
                    //battleMenu.SetActive(true);
                    if (!targetCharacterMenu.activeInHierarchy)
                    {
                        currentTurnIndicator[currentTurn].SetActive(true);

                    }
                    else
                    {
                        currentTurnIndicator[currentTurn].SetActive(false);
                    }

                    
                }
                else //Hide battle menu and start enemy's turn
                {
                    //battleMenu.SetActive(false);

                    if (!targetEnemyMenu.activeInHierarchy)
                    {
                        //Let enemy attack
                        StartCoroutine(EnemyMoveCo());
                    }
                }
            }
        }
    }

    public IEnumerator waitForSound()
    {
        //Wait Until Sound has finished playing
        while (AudioManager.instance.bgm[battleMusicIntro].isPlaying)
        {
            yield return null;
        }

        //Auidio has finished playing, disable GameObject
        AudioManager.instance.PlayBGM(battleMusic);
    }

    public void PlayButtonSound(int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    //Method to start battle, expects a string array of enemies and a bool to enable/disable retreat
    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        battleMenu.SetActive(true);

        //Check if mobile controlls are enabled and hide them during battle
        if (ControlManager.instance.mobile == true)
        {
            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(false);
                GameMenu.instance.touchController.SetActive(false);
                GameMenu.instance.touchConfirmButton.SetActive(false);
                touchBackButton.SetActive(true);
            }
        }

        //Highlight the attack button for non-mobile input
        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = attackButton;
            GameMenu.instance.SelectFirstButton();
        }

        //Put the correct values into the character status within the battle menu
        UpdateCharacterStatus();


        if (!battleActive)
        {
            //Will be true or false depending on the setting within the BattleStarter script
            noRetreat = setCannotFlee;

            battleActive = true;

            GameManager.instance.battleActive = true;

            //Put the battle background sprite into place
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            spriteRenderer.sprite = battleBG;
            battleScene.SetActive(true);

            //Play battle music
            StartCoroutine(waitForSound());

            //playerStats.Clear();

            foreach (CharacterStatus character in GameManager.instance.characterStatus)
            {
                if (character.gameObject.activeInHierarchy && !playerStats.Contains(character))
                {
                    playerStats.Add(character);
                }
            }

            for (int i = 0; i < playerStats.Count; i++)
            {
                for (int j = 0; j < characterPrefabs.Count; j++)
                {
                    if (playerStats[i].characterName == characterPrefabs[j].characterName)
                    {
                        //Instantiate every active character at their i positions
                        BattleCharacter newCaracter = Instantiate(characterPrefabs[j], characterPositions[i].position, characterPositions[i].rotation);
                        newCaracter.transform.parent = characterPositions[i];
                        activeBattlers.Add(newCaracter);

                        //Give each character the correct stats from the GameManager script
                        CharacterStatus character = GameManager.instance.characterStatus[j];
                        activeBattlers[i].currentHp = character.currentHP;
                        activeBattlers[i].maxHP = character.maxHP;
                        activeBattlers[i].currentSP = character.currentSP;
                        activeBattlers[i].maxSP = character.maxSP;
                        activeBattlers[i].agility = character.agility;
                        activeBattlers[i].strength = character.strength;
                        activeBattlers[i].defense = character.defence;
                        activeBattlers[i].weaponStrength = character.offenseStrength;
                        activeBattlers[i].armorStrength = character.defenseStrength;
                        
                        activeBattlers[i].poisoned = character.poisoned;
                        activeBattlers[i].silenced = character.silenced;

                        activeBattlers[i].skills = character.skills;

                    }
                }
            }


            //Go through the list of enemies and put them in position
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].characterName == enemiesToSpawn[i])
                        {
                            //Instantiate every enemy at their i positions
                            BattleCharacter newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                            
                        }
                    }
                }
            }

            /*
             * Wird jetzt von Animator übernommen
             * 
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].characterName == "Rem")
                {
                    activeBattlers[i].anim["Battle_idle"].wrapMode = WrapMode.Loop;
                    activeBattlers[i].anim.Play("Battle_idle");
                }else
                {
                    activeBattlers[i].anim["Battle_idle"].wrapMode = WrapMode.Loop;
                    activeBattlers[i].anim.Play("Battle_idle");
                }

                

            }
            */

            //Randomize turn order
            waitForTurn = true;

            int bestAgility = 0;

            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].agility > bestAgility)
                {
                    bestAgility = activeBattlers[i].agility;
                    currentTurn = i;
                }
            }

            //currentTurn = Random.Range(0, activeBattlers.Count);

            if (!activeBattlers[currentTurn].character)
            {
                attackButton.interactable = false;
                skillButton.interactable = false;
                itemButton.interactable = false;
                retreatButton.interactable = false;
            }else
            {
                attackButton.interactable = true;
                skillButton.interactable = true;
                itemButton.interactable = true;
                retreatButton.interactable = true;
            }

            UpdateCharacterStatus();
        }
    }

    //Method to start a new turn
    public void NextTurn()
    {
        skillMenu.SetActive(false);
        CloseItemCharChoice();
        attackButton.interactable = true;
        skillButton.interactable = true;
        itemButton.interactable = true;
        retreatButton.interactable = true;

        battleMenu.SetActive(true);
        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = attackButton;
            GameMenu.instance.SelectFirstButton();
        }

        currentTurnIndicator[currentTurn].SetActive(false);
        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        waitForTurn = true;

        UpdateBattle();
        UpdateCharacterStatus();

        if (!activeBattlers[currentTurn].character)
        {
            attackButton.interactable = false;
            skillButton.interactable = false;
            itemButton.interactable = false;
            retreatButton.interactable = false;
        }

        if (activeBattlers[currentTurn].poisoned)
        {
            StartCoroutine(dealPoisonDamageCo());
        }

    }

    //Method for updating battle objects
    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHp < 0)
            {
                activeBattlers[i].currentHp = 0;
            }

            if (activeBattlers[i].currentHp == 0)
            {
                //Show dead character
                if (activeBattlers[i].character)
                {
                    //activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].defeatedSprite;
                    activeBattlers[i].anim.SetTrigger("Defeated");

                    activeBattlers[i].defeated = true;
                    activeBattlers[i].poisoned = false;
                    activeBattlers[i].silenced = false;

                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }

            }
            else
            {
                activeBattlers[i].anim.SetTrigger("Battle_idle");
                if (activeBattlers[i].character)
                {
                    allPlayersDead = false;
                    activeBattlers[i].spriteRenderer.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }
        }

        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //Battle won
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //Battle lost

                //Leave battle when an unbeatable battle is lost
                if (unbeatable)
                {
                    StartCoroutine(EndBattleCo());
                }

                //Show game over screen if a beatable battle was lost
                if (!unbeatable)
                {
                    StartCoroutine(GameOverCo());
                }
                
            }
        }
        else
        {
            while (activeBattlers[currentTurn].currentHp == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
        }
    }

    //Coroutine to wait some seconds between enemy attacks
    public IEnumerator EnemyMoveCo()
    {
        waitForTurn = false;
        yield return new WaitForSeconds(1f);

        yield return new WaitForSeconds(1.2f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();

    }

    //Method for enemy attacks
    public void EnemyAttack()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;


        if (!activeBattlers[currentTurn].silenced && activeBattlers[currentTurn].currentHp > 0)
        {
            List<int> players = new List<int>();
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[i].character && activeBattlers[i].currentHp > 0)
                {
                    players.Add(i);
                }
            }
            int selectedTarget = players[Random.Range(0, players.Count)];

            int selectAttack = Random.Range(0, activeBattlers[currentTurn].skills.Length);
            int movePower = 0;
            bool attackAll = false;
            int attackAllEffect = 0;
            string skillName = "";
            BattleSkill skill = null;

            for (int i = 0; i < skillList.Length; i++)
            {
                if (skillList[i].skillName == activeBattlers[currentTurn].skills[selectAttack])
                {
                    if (!attackAll)
                    {
                        Instantiate(skillList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                    }

                    movePower = skillList[i].skillPower;
                    skillName = skillList[i].skillName;
                    attackAllEffect = i;
                    skill = skillList[i];

                    if (skillList[i].attackAll)
                    {
                        attackAll = true;
                    }
                }
            }

            if (attackAll)
            {
                for (int i = 0; i < activeBattlers.Count; i++)
                {
                    if (activeBattlers[selectedTarget].character && activeBattlers[i].character && activeBattlers[i].currentHp > 0)
                    {
                        Instantiate(skillList[attackAllEffect].effect, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation);
                        DealDamage(i, movePower, skill);
                    }

                    if (!activeBattlers[selectedTarget].character && !activeBattlers[i].character && activeBattlers[i].currentHp > 0)
                    {
                        Instantiate(skillList[attackAllEffect].effect, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation);
                        DealDamage(i, movePower, skill);
                    }
                }
            }

            //Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
            activeBattlers[currentTurn].anim.SetTrigger("Attack");

            if (!attackAll)
            {
                DealDamage(selectedTarget, movePower, skill);
            }
        }
        else
        {
            if (activeBattlers[currentTurn].currentHp > 0)
            {
                StartCoroutine(SilencedEnemyCo());
            }
            
        }
        
        
    }

    public IEnumerator SilencedEnemyCo()
    {
        yield return new WaitForSeconds(1);
        battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " is silenced and can not attack!";
        battlePrompts.Activate();
        yield return new WaitForSeconds(1);
    }

    //Method for calculating dealt damage
    public void DealDamage(int target, int movePower, BattleSkill skill)
    {

        if (skill.strengthModifier)
        {
            activeBattlers[target].strengtModifier += skill.skillPower;

            Debug.Log(activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and lowered " + activeBattlers[target].characterName + "'s Strength by " + skill.skillPower);

            battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + skill.skillName;
            battlePrompts.Activate();

            Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(movePower);

        }

        if (skill.defenseModifier)
        {
            activeBattlers[target].defenseModifier += skill.skillPower;

            Debug.Log(activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and lowered " + activeBattlers[target].characterName + "'s defense by " + skill.skillPower);

            battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + skill.skillName;
            battlePrompts.Activate();

            Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(movePower);

        }

        if (!skill.strengthModifier && !skill.defenseModifier)
        {
            float effectiveness = GetEffectiveness(activeBattlers[target], skill);

            float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].weaponStrength + activeBattlers[currentTurn].strengtModifier;
            float defPwr = activeBattlers[target].defense + activeBattlers[target].armorStrength + activeBattlers[target].defenseModifier;

            float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);
            int damageToGive = Mathf.RoundToInt(damageCalc * effectiveness);

            if (skill.canPoison)
            {
                activeBattlers[target].poisoned = true;
                Debug.Log(activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and is dealing " + damageCalc + "(" + damageToGive + ") damage + poison effect to " + activeBattlers[target].characterName);

                battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and inflicted poison";
                battlePrompts.Activate();
            }

            if (skill.canSilence)
            {
                activeBattlers[target].silenced = true;
                Debug.Log(activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and is dealing " + damageCalc + "(" + damageToGive + ") damage + silence effect to " + activeBattlers[target].characterName);

                battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and inflicted silence";
                battlePrompts.Activate();
            }

            if (!skill.canPoison && !skill.canSilence)
            {
                Debug.Log(activeBattlers[currentTurn].characterName + " used " + skill.skillName + " and is dealing " + damageCalc + "(" + damageToGive + ") damage to " + activeBattlers[target].characterName);

                battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + skill.skillName;
                battlePrompts.Activate();
            }
            
            //Choose only one target if attackAll is false
            if (activeBattlers[target].character && !GameManager.instance.infiniteHP)
            {
                activeBattlers[target].currentHp -= damageToGive;
            }

            if (!activeBattlers[target].character)
            {
                activeBattlers[target].currentHp -= damageToGive;
                activeBattlers[target].HPBar.value = activeBattlers[target].currentHp;
            }

            Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);

        }



        //Play Take_Damage animation on character
        if (activeBattlers[target].character)
        {
            activeBattlers[target].anim.SetTrigger("Take_Damage");
        }

        //Play Take_Damage animation on enemy
        if (!activeBattlers[target].character)
        {
            activeBattlers[target].anim.SetTrigger("Take_Damage");
        }

        UpdateCharacterStatus();

        StartCoroutine(WaitCo(target));
    }

    public float GetEffectiveness(BattleCharacter target, BattleSkill skill)
    {
        for (int i = 0; i < skill.strongAgainst.Length; i++)
        {
            if ((int)skill.strongAgainst[i] == 0)//against none
            {
                Debug.Log(skill.skillName + " strong against none");
                break;
            }

            if ((int)skill.strongAgainst[i] == 1)//against all
            {
                Debug.Log(skill.skillName + " strong against all");
                return 1.2f;
            }

            if ((int)skill.strongAgainst[i] == (int)target.type)
            {
                Debug.Log(skill.skillName + " strong against " + target.type);
                return 1.2f;
            }
        }

        for (int i = 0; i < skill.weakAgainst.Length; i++)
        {
            if ((int)skill.weakAgainst[i] == 0)//against none
            {
                Debug.Log(skill.skillName + " weak against none");
                break;
            }

            if ((int)skill.weakAgainst[i] == 1)//against all
            {
                Debug.Log(skill.skillName + " weak against all");
                return 0.8f;
            }

            if ((int)skill.weakAgainst[i] == (int)target.type)
            {
                Debug.Log(skill.skillName + " weak against " + target.type);
                return 0.8f;
            }
        }

        for (int i = 0; i < skill.normalAgainst.Length; i++)
        {
            if ((int)skill.normalAgainst[i] == 0)//against none
            {
                Debug.Log(skill.skillName + " normal against none");
                break;
            }

            if ((int)skill.normalAgainst[i] == 1)//against all
            {
                Debug.Log(skill.skillName + " normal against all");
                return 1;
            }

            if ((int)skill.normalAgainst[i] == (int)target.type)
            {
                Debug.Log(skill.skillName + " normal against " + target.type);
                return 1;
            }
        }

        Debug.Log(skill.skillName + " none against " + target.type);
        return 1;
    }

    public IEnumerator WaitCo(int target)
    {
        yield return new WaitForSeconds(1);
        activeBattlers[target].anim.SetTrigger("Battle_idle");
        activeBattlers[currentTurn].anim.SetTrigger("Battle_idle");
    }

    //Method for updating character status
    public void UpdateCharacterStatus()
    {
        //playerStats = GameManager.instance.characterStatus;

        for (int i = 0; i < characterName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if (activeBattlers[i].character)
                {
                    BattleCharacter playerData = activeBattlers[i];

                    CharacterSlot[i].SetActive(true);
                    characterName[i].gameObject.SetActive(true);
                    characterName[i].text = playerData.characterName;
                    characterHP[i].text = Mathf.Clamp(playerData.currentHp, 0, int.MaxValue) + "/" + playerData.maxHP;
                    HPSlider[i].maxValue = playerData.maxHP;
                    HPSlider[i].value = playerData.currentHp;
                    characterSP[i].text = Mathf.Clamp(playerData.currentSP, 0, int.MaxValue) + "/" + playerData.maxSP;
                    SPSlider[i].maxValue = playerData.maxSP;
                    SPSlider[i].value = playerData.currentSP;
                    portrait[i].sprite = playerData.portrait;
                    characterLevel[i].text = "Lv " + playerStats[i].level;

                    poison[i].SetActive(playerStats[i].poisoned);
                    silence[i].SetActive(playerStats[i].silenced);

                    if (activeBattlers[i].strengtModifier == 0)
                    {
                        strUp[i].SetActive(false);
                        strDwn[i].SetActive(false);
                    }

                    if (activeBattlers[i].strengtModifier < 0)
                    {
                        strUp[i].SetActive(false);
                        strDwn[i].SetActive(true);
                    }

                    if (activeBattlers[i].strengtModifier > 0)
                    {
                        strUp[i].SetActive(true);
                        strDwn[i].SetActive(false);
                    }

                    if (activeBattlers[i].defenseModifier == 0)
                    {
                        defUp[i].SetActive(false);
                        defDwn[i].SetActive(false);
                    }

                    if (activeBattlers[i].defenseModifier < 0)
                    {
                        defUp[i].SetActive(false);
                        defDwn[i].SetActive(true);
                    }

                    if (activeBattlers[i].defenseModifier > 0)
                    {
                        defUp[i].SetActive(true);
                        defDwn[i].SetActive(false);
                    }
                }
                else
                {
                    CharacterSlot[i].SetActive(false);
                }
            }
            else
            {
                CharacterSlot[i].SetActive(false);
            }
        }

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].character)
            {
                activeBattlers[i].HPBar.maxValue = activeBattlers[i].maxHP;
                activeBattlers[i].HPBar.value = activeBattlers[i].currentHp;

                activeBattlers[i].poisonUi.SetActive(activeBattlers[i].poisoned);
                activeBattlers[i].silenceUi.SetActive(activeBattlers[i].silenced);

                if (activeBattlers[i].strengtModifier == 0)
                {
                    activeBattlers[i].strUp.SetActive(false);
                    activeBattlers[i].strDwn.SetActive(false);
                }

                if (activeBattlers[i].strengtModifier < 0)
                {
                    activeBattlers[i].strUp.SetActive(false);
                    activeBattlers[i].strDwn.SetActive(true);
                }

                if (activeBattlers[i].strengtModifier > 0)
                {
                    activeBattlers[i].strUp.SetActive(true);
                    activeBattlers[i].strDwn.SetActive(false);
                }

                if (activeBattlers[i].defenseModifier == 0)
                {
                    activeBattlers[i].defUp.SetActive(false);
                    activeBattlers[i].defDwn.SetActive(false);
                }

                if (activeBattlers[i].defenseModifier < 0)
                {
                    activeBattlers[i].defUp.SetActive(false);
                    activeBattlers[i].defDwn.SetActive(true);
                }

                if (activeBattlers[i].defenseModifier > 0)
                {
                    activeBattlers[i].defUp.SetActive(true);
                    activeBattlers[i].defDwn.SetActive(false);
                }
            }
        }

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].character)
            {
                poison[i].SetActive(activeBattlers[i].poisoned);
                silence[i].SetActive(activeBattlers[i].silenced);
            }
            
        }
        
    }

    //Method for player attack
    public void PlayerAttack(string moveName, int selectedTarget)
    {
        StartCoroutine(DelayAttackCo(moveName, selectedTarget));

    }

    //Adds a slight delay between choosing the target and affecting the target with the item
    public IEnumerator DelayAttackCo(string moveName, int selectedTarget)
    {
        targetEnemyMenu.SetActive(false);
        skillMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);    
        
        int movePower = 0;
        bool attackAll = false;
        int attackAllEffect = 0;
        string skillName = "";
        BattleSkill skill = null;

        for (int i = 0; i < skillList.Length; i++)
        {
            if (skillList[i].skillName == moveName)
            {
                if (!attackAll)
                {
                    Instantiate(skillList[i].effect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                }
                
                movePower = skillList[i].skillPower;
                skillName = skillList[i].skillName;
                attackAllEffect = i;
                skill = skillList[i];

                if (skillList[i].attackAll)
                {
                    attackAll = true;
                }
            }

        }

        if (attackAll)
        {
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                if (activeBattlers[selectedTarget].character && activeBattlers[i].character && activeBattlers[i].currentHp > 0)
                {
                    Instantiate(skillList[attackAllEffect].effect, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation);
                    DealDamage(i, movePower, skill);
                }

                if (!activeBattlers[selectedTarget].character && !activeBattlers[i].character && activeBattlers[i].currentHp > 0)
                {
                    Instantiate(skillList[attackAllEffect].effect, activeBattlers[i].transform.position, activeBattlers[i].transform.rotation);
                    DealDamage(i, movePower, skill);
                }
            }
        }


        //Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        activeBattlers[currentTurn].anim.SetTrigger("Attack");
        DealDamage(selectedTarget, movePower, skill);
        yield return new WaitForSeconds(1);
        activeBattlers[currentTurn].anim.SetTrigger("Battle_idle");
        battleMenu.SetActive(false);


        NextTurn();
    }

    //Method to opening the target enemy menu. Also shows the correct number of enemy buttons
    public void OpenTargetEnemyMenu(string attackName)
    {

        if (!skillMenu.activeInHierarchy)
        {
            skillButton.interactable = false;
            itemButton.interactable = false;
            retreatButton.interactable = false;
        }
        

        for (int i = 0; i < skillButtonsB.Length; i++)
        {
            if (attackName != skillButtons[i].nameText.text)
            {
                skillButtonsB[i].interactable = false;
            }
        }
        

        targetEnemyMenu.SetActive(true);
        battleMenu.SetActive(false);
        List<int> Enemies = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].character)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetEnemyButtons.Length; i++)
        {
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHp > 0)
            {
                targetEnemyButtons[i].gameObject.SetActive(true);

                targetEnemyButtons[i].moveName = attackName;
                targetEnemyButtons[i].activeBattlerTarget = Enemies[i];
                targetEnemyButtons[i].targetName.text = activeBattlers[Enemies[i]].characterName + " " + (1+i);
            }
            else
            {
                targetEnemyButtons[i].gameObject.SetActive(false);
            }
        }

        if (!objTargetMenuButton0.activeInHierarchy)
        {
            if (objTargetMenuButton1.activeInHierarchy || objTargetMenuButton2.activeInHierarchy || objTargetMenuButton3.activeInHierarchy || objTargetMenuButton4.activeInHierarchy || objTargetMenuButton5.activeInHierarchy)
            {
                if(ControlManager.instance.mobile == false)
                {
                    
                    if (objTargetMenuButton3.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton3;
                        GameMenu.instance.SelectFirstButton();
                    }
                    if (objTargetMenuButton2.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton2;
                        GameMenu.instance.SelectFirstButton();
                    }
                    if (objTargetMenuButton1.activeInHierarchy)
                    {
                        GameMenu.instance.btn = targetEnemyMenuButton1;
                        GameMenu.instance.SelectFirstButton();
                    }
                }                
            }                
        }
        else
        {
            if(ControlManager.instance.mobile == false)
            {
                GameMenu.instance.btn = targetEnemyMenuButton0;
                GameMenu.instance.SelectFirstButton();
            }            
        }
    }

    public void OpenSkillMenu()
    {
        if (activeBattlers[currentTurn].silenced)
        {
            battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " is silenced and can not use skills";
            battlePrompts.Activate();
        }
        else
        {
            for (int i = 0; i < skillButtonsB.Length; i++)
            {
                skillButtonsB[i].interactable = true;
            }

            //closeMenu = true;
            battleMenu.SetActive(false);



            skillMenu.SetActive(true);

            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (activeBattlers[currentTurn].skills.Length > i)
                {
                    skillButtons[i].gameObject.SetActive(true);

                    skillButtons[i].skill = activeBattlers[currentTurn].skills[i];
                    skillButtons[i].nameText.text = skillButtons[i].skill;

                    for (int j = 0; j < skillList.Length; j++)
                    {
                        if (skillList[j].skillName == skillButtons[i].skill)
                        {
                            skillButtons[i].skillCost = skillList[j].skillCost;
                            skillButtons[i].costText.text = skillButtons[i].skillCost.ToString();
                            skillButtons[i].description = skillList[j].description;
                        }
                    }

                }
                else
                {
                    skillButtons[i].gameObject.SetActive(false);
                }
            }

            if (ControlManager.instance.mobile == false)
            {
                GameMenu.instance.btn = skillButton0;
                GameMenu.instance.SelectFirstButton();
            }
        }
    }

    //Method to calculate retreat possibility
    public void Retreat()
    {
        //Shows the following message if retreat from battle is disabled
        if (noRetreat)
        {
            battlePrompts.notificationText.text = "Retreat is impossible!";
            battlePrompts.Activate();
        }
        else
        {
            //Calculation of retreat possibility
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < retreatRate)
            {
                retreating = true;
                StartCoroutine(EndBattleCo());
            }
            else
            {
                //Shows the following message if retreat failed
                NextTurn();
                battlePrompts.notificationText.text = "Retreat failed!";
                battlePrompts.Activate();
            }
        }

    }

    void ResetBattleScene()
    {
        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;

        //Deactivate battle scene
        battleActive = false;

        targetEnemyMenu.SetActive(false);
        skillMenu.SetActive(false);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            activeBattlers[i].strengtModifier = 0;
            activeBattlers[i].defenseModifier = 0;
        }
    }

    //Coroutine to end a battle
    public IEnumerator EndBattleCo()
    {
        ResetBattleScene();

        yield return new WaitForSeconds(.5f);
        AudioManager.instance.PlayBGM(victoryMusicIntro);

        //Wait Until Sound has finished playing
        while (AudioManager.instance.bgm[victoryMusicIntro].isPlaying)
        {
            yield return null;
        }

        AudioManager.instance.PlayBGM(victoryMusic);

        //battleMenu.SetActive(false);

        yield return new WaitForSeconds(.5f);

        ScreenFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        if (unbeatable)
        {
            for (int i = 0; i < activeBattlers.Count; i++)
            {
                activeBattlers[i].currentHp = 1;
            }
        }

        //Update current HP and SP in GameManager script
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].character)
            {
                for (int j = 0; j < GameManager.instance.characterStatus.Count; j++)
                {
                    if (activeBattlers[i].characterName == GameManager.instance.characterStatus[j].characterName)
                    {
                        GameManager.instance.characterStatus[j].currentHP = activeBattlers[i].currentHp;
                        GameManager.instance.characterStatus[j].currentSP = activeBattlers[i].currentSP;

                        GameManager.instance.characterStatus[j].poisoned = activeBattlers[i].poisoned;
                        GameManager.instance.characterStatus[j].silenced = activeBattlers[i].silenced;
                    }
                }
            }

            Destroy(activeBattlers[i].gameObject);
        }

        ScreenFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        playerStats.Clear();
        activeBattlers.Clear();
        currentTurn = 0;

        if (retreating)
        {
            GameManager.instance.battleActive = false;
            retreating = false;
            AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);

            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(true);
                GameMenu.instance.touchController.SetActive(true);
                GameMenu.instance.touchConfirmButton.SetActive(true);
            }
        }
        else
        {
            RewardScreen.instance.OpenRewardScreen(rewardXP, rewardGold, rewardItems, rewardEquipItems);
        }

        //AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    //Coroutine to show game over screen
    public IEnumerator GameOverCo()
    {
        //Reset all managers
        for (int i = 0; i < EventManager.instance.completedEvents.Length; i++)
        {
            EventManager.instance.completedEvents[i] = false;
        }

        for (int i = 0; i < ChestManager.instance.openedChests.Length; i++)
        {
            ChestManager.instance.openedChests[i] = false;
        }

        for (int i = 0; i < QuestManager.instance.completedQuests.Length; i++)
        {
            QuestManager.instance.completedQuests[i] = false;
        }


        //ResetBattleScene();

        attackButton.interactable = false;
        skillButton.interactable = false;
        itemButton.interactable = false;
        retreatButton.interactable = false;

        //Deactivate battle scene
        battleActive = false;

        targetEnemyMenu.SetActive(false);
        skillMenu.SetActive(false);

        ScreenFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        //Destroy(activeBattlers[0]);
        battleScene.SetActive(false);

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            Destroy(activeBattlers[i].gameObject);
        }

        activeBattlers.Clear();
        playerStats.Clear();
        currentTurn = 0;
        GameManager.instance.battleActive = false;

        SceneManager.LoadScene(gameOverScene);
    }

    //Method for showing the correct amount of items during battle
    public void ShowItems()
    {
        GameManager.instance.SortItems();
        
        itemMenu.SetActive(true);

        itemButton.interactable = false;
        skillButton.interactable = false;
        attackButton.interactable = false;
        retreatButton.interactable = false;

        //Set button navigation mode to automatic
        customNav.mode = Navigation.Mode.Automatic;

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtonsB[i].interactable = true;
        }

        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].buttonValue = i;
            hilightedBattleItem[i].buttonValue = i;

            //Set navigation mode of non-disabled buttons to automatic in order to avoid navigating into disabled buttons
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtonsB[i].navigation = customNav;

            }

            //Make only those item buttons interactable which actually hold items 
            if (GameManager.instance.itemsHeld[i] == "")
            {
                itemButtonsB[i].interactable = false;
            }

            //Checks if there are any items stored in the GameManager script and sorts them in the battle item menu
            if (GameManager.instance.itemsHeld[i] != "")
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(true);
                //itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemName;
            }
            else
            {
                //itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
        
            GameMenu.instance.btn = itemButton0;
            GameMenu.instance.SelectFirstButton();
            
    }

    //Method for returning the chosen item and show the "use" button during battle
    public void SelectBattleItem(Item newItem)
    {
        activeItem = newItem;
        GameMenu.instance.activeItem = newItem;

        if (activeItem.item)
        {
            battleUseButtonText.text = "Use";
        }

        if (activeItem.offense || activeItem.defense)
        {
            battleUseButtonText.text = "Equip";
        }

        battleItemName.text = activeItem.itemName;
        battleItemDescription.text = activeItem.description;
        itemSprite.sprite = activeItem.itemSprite;
    }

    //Method for showing the item chracter choice menu. Also shows the correct number of character buttons
    public void OpenItemCharChoice()
    {
        
        //In non-mobile disable every item button except for selected item button
        for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
            {
                if (i != buttonValue)
                {
                    itemButtonsB[i].interactable = false;
                }

            }

        if (ControlManager.instance.mobile == false)
        {
            GameMenu.instance.btn = targetCharacterButton1;
            GameMenu.instance.SelectFirstButton();
        }

        targetCharacterMenu.SetActive(true);

        for (int i = 0; i < targetCharacterName.Length; i++)
        {
            targetCharacterName[i].text = GameManager.instance.characterStatus[i].characterName;
            //targetCharacterName[i].transform.parent.gameObject.SetActive(GameManager.instance.characterStatus[i].gameObject.activeInHierarchy);
            if (i < playerStats.Count)
            {
                targetCharacterName[i].transform.parent.gameObject.SetActive(true);
            }
            else
            {
                targetCharacterName[i].transform.parent.gameObject.SetActive(false);
            }
            
        }
    }

    public void CloseItemCharChoice()
    {
        targetCharacterMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        selectCharForItem = selectChar;

        if (!skillMenu.activeInHierarchy)
        {
            //Set selectCharForItem to be the same as the target character for DelayCo()
            
            //StartCoroutine(DelayItemCo());

            activeItem.UseBattleItem(selectChar);
            battleMenu.SetActive(false);
            itemButton.interactable = true;
            skillButton.interactable = true;
            attackButton.interactable = true;
            retreatButton.interactable = true;

            //Check if the item could be used before progressing to next turn
            if (usable)
            {
                StartCoroutine(DelayItemCo());
                activeBattlers[selectChar].spriteRenderer.sprite = activeBattlers[selectChar].aliveSprite;
                usable = false;
            }
        }

        if (skillMenu.activeInHierarchy)
        {
            targetEnemyMenu.SetActive(false);

            if (!healStatusEffect)
            {
                if (activeBattlers[currentTurn].currentSP >= selectedSkillCost && activeBattlers[currentTurn].currentHp > 0)
                {
                    if (activeBattlers[selectChar].currentHp > 0 && activeBattlers[selectChar].currentHp < activeBattlers[selectChar].maxHP)
                    {
                        activeBattlers[selectChar].currentHp += selectedSkillPower;

                        if (activeBattlers[selectChar].currentHp > activeBattlers[selectChar].maxHP)
                        {
                            activeBattlers[selectChar].currentHp = activeBattlers[selectChar].maxHP;
                        }

                        activeBattlers[currentTurn].currentSP -= selectedSkillCost;
                    }

                    StartCoroutine(DelayItemCo());
                }
            }

            if (healStatusEffect)
            {
                if (activeBattlers[currentTurn].currentSP >= selectedSkillCost && activeBattlers[currentTurn].currentHp > 0)
                {
                    if (activeBattlers[selectChar].currentHp > 0 && activeBattlers[selectChar].poisoned)
                    {
                        activeBattlers[selectChar].poisoned = false;
                        activeBattlers[selectChar].silenced = false;

                        activeBattlers[currentTurn].currentSP -= selectedSkillCost;

                        

                        StartCoroutine(DelayItemCo());
                    }
                }
            }
            
        }
        
    }

    public IEnumerator DelayItemCo()
    {
        if (!skillMenu.activeInHierarchy)
        {
            yield return new WaitForSeconds(1);

            if (!activeItem.battleStatusModifier)
            {
                Instantiate(theDamageNumber, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation).SetDamage(activeItem.amountToChange);
            }
            else
            {
                Instantiate(theDamageNumber, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation).SetDamage(activeItem.defenseStrength + activeItem.offenseStrength);
            }
            

            battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + activeItem.name;
            battlePrompts.Activate();
            yield return new WaitForSeconds(1);
            NextTurn();
        }
        
        //Pretend skill is item
        if (skillMenu.activeInHierarchy)
        {
            targetCharacterMenu.SetActive(false);
            skillMenu.SetActive(false);
            if (!healStatusEffect)
            {
                yield return new WaitForSeconds(1);
                Instantiate(theDamageNumber, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation).SetDamage(selectedSkillPower);

                battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + selectedSkill + " on " + activeBattlers[selectCharForItem].characterName;
                battlePrompts.Activate();

                for (int i = 0; i < skillList.Length; i++)
                {
                    if (skillList[i].skillName == selectedSkill)
                    {
                        Instantiate(skillList[i].effect, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation);

                    }
                }

                yield return new WaitForSeconds(1);
                NextTurn();
            }
            
            if (healStatusEffect)
            {
                yield return new WaitForSeconds(1);
                
                battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " used " + selectedSkill + " on " + activeBattlers[selectCharForItem].characterName;
                battlePrompts.Activate();

                for (int i = 0; i < skillList.Length; i++)
                {
                    if (skillList[i].skillName == selectedSkill)
                    {
                        Instantiate(skillList[i].effect, activeBattlers[selectCharForItem].transform.position, activeBattlers[selectCharForItem].transform.rotation);

                    }
                }

                yield return new WaitForSeconds(1);
                NextTurn();
            }

        }

    }

    public IEnumerator dealPoisonDamageCo()
    {
        yield return new WaitForSeconds(1);

        int poisonDamage = (activeBattlers[currentTurn].maxHP / 10);

        if (activeBattlers[currentTurn].character && GameManager.instance.infiniteHP)
        {
            poisonDamage = 0;
        }

        activeBattlers[currentTurn].currentHp -= poisonDamage;
        Instantiate(theDamageNumber, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation).SetDamage(poisonDamage);
        
        activeBattlers[currentTurn].anim.SetTrigger("Take_Damage");

        battlePrompts.notificationText.text = activeBattlers[currentTurn].characterName + " takes poison damage!";
        battlePrompts.Activate();
        UpdateBattle();
        UpdateCharacterStatus();
    }

}
