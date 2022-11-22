using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonEvents : MonoBehaviour
{
    [HideInInspector]
    public int toolbar;
    [HideInInspector]
    public string currentTab;

    [HideInInspector]
    public bool blackScreen;
    [HideInInspector]
    public bool fadeFromBlack;

    [Header("Transition Settings")]
    [Tooltip("Activates the screen to fade to black and back again for a specified duration")]
    public bool activateScreenFade;
    [Tooltip("Duration for the screenfade in seconds")]
    public float fadeTime = 1;
    

    [Header("Display Settings")]
    [Tooltip("Disable the game menu. Helpful in cutscenes where the player shouldn't be able to bring up the menu")]
    public bool blockGameMenu;
    [Tooltip("Hide the touch interface in mobile games. Helpful in cut scenes where the player shouldn't see the touch interface")]
    public bool hideTouchButtons;
    [Tooltip("Bring back the touch interface in mobile games")]
    public bool showTouchButtons;

    [Header("Event Settings")]
    [Tooltip("Mark an event as complete after a screen fade. Works only with activated screen fade")]
    public bool markEventCompleteAfterFade;
    [Tooltip("Mark an event as complete immediately")]
    public bool markEventComplete;
    [Tooltip("Enter the event that should be completed. This event has to be registered in the Event Manager")]
    public string eventToMark;

    [Header("Quest Settings")]
    [Tooltip("Mark a quest as complete after a screen fade. Works only with activated screen fade")]
    public bool markQuestCompleteAfterFade;
    [Tooltip("Mark a quest as complete immediately")]
    public bool markQuestComplete;
    [Tooltip("Enter the quest that should be completed. This quest has to be registered in the Quest Manager")]
    public string questToMark;

    [Header("Player Settings")]
    [Tooltip("Disable player movement")]
    public bool lockPlayer;
    [Space]
    [Tooltip("Make player character invisible. Helpful for cutscenes without the protagonist")]
    public bool hidePlayer;
    [Space]
    [Tooltip("Make the player character face down")]
    public bool facePlayerDown;
    [Tooltip("Make the player character face left")]
    public bool facePlayerLeft;
    [Tooltip("Make the player character face up")]
    public bool facePlayerUp;
    [Tooltip("Make the player character face right")]
    public bool facePlayerRight;
    [Space]
    [Tooltip("Change the location of the player character with x and y values. z value should usually remain unchanged")]
    public bool transposePlayer;
    public float x;
    public float y;
    public float z;
    [Space]
    [Tooltip("Change the players' size")]
    public bool ChangePlayerSize;
    public Vector3 newSize;
    [Space]
    [Tooltip("Replace player")]
    public bool replacePlayer;
    [Tooltip("Provide the animator of the new player")]
    public Animator playerAnimator;
    [Space]
    [Tooltip("Enter the party member slot of current player")]
    public int currentPartyMemberSlot;
    [Tooltip("Enter the party member slot of next player")]
    public int nextPartyMemberSlot;

    [Header("Environment Settings")]
    [Tooltip("Activate another background music")]
    public bool changeBGM;
    [Tooltip("Enter the number corresponding to the background music from the Audio Manager")]
    public int BGM;
    [Tooltip("Activate day time lighting")]
    public bool dayTime;
    [Tooltip("Activate night time lighting")]
    public bool nightTime;

    [Header("Teleport Settings")]
    [Tooltip("Load another scene")]
    public bool changeScene;
    [Tooltip("Enter the scene name")]
    public string scene;
    [Tooltip("Enter the duration of transition to the new scene in seconds")]
    public float transitionTime = 1f;
    [Tooltip("Enter the players' position in the next scene")]
    public Vector2 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (blackScreen)
        {
            ScreenFade.instance.fadeFromBlack = false;
            ScreenFade.instance.fadeScreenImage.color = new Color(0,0,0,1);
        }

        if (changeBGM)
        {
            AudioManager.instance.PlayBGM(BGM);
        }
        
        if(transposePlayer)
        {
            PlayerController.instance.transform.position = new Vector3(x, y, z);
        }

        if (ChangePlayerSize)
        {
            PlayerController.instance.gameObject.transform.localScale = newSize;
        }

        if (replacePlayer)
        {
            Animator anim = PlayerController.instance.GetComponent<Animator>();
            anim.runtimeAnimatorController = playerAnimator.runtimeAnimatorController;
            GameManager.instance.characterSlots[currentPartyMemberSlot].SetActive(false);
            GameManager.instance.characterSlots[nextPartyMemberSlot].SetActive(true);
            GameManager.instance.characterSlots[nextPartyMemberSlot].transform.SetAsFirstSibling();


            GameManager.instance.characterStatus.Insert(0, GameManager.instance.characterStatus[nextPartyMemberSlot]);
            GameManager.instance.characterStatus.RemoveAt(nextPartyMemberSlot + 1);
        }
        
        if(hidePlayer)
        {
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
        }else
        {
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        }
        
        if (facePlayerDown)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveY", -1f);
        }

        if (facePlayerLeft)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveX", -1f);
        }

        if (facePlayerUp)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveY", 1f);
        }

        if (facePlayerRight)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveX", 1f);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if(blockGameMenu)
        {
            GameManager.instance.cutSceneActive = true;
        }else
        {
            GameManager.instance.cutSceneActive = false;
        }

        if(hideTouchButtons)
        {
            //Hide touch interface
            GameMenu.instance.touchConfirmButton.SetActive(false);
            GameMenu.instance.touchMenuButton.SetActive(false);
            GameMenu.instance.touchController.SetActive(false);
        }
        if(showTouchButtons)
        {
            if (ControlManager.instance.mobile == true)
            {
                GameMenu.instance.touchMenuButton.SetActive(true);
                GameMenu.instance.touchController.SetActive(true);
                GameMenu.instance.touchConfirmButton.SetActive(true);
            }
        }


        if(lockPlayer)
        {
            //Disable player movement
            PlayerController.instance.canMove = false;
        }

        

        if (activateScreenFade)
        {
            ScreenFade.instance.FadeToBlack();
            fadeTime -= Time.deltaTime;
            if (fadeTime <= 0)
            {
                if(markQuestCompleteAfterFade)
                {
                    //DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markQuestCompleteAfterFade);
                    QuestManager.instance.MarkQuestComplete(questToMark);
                }

                if (markEventCompleteAfterFade)
                {
                    EventManager.instance.MarkEventComplete(eventToMark);
                }
                ScreenFade.instance.FadeFromBlack();
                activateScreenFade = false;
            }
        }

        if (fadeFromBlack)
        {
            ScreenFade.instance.FadeFromBlack();
        }

        if (markQuestComplete)
        {
                    QuestManager.instance.MarkQuestComplete(questToMark);
                    GameManager.instance.cutSceneActive = false;
        }

        if (markEventComplete)
        {
            EventManager.instance.MarkEventComplete(eventToMark);
            GameManager.instance.cutSceneActive = false;
        }

        if (changeScene)
        {

            //GameManager.instance.fadingBetweenAreas = true;

            ScreenFade.instance.FadeToBlack();
            transitionTime -= Time.deltaTime;
            if (transitionTime <= 0)
            {
                PlayerController.instance.transform.position = newPosition;
                SceneManager.LoadScene(scene);
                
            }
            
           // }
        }

        if (nightTime)
        {
            //Set directional light to night time
            DirectionalLight.instance.GetComponent<Light>().intensity = .1f;
            DirectionalLight.instance.GetComponent<Light>().color = new Color(0, .2f, 1, 1);
            nightTime = false;
        }

        if (dayTime)
        {
            //Set directional light to night time
            DirectionalLight.instance.GetComponent<Light>().intensity = 1.4f;
            DirectionalLight.instance.GetComponent<Light>().color = new Color(1, 1, 1, 1);
            dayTime = false;
        }
    } 
}
