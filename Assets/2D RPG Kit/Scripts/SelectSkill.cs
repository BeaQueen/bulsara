using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkill : MonoBehaviour {

    public int usingChar;
    public string skill;
    public int skillPower;
    public string description;
    public int skillCost;
    public bool usableInMenu;
    public bool healStatusEffects;
    public Text nameText;
    public Text costText;
    public Text descriptionText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Press()
    {
        //In battles
        if (GameManager.instance.battleActive)
        {
            if (!usableInMenu)
            {
                if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP >= skillCost)
                {
                    //BattleManager.instance.skillMenu.SetActive(false);
                    BattleManager.instance.OpenTargetEnemyMenu(skill);
                    BattleManager.instance.skillCost = skillCost;

                    if (!GameManager.instance.infiniteSP)
                    {
                        BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentSP -= BattleManager.instance.skillCost;
                    }

                }
                else
                {
                    //let player know there is not enough SP
                    BattleManager.instance.battlePrompts.notificationText.text = "Not Enough SP!";
                    BattleManager.instance.battlePrompts.Activate();
                    BattleManager.instance.skillMenu.SetActive(false);

                    GameMenu.instance.btn = BattleManager.instance.attackButton;
                    GameMenu.instance.SelectFirstButton();
                }
            }

            //Pretend skill is item to use in battle for allies
            if (usableInMenu || healStatusEffects)
            {
                BattleManager.instance.OpenItemCharChoice();
                BattleManager.instance.selectedSkillChar = usingChar;
                BattleManager.instance.selectedSkillCost = skillCost;
                BattleManager.instance.selectedSkillPower = skillPower;
                BattleManager.instance.selectedSkill = skill;
                BattleManager.instance.healStatusEffect = healStatusEffects;
                BattleManager.instance.targetEnemyMenu.SetActive(false);
            }            
        }

        //In game menu
        if (usableInMenu || healStatusEffects)
        {
            for (int i = 0; i < GameMenu.instance.skillCharacterSlots.Length; i++)
            {
                GameMenu.instance.skillCharacterSlots[i].SetActive(false);
                GameMenu.instance.skillCharChoice[i].gameObject.SetActive(false);
            }

            GameMenu.instance.selectedSkillPower = skillPower;
            GameMenu.instance.selectedSkillCost = skillCost;
            GameMenu.instance.selectedSkillCost = skillCost;
            GameMenu.instance.selectedSkillHealStatusEffects = healStatusEffects;
            if (GameManager.instance.gameMenuOpen)
            {
                Debug.Log(usableInMenu);
                for (int i = 0; i < GameMenu.instance.skillButtons.Length; i++)
                {
                    GameMenu.instance.skillButtons[i].GetComponent<Button>().interactable = false;
                }

                for (int i = 0; i < GameMenu.instance.playerStats.Count; i++)
                {
                    GameMenu.instance.UpdateMainStats();
                    GameMenu.instance.skillSelected = true;
                    GameMenu.instance.skillCharacterSlotsMenu.SetActive(true);
                    GameMenu.instance.skillCharacterSlots[i].SetActive(true);
                    GameMenu.instance.skillCharChoice[i].gameObject.SetActive(true);
                }

                GameMenu.instance.btn = GameMenu.instance.skillCharChoice[0];
                GameMenu.instance.SelectFirstButton();
            }
        }
    }

    
}
