using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class DialogStarter : MonoBehaviour {

    [HideInInspector]
    public int numberOfItemsHeld;
    [HideInInspector]
    public int numberOfEquipItemsHeld;

    [Header("NPC appereance")]
    public SpriteRenderer headSpriteRenderer;
    public SpriteRenderer bodySpriteRenderer;
    Sprite defaultHeadSprite;
    Sprite defaultBodySprite;

    public Sprite headUp;
    public Sprite headDown;
    public Sprite headLeft;
    public Sprite headRight;
    public Sprite bodyUp;
    public Sprite bodyDown;
    public Sprite bodyLeft;
    public Sprite bodyRight;

    public bool saveDefaultRotation;

    [Header("Character Portraits")]
    public Sprite[] portraits;

    [Header("Dialog Lines")]
    //The lines the npcs say when the player talks to them
    [Tooltip("The lines the npcs say when the player talks to them. One line can fill an entire message box with appropriate line breaks")]
    public string[] lines;

    [Space(10)]
    [Tooltip("Set the text for dialog choice A")]
    public string ChoiceAText;
    [Tooltip("Set the game object to activate for dialog choice A")]
    public GameObject choiceA;

    [Space(10)]
    [Tooltip("Set the text for dialog choice B")]
    public string ChoiceBText;
    [Tooltip("Set the game object to activate for dialog choice B")]
    public GameObject choiceB;

    [Tooltip ("Inn and shop keepers will say these lines after closing their menus") ]
    [UnityEngine.Serialization.FormerlySerializedAs("sayGoodBye")]
    public string[] finalMessage;

    //Check wheather the player is in range to talk to npc
    private bool canActivate;

    [Header("Activation")]
    //For different activation methods
    [Tooltip("Activates the dialog as soon as this script is activated. Keep in mind that the player character still has to be in the trigger zone")]
    public bool activateOnAwake;
    [Tooltip("Activates the dialog when the player presses the confirm button")]
    public bool activateOnButtonPress;
    [Tooltip("Activates the dialog when the player enters the trigger zone")]
    public bool activateOnEnter;
    [Tooltip("Activate a delay before showing the dialog")]
    public bool waitBeforeActivatingDialog;
    [Tooltip("Enter the duration of the delay in seconds")]
    public float waitTime;

    [Header("NPC Settings")]
    //Check if the player talks to a person for displaying a name tag
    [Tooltip("Display a name tag")]
    public bool displayName = true;

    //If npc should join your party
    [Tooltip("Let the NPC join the players party at a given slot")]
    public bool addNewPartyMember;
    [Tooltip("Choose the characters position in the Game Manager")]
    public int partyMemberToAdd;
    [Tooltip("Removes the NPC from scene after joining")]
    public bool destroyAfterJoining;

    [Header("Inn Settings")]
    //If npc should be an inn keeper
    [Tooltip("Activates the inn menu")]
    public bool isInn;
    [Tooltip("Set the price for one night")]
    public int innPrice;

    [Header("Shop Settings")]
    //If npc should be a shop keeper
    [Tooltip("Activates the shop menu")]
    public bool isShop;
    [Tooltip("Enter all items that should be on sale in this shop")]
    public string[] ItemsForSale = new string[40];

    [Header("Receive Settings")]
    [Tooltip("Receive an item after conversation")]
    public bool receiveItem;    
    [Tooltip("Drag and drop an item prefab")]
    public Item itemToReceive;
    [Tooltip("Give an item after conversation")]
    public bool giveItem;
    [Tooltip("Drag and drop an item prefab")]
    public Item itemToGive;
    [Tooltip("Receive gold after conversation")]
    public bool receiveGold;
    [Tooltip("The amount of gold received")]
    public int goldAmount;

    [Header("Quest Settings")]
    //For completing quests after dialog
    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;
    [Tooltip("Mark a quest as complete after the dialog")]
    public bool markComplete;

    [Header("Event Settings")]
    //For completing quests after dialog
    //public bool shouldActivateQuest;
    [Tooltip("Enter the event that should be completed. This quest has to be registered in the Quest Manager")]
    public string eventToMark;
    [Tooltip("Mark an event as complete after the dialog")]
    public bool markEventComplete;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
        //Check if dialog should be activated on awake or enter
        if (activateOnAwake || activateOnEnter)
        {
            //Check if player is in reach and if no other dialog is currently active
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy)
            {
                PlayerController.instance.canMove = false;
                //Set this to false to prevent activating dialog endlessly
                activateOnEnter = false;

                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }
                    else
                    {
                        activateOnAwake = false;

                        //Hide mobile controller during dialogs
                        GameMenu.instance.touchMenuButton.SetActive(false);
                        GameMenu.instance.touchController.SetActive(false);
                        GameMenu.instance.touchConfirmButton.SetActive(false);

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = itemToReceive;
                            DialogManager.instance.itemRecieved = true;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;
                            numberOfEquipItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
                            {
                                if (GameManager.instance.equipItemsHeld[i] != "")
                                {
                                    numberOfEquipItemsHeld++;
                                }
                            }

                            if (itemToReceive)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }

                                }

                                if (Shop.instance.selectedItem.defense || Shop.instance.selectedItem.offense)
                                {
                                    if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        //StartCoroutine(gotItemMessageCo());
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }
                                }
                            }
                        }

                        if (giveItem)
                        {
                            Shop.instance.selectedItem = itemToGive;
                            DialogManager.instance.itemGiven = true;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.itemsHeld[i] = "";
                                    giveItem = false;
                                }

                                if (GameManager.instance.equipItemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.equipItemsHeld[i] = "";
                                    giveItem = false;
                                }
                            }
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }

                        //Add new member to party
                        if (addNewPartyMember)
                        {
                            DialogManager.instance.addNewPartyMember = addNewPartyMember;
                            DialogManager.instance.partyMemberToAdd = partyMemberToAdd;
                            DialogManager.instance.NPC = gameObject;

                            if (destroyAfterJoining)
                            {
                                DialogManager.instance.addedPartyMember = true;
                            }
                            
                        }

                        //Show inn menu
                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = finalMessage;
                        }

                        //Show shop menu
                        if (isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = finalMessage;
                        }
                        
                        DialogManager.instance.ShowDialogAuto(portraits, lines, displayName);
                        
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }
                }
            }
        }

        //Check for button input
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            
            if (canActivate && !DialogManager.instance.dialogBox.activeInHierarchy && !Inn.instance.innMenu.activeInHierarchy && !GameMenu.instance.menu.activeInHierarchy && !GameManager.instance.battleActive)
            {
                PlayerController.instance.canMove = false;

                if (headSpriteRenderer != null && bodySpriteRenderer != null)
                {
                    if (PlayerController.instance.facingUp)
                    {
                        headSpriteRenderer.sprite = headDown;
                        bodySpriteRenderer.sprite = bodyDown;
                    }

                    if (PlayerController.instance.facingDown)
                    {
                        headSpriteRenderer.sprite = headUp;
                        bodySpriteRenderer.sprite = bodyUp;
                    }

                    if (PlayerController.instance.facingLeft)
                    {
                        headSpriteRenderer.sprite = headRight;
                        bodySpriteRenderer.sprite = bodyRight;
                    }

                    if (PlayerController.instance.facingRight)
                    {
                        headSpriteRenderer.sprite = headLeft;
                        bodySpriteRenderer.sprite = bodyLeft;
                    }
                }
                
                //activateOnEnterConfirm = false;
                if (!DialogManager.instance.dontOpenDialogAgain)
                {
                    if (waitBeforeActivatingDialog)
                    {
                        //Disable player movement
                        PlayerController.instance.canMove = false;
                        StartCoroutine(waitCo());
                    }else
                    {
                        activateOnAwake = false;
                        GameMenu.instance.touchMenuButton.SetActive(false);
                        GameMenu.instance.touchController.SetActive(false);
                        GameMenu.instance.touchConfirmButton.SetActive(false);

                        //Add item after conversation
                        if (receiveItem)
                        {
                            //Take the reference for isItem/isWeapon/isArmour from shop instance
                            Shop.instance.selectedItem = itemToReceive;
                            DialogManager.instance.itemRecieved = true;

                            //Calculate the amount of items / equipment held in inventory to prevent adding more items if inventory is full
                            numberOfItemsHeld = 0;
                            numberOfEquipItemsHeld = 0;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] != "")
                                {
                                    numberOfItemsHeld++;
                                }
                            }

                            for (int i = 0; i < GameManager.instance.equipItemsHeld.Length; i++)
                            {
                                if (GameManager.instance.equipItemsHeld[i] != "")
                                {
                                    numberOfEquipItemsHeld++;
                                }
                            }

                            if (itemToReceive)
                            {
                                if (Shop.instance.selectedItem.item)
                                {
                                    if (numberOfItemsHeld < GameManager.instance.itemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }

                                }

                                if (Shop.instance.selectedItem.defense || Shop.instance.selectedItem.offense)
                                {
                                    if (numberOfEquipItemsHeld < GameManager.instance.equipItemsHeld.Length)
                                    {
                                        GameMenu.instance.gotItemMessageText.text = "You found a " + itemToReceive.itemName + "!";
                                        //StartCoroutine(gotItemMessageCo());
                                        GameManager.instance.AddItem(itemToReceive.itemName);
                                        receiveItem = false;
                                    }
                                    else
                                    {
                                        DialogManager.instance.fullInventory = true;
                                        receiveItem = true;
                                    }
                                }
                            }                            
                        }

                        if (giveItem)
                        {
                            Shop.instance.selectedItem = itemToGive;
                            DialogManager.instance.itemGiven = true;

                            for (int i = 0; i < GameManager.instance.itemsHeld.Length; i++)
                            {
                                if (GameManager.instance.itemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.itemsHeld[i] = "";
                                    giveItem = false;
                                }

                                if (GameManager.instance.equipItemsHeld[i] == itemToGive.name)
                                {
                                    GameManager.instance.equipItemsHeld[i] = "";
                                    giveItem = false;
                                }
                            }
                        }

                        //Add gold after conversation
                        if (receiveGold)
                        {
                            GameMenu.instance.gotItemMessageText.text = "You found " + receiveGold + " Gold!";
                            //StartCoroutine(gotItemMessageCo());
                            GameManager.instance.currentGold += goldAmount;
                            receiveGold = false;
                        }

                        //Add new member to party
                        if (addNewPartyMember)
                        {
                            DialogManager.instance.addNewPartyMember = addNewPartyMember;
                            DialogManager.instance.partyMemberToAdd = partyMemberToAdd;
                            DialogManager.instance.NPC = gameObject;

                            if (destroyAfterJoining)
                            {
                                DialogManager.instance.addedPartyMember = true;
                            }
                        }

                        if (isInn)
                        {
                            DialogManager.instance.isInn = isInn;
                            DialogManager.instance.innPrice = innPrice;
                            Inn.instance.sayGoodBye = finalMessage;
                        }

                        if(isShop)
                        {
                            DialogManager.instance.isShop = isShop;
                            Shop.instance.itemsForSale = ItemsForSale;
                            Shop.instance.sayGoodBye = finalMessage;
                        }

                        DialogManager.instance.choiceA = choiceA;
                        DialogManager.instance.choiceB = choiceB;
                        DialogManager.instance.choiceALabel.text = ChoiceAText;
                        DialogManager.instance.choiceBLabel.text = ChoiceBText;
                        DialogManager.instance.dialogObject = gameObject;

                        DialogManager.instance.ShowDialog(portraits, lines, displayName);


                        //DialogManager.instance.SayGoodBye(sayGoodBye, isPerson);
                        DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
                        if (markEventComplete)
                        {
                            DialogManager.instance.ActivateEventAtEnd(eventToMark, markEventComplete);
                        }
                        
                    }                    
                }
            }            
        }
	}

    //Check if player enters trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Facing Collider")
        {
            canActivate = true;
            //DialogManager.instance.dontOpenDialogAgain = false;
            
        }
    }

    //Check if player exits trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Facing Collider")
        {
            canActivate = false;

            if (!activateOnButtonPress)
            {
                activateOnEnter = true;
            }
        }
    }

    //Put in a slight delay between activating the dialog and showing the dialog
    IEnumerator waitCo()
    {

        yield return new WaitForSeconds(waitTime);
        waitBeforeActivatingDialog = false;
        
    }
}
