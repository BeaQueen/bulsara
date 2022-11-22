using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))]
public class CompleteQuest : MonoBehaviour {

    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;
    [Tooltip("Mark a quest as complete immediately")]
    public bool markComplete;
    [Tooltip("Mark a quest as complete when the player presses the confirm button")]
    public bool markOnButtonPress;
    [Tooltip("Mark a quest as complete when the player enters the trigger zone")]
    public bool markOnEnter;
    [Tooltip("Mark a quest as complete when the player exits the trigger zone")]
    public bool markOnExit;

    private bool canMark;

    [Tooltip("Deactivate this game object after the quest was marked as complete")]
    public bool deactivateOnMarking;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || CrossPlatformInputManager.GetButtonDown("RPGConfirmTouch") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            if (canMark && markOnButtonPress && !GameManager.instance.battleActive && !GameManager.instance.gameMenuOpen)
            {
                MarkQuest();
            }
        }
    }

    public void MarkQuest()
    {
        if(markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        } else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }

        gameObject.SetActive(!deactivateOnMarking);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (markOnEnter)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (markOnExit)
            {
                MarkQuest();
            }
            else
            {
                canMark = false;
            }
        }
    }
}
