using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityStandardAssets.CrossPlatformInput;

public class BattleStarter : MonoBehaviour {

    [Header("Enemy settings")]
    [Tooltip("Choose an encounter rate for each difficulty setting. The encounter rate is a random ragne between 1 and your entered value and will be reset after each fight. This number is counted down while moving and starts a battle when it reaches 0.")]
    public float encounterRateEasy;
    [Tooltip("Choose an encounter rate for each difficulty setting. The encounter rate is a random ragne between 1 and your entered value and will be reset after each fight. This number is counted down while moving and starts a battle when it reaches 0.")]
    public float encounterRateNormal;
    [Tooltip("Choose an encounter rate for each difficulty setting. The encounter rate is a random ragne between 1 and your entered value and will be reset after each fight. This number is counted down while moving and starts a battle when it reaches 0.")]
    public float encounterRateHard;
    [Tooltip("Thes size of this list determines how many different teams of enemies can be encountered.")]
    public BattleType[] randomBattles;

    [Header("Battle Settings")]
    [Tooltip("Assign a background image for this battle zone")]
    public Sprite battleBG;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int battleMusicIntro;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int battleMusic;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int victoryMusicIntro;
    [Tooltip("Enter the number corresponding to the sound effect from the Audio Manager")]
    public int victoryMusic;
    [Tooltip("Activates a battle as soon as the player enters the battle zone")]
    public bool activateOnEnter;
    [Tooltip("Activates a battle as soon as the player exits the battle zone")]
    public bool activateOnExit;
    [Tooltip("Deactivates this battle zone after the first battle")]
    public bool singleBattle;
    [Tooltip("Deactivates this battle zone after the first battle")]
    public bool unbeatable;
    [Tooltip("Disable retreat from battle within this battle zone")]
    public bool noRetreat;
    [Tooltip("Winning a battle completes a quest")]
    public bool completeQuest;
    [Tooltip("Enter the quest that should be completed")]
    public string QuestToComplete;

    [Header("Debugging")]
    [Tooltip("The actual encounter rate based on the difficulty")]
    public float encounterRate;

    public BoxCollider2D battleCollider;
    //Debug
    private bool inArea;
    public float countdown;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        countdown = Random.Range(1, encounterRate);
        //countdown = 10;

        if (GameManager.instance.easy)
        {
            encounterRate = encounterRateEasy;
        }

        if (GameManager.instance.normal)
        {
            encounterRate = encounterRateNormal;
        }

        if (GameManager.instance.hard)
        {
            encounterRate = encounterRateHard;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(inArea && PlayerController.instance.canMove)
        {
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 || CrossPlatformInputManager.GetAxisRaw("Horizontal") !=0 || CrossPlatformInputManager.GetAxisRaw("Vertical") != 0)
            {
                countdown -= Time.deltaTime;

                if (countdown < 0f)
                {
                    countdown = Random.Range(3, encounterRate);
                    //countdown = 10;

                    if (!GameManager.instance.noEncounters)
                    {
                        StartCoroutine(StartBattleCo());
                    }
                    
                }
            }

            
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (activateOnExit)
            {
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
        }
    }

    public IEnumerator StartBattleCo()
    {
        BattleManager.instance.battleMusicIntro = battleMusicIntro;
        //Play battle music
        AudioManager.instance.PlayBGM(BattleManager.instance.battleMusicIntro);
        
        ScreenFade.instance.FadeToBlack();
        BattleManager.instance.battleBG = battleBG;
        
        BattleManager.instance.battleMusic = battleMusic;
        BattleManager.instance.victoryMusicIntro = victoryMusicIntro;
        BattleManager.instance.victoryMusic = victoryMusic;
        GameManager.instance.battleActive = true;
        GameMenu.instance.gotItemMessage.SetActive(false);

        int selectedBattle = Random.Range(0, randomBattles.Length);

        BattleManager.instance.rewardItems = randomBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardEquipItems = randomBattles[selectedBattle].rewardEquipItems;
        BattleManager.instance.rewardXP = randomBattles[selectedBattle].rewardXP;
        BattleManager.instance.rewardGold = randomBattles[selectedBattle].rewardGold;

        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.unbeatable = unbeatable;
        BattleManager.instance.BattleStart(randomBattles[selectedBattle].enemies, noRetreat);
        BattleManager.instance.UpdateCharacterStatus();
        BattleManager.instance.UpdateBattle();
        ScreenFade.instance.FadeFromBlack();

        if(singleBattle)
        {
            gameObject.SetActive(false);
        }

        RewardScreen.instance.markQuestComplete = completeQuest;
        RewardScreen.instance.questToMark = QuestToComplete;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(120, 0, 0, .3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(battleCollider.offset.x, battleCollider.offset.y, -.7f), battleCollider.size);
    }
}
