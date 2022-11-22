using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using System;

public class DialogManager : MonoBehaviour {

    //Make instance of this script to be able reference from other scripts!
    public static DialogManager instance;

    [Header("Initialization")]
    //Game objects used by this code
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;
    public Image portrait;
    public GameObject dialogChoices;
    public Button choiceButton;

    [Header("Dialog")]
    //Confirm the spoken lines + portraits
    public Sprite[] dialogPortraits;
    string portraitText;
    public string[] dialogLines;
    [UnityEngine.Serialization.FormerlySerializedAs("sayGoodBye")]
    public string[] finalMessage;
    public int currentLine;
    public bool justStarted;
    public bool fullInventory;
    public bool itemRecieved;
    public bool itemGiven;
    public bool addedPartyMember;
    public GameObject dialogObject;
    public GameObject choiceA;
    public GameObject choiceB;
    public Text choiceALabel;
    public Text choiceBLabel;

    [HideInInspector]
    public bool closeShop = false;
    [HideInInspector]
    public bool dontOpenDialogAgain;

    [Header("Dialog Type")]
    //Dialog Type    
    public bool addNewPartyMember;
    public int partyMemberToAdd;
    public bool isInn;
    public bool isShop;
    public string[] itemsForSale;
    public int innPrice;
    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;
    private string eventToMark;
    private bool markEventComplete1;
    private bool shouldMarkEvent;
    public GameObject NPC;

    // Use this for initialization
    void Start () {
        instance = this;
        
	}
	
	// Update is called once per frame
	void Update () {
		
        //Check if the dialo box is shown by the DialogActivator script
        if(dialogBox.activeInHierarchy)
        {
            //Progress through the lines by pressing the following buttons
            if(Input.GetButtonUp("RPGConfirmPC") || Input.GetButtonUp("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonUp("RPGConfirmTouch"))
            {
                //Check if dialog just opened without any progression
                if (!justStarted)
                {
                    //Prevents opening the dialog box again after confirmin the last lline with button press (Since progressing through dialog is the same button as activating dialog
                    dontOpenDialogAgain = false;
                    currentLine++;

                    //Check if the current line is within the length of dialog lines and close the dialog box if the last line was reached
                    if (currentLine >= dialogLines.Length)
                    {
                        if (choiceA == null && choiceB == null)
                        {
                            dialogBox.SetActive(false);

                            if (itemRecieved && !fullInventory)
                            {
                                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                                StartCoroutine(gotItemMessageCo());
                                itemRecieved = false;
                            }

                            if (itemGiven)
                            {
                                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                                StartCoroutine(gotItemMessageCo());
                                itemGiven = false;
                            }

                            if (fullInventory)
                            {
                                Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                                StartCoroutine(Shop.instance.PromptCo());
                                fullInventory = false;
                            }

                            if (addedPartyMember)
                            {
                                Destroy(NPC);
                                addedPartyMember = false;
                            }

                            if (ControlManager.instance.mobile == true)
                            {
                                GameMenu.instance.touchMenuButton.SetActive(true);
                                GameMenu.instance.touchController.SetActive(true);
                                GameMenu.instance.touchConfirmButton.SetActive(true);
                            }

                            GameManager.instance.dialogActive = false;


                            //Adds next caharacter to party
                            if (addNewPartyMember)
                            {
                                GameManager.instance.characterSlots[partyMemberToAdd].SetActive(true);
                            }
                            /*
                            if (addToPartyCharacter2 == true) 
                            {
                                if (!GameManager.instance.character1.activeInHierarchy)
                                {
                                    GameManager.instance.character1.SetActive(true);
                                }
                                addToPartyCharacter2 = false;
                            }
                            //Adds next caharacter to party
                            if (addToPartyCharacter3 == true)
                            {
                                if (!GameManager.instance.character2.activeInHierarchy)
                                {
                                    GameManager.instance.character2.SetActive(true);
                                }
                                addToPartyCharacter3 = false;
                            }*/

                            //Opens inn menu
                            if (isInn)
                            {
                                Inn.instance.OpenInn();
                                dontOpenDialogAgain = true;
                            }
                            //Opens shop menu
                            if (isShop)
                            {

                                Shop.instance.OpenShop();
                                dontOpenDialogAgain = true;
                            }

                            //Marks quest complete
                            if (shouldMarkQuest)
                            {
                                shouldMarkQuest = false;
                                if (markQuestComplete)
                                {
                                    QuestManager.instance.MarkQuestComplete(questToMark);
                                }
                                else
                                {
                                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                                }
                            }

                            //Marks event complete
                            if (shouldMarkEvent)
                            {
                                shouldMarkEvent = false;
                                if (markEventComplete1)
                                {
                                    EventManager.instance.MarkEventComplete(eventToMark);
                                }
                                else
                                {
                                    EventManager.instance.MarkEventIncomplete(eventToMark);
                                }
                            }
                        }
                        else
                        {
                            dialogChoices.SetActive(true);
                            GameMenu.instance.btn = choiceButton;
                            GameMenu.instance.SelectFirstButton();
                        }

                        
                    }
                    else
                    {
                        //Show name 
                        CheckIfName();
                        CheckIfPortrait();

                        dialogText.text = dialogLines[currentLine];
                    }
                } else
                {
                    justStarted = false;
                }

                
            }
        }

	}

    //Method to call the dialog. Needs the lines as string array + bool for 
    //Use this to call a dialog that is activated by a button press
    public void ShowDialog(Sprite[] portraits, string[] newLines, bool isPerson)
    {
        dialogChoices.SetActive(false);

        if (newLines.Length != 0)
        {
            dialogPortraits = portraits;
            dialogLines = newLines;


            currentLine = 0;

            CheckIfName();
            CheckIfPortrait();

            dialogText.text = dialogLines[currentLine];
            dialogBox.SetActive(true);


            justStarted = true;

            nameBox.SetActive(isPerson);

            GameManager.instance.dialogActive = true;
        }
        
        if (newLines.Length == 0)
        {
            if (itemRecieved && !fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemRecieved = false;
            }

            if (itemGiven)
            {
                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemGiven = false;
            }

            if (fullInventory)
            {
                Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                StartCoroutine(Shop.instance.PromptCo());
                fullInventory = false;
            }

            if (addedPartyMember)
            {
                Destroy(NPC);
                addedPartyMember = false;
            }

            //Adds next caharacter to party
            if (addNewPartyMember)
            {
                GameManager.instance.characterSlots[partyMemberToAdd].SetActive(true);
            }
            /*
            if (addToPartyCharacter2 == true) 
            {
                if (!GameManager.instance.character1.activeInHierarchy)
                {
                    GameManager.instance.character1.SetActive(true);
                }
                addToPartyCharacter2 = false;
            }
            //Adds next caharacter to party
            if (addToPartyCharacter3 == true)
            {
                if (!GameManager.instance.character2.activeInHierarchy)
                {
                    GameManager.instance.character2.SetActive(true);
                }
                addToPartyCharacter3 = false;
            }*/

            //Opens inn menu
            if (isInn)
            {
                Inn.instance.OpenInn();
                dontOpenDialogAgain = true;
            }
            //Opens shop menu
            if (isShop)
            {

                Shop.instance.OpenShop();
                dontOpenDialogAgain = true;
            }

            //Marks quest complete
            if (shouldMarkQuest)
            {
                shouldMarkQuest = false;
                if (markQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                }
            }

            //Marks event complete
            if (shouldMarkEvent)
            {
                shouldMarkEvent = false;
                if (markEventComplete1)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                else
                {
                    EventManager.instance.MarkEventIncomplete(eventToMark);
                }
            }
        }
        
    }

    //Method to call the dialog. Needs the lines as string array + bool for 
    //Use this to call a dialog that is activated on awake/enter
    public void ShowDialogAuto(Sprite[] portraits, string[] newLines, bool isPerson)
    {
        if (newLines.Length != 0)
        {
            dialogPortraits = portraits;
            dialogLines = newLines;


            currentLine = 0;

            CheckIfName();
            CheckIfPortrait();

            dialogText.text = dialogLines[currentLine];
            dialogBox.SetActive(true);


            justStarted = false;

            nameBox.SetActive(isPerson);

            GameManager.instance.dialogActive = true;
        }

        if (newLines.Length == 0)
        {
            if (itemRecieved && !fullInventory)
            {
                GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemRecieved = false;
            }

            if (itemGiven)
            {
                GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
                StartCoroutine(gotItemMessageCo());
                itemGiven = false;
            }

            if (fullInventory)
            {
                Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
                StartCoroutine(Shop.instance.PromptCo());
                fullInventory = false;
            }

            if (addedPartyMember)
            {
                Destroy(NPC);
                addedPartyMember = false;
            }

            //Adds next caharacter to party
            if (addNewPartyMember)
            {
                GameManager.instance.characterSlots[partyMemberToAdd].SetActive(true);
            }
            /*
            if (addToPartyCharacter2 == true) 
            {
                if (!GameManager.instance.character1.activeInHierarchy)
                {
                    GameManager.instance.character1.SetActive(true);
                }
                addToPartyCharacter2 = false;
            }
            //Adds next caharacter to party
            if (addToPartyCharacter3 == true)
            {
                if (!GameManager.instance.character2.activeInHierarchy)
                {
                    GameManager.instance.character2.SetActive(true);
                }
                addToPartyCharacter3 = false;
            }*/

            //Opens inn menu
            if (isInn)
            {
                Inn.instance.OpenInn();
                dontOpenDialogAgain = true;
            }
            //Opens shop menu
            if (isShop)
            {

                Shop.instance.OpenShop();
                dontOpenDialogAgain = true;
            }

            //Marks quest complete
            if (shouldMarkQuest)
            {
                shouldMarkQuest = false;
                if (markQuestComplete)
                {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }
                else
                {
                    QuestManager.instance.MarkQuestIncomplete(questToMark);
                }
            }

            //Marks event complete
            if (shouldMarkEvent)
            {
                shouldMarkEvent = false;
                if (markEventComplete1)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                else
                {
                    EventManager.instance.MarkEventIncomplete(eventToMark);
                }
            }
        }

    }

    //Method to call good bye lines for closing message when exiting the shop/inn
    public void SayGoodBye(string[] goodByeLines, bool isPerson)
    {
        dialogLines = goodByeLines;
        GameMenu.instance.touchController.SetActive(false);
        finalMessage = goodByeLines;
        Shop.instance.sayGoodBye = goodByeLines;
        currentLine = 0;

        CheckIfName();
        CheckIfPortrait();

        dialogText.text = finalMessage[currentLine];
        dialogBox.SetActive(true);

        if (!ControlManager.instance.mobile)
        {
            justStarted = true;
        }

        if (ControlManager.instance.mobile)
        {
            justStarted = false;
        }
        
        
        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive = true;
        dontOpenDialogAgain = true;
    }

    public void SayGoodByeInn(string[] goodByeLines, bool isPerson, bool stay)
    {
        dialogLines = goodByeLines;
        GameMenu.instance.touchController.SetActive(false);
        finalMessage = goodByeLines;
        Shop.instance.sayGoodBye = goodByeLines;
        currentLine = 0;

        CheckIfName();
        CheckIfPortrait();

        dialogText.text = finalMessage[currentLine];
        dialogBox.SetActive(true);

        if (!ControlManager.instance.mobile)
        {
            justStarted = stay;
        }

        if (ControlManager.instance.mobile)
        {
            justStarted = false;
        }


        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive = true;
        dontOpenDialogAgain = true;
    }

    //Show name tag on dialog box. start a line with // to indicate a name. Dialog Activator script must have the isPerson bool true to display names 
    public void CheckIfName()
    {

        if (dialogLines[currentLine].StartsWith("//"))
        {
            nameText.text = dialogLines[currentLine].Replace("//", "");
            currentLine++;
        }
        else
        {
            CheckIfPortrait();
        }

    }

    public void CheckIfPortrait()
    {
        
        if (dialogPortraits != null)
        {
            if (dialogLines[currentLine].StartsWith("**"))
            {
                portraitText = dialogLines[currentLine].Replace("**", "");

                if (portraitText != "")
                {
                    portrait.color = new Color(1, 1, 1, 1);
                    int currentPortrait = Convert.ToInt32(portraitText);
                    portrait.sprite = dialogPortraits[currentPortrait];
                    currentLine++;
                }
                else
                {
                    portrait.color = new Color(1, 1, 1, 0);
                    currentLine++;
                }

            }
        }
        
    }

    //Method to complete a quest after dialog
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;

        shouldMarkQuest = true;
    }

    //Method to complete an event after dialog
    public void ActivateEventAtEnd(string eventName, bool markEventComplete)
    {
        eventToMark = eventName;
        markEventComplete1 = markEventComplete;

        shouldMarkEvent = true;
    }

    public IEnumerator gotItemMessageCo()
    {
        yield return new WaitForSeconds(.5f);
        GameMenu.instance.gotItemMessage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameMenu.instance.gotItemMessage.SetActive(false);

    }

    public void SelectDialogChoice(int buttonValue)
    {
        

        dialogObject.SetActive(false);
        dialogBox.SetActive(false);

        


        if (itemRecieved && !fullInventory)
        {
            GameMenu.instance.gotItemMessageText.text = "You found a " + Shop.instance.selectedItem.name + "!";
            StartCoroutine(gotItemMessageCo());
            itemRecieved = false;
        }

        if (itemGiven)
        {
            GameMenu.instance.gotItemMessageText.text = "You gave " + Shop.instance.selectedItem.name + "!";
            StartCoroutine(gotItemMessageCo());
            itemGiven = false;
        }

        if (fullInventory)
        {
            Shop.instance.promptText.text = "You found a " + Shop.instance.selectedItem.name + "." + "\n" + "But your equipment bag is full!";
            StartCoroutine(Shop.instance.PromptCo());
            fullInventory = false;
        }

        if (addedPartyMember)
        {
            Destroy(NPC);
            addedPartyMember = false;
        }

        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }

        if (buttonValue == 0)
        {
            choiceA.SetActive(true);
        }

        if (buttonValue == 1)
        {
            choiceB.SetActive(true);
        }

        dialogChoices.SetActive(false);
        choiceA = null;
        choiceB = null;
        GameManager.instance.dialogActive = false;


        //Adds next caharacter to party
        if (addNewPartyMember)
        {
            GameManager.instance.characterSlots[partyMemberToAdd].SetActive(true);
        }
        /*
        if (addToPartyCharacter2 == true) 
        {
            if (!GameManager.instance.character1.activeInHierarchy)
            {
                GameManager.instance.character1.SetActive(true);
            }
            addToPartyCharacter2 = false;
        }
        //Adds next caharacter to party
        if (addToPartyCharacter3 == true)
        {
            if (!GameManager.instance.character2.activeInHierarchy)
            {
                GameManager.instance.character2.SetActive(true);
            }
            addToPartyCharacter3 = false;
        }*/

        //Opens inn menu
        if (isInn)
        {
            Inn.instance.OpenInn();
            dontOpenDialogAgain = true;
        }
        //Opens shop menu
        if (isShop)
        {

            Shop.instance.OpenShop();
            dontOpenDialogAgain = true;
        }

        //Marks quest complete
        if (shouldMarkQuest)
        {
            shouldMarkQuest = false;
            if (markQuestComplete)
            {
                QuestManager.instance.MarkQuestComplete(questToMark);
            }
            else
            {
                QuestManager.instance.MarkQuestIncomplete(questToMark);
            }
        }

        //Marks event complete
        if (shouldMarkEvent)
        {
            shouldMarkEvent = false;
            if (markEventComplete1)
            {
                EventManager.instance.MarkEventComplete(eventToMark);
            }
            else
            {
                EventManager.instance.MarkEventIncomplete(eventToMark);
            }
        }

        
    }
}
