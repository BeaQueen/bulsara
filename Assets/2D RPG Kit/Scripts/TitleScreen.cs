using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour {

    public int music;
    public string newGameScene;

    public GameObject continueButton;
    public GameObject mainMenu;
    public GameObject difficultySettings;
    public Text pressStartText;
    public GameObject pressStart;
    public Button newGameBtn;
    public Button continueBtn;
    public Button normalBtn;


    //Event sytsem
    public EventSystem es;

    public string loadGameScene;

	// Use this for initialization
	void Start () {
        if(ControlManager.instance.mobile)
        {
            Screen.SetResolution(1280, 720, true);
        }
        Screen.SetResolution(1280, 720, true);
        Cursor.visible = false;
        StopCoroutine(PressStartCo());
        StartCoroutine(PressStartCo());
        ScreenFade.instance.fadeScreenObject.SetActive(false);
        AudioManager.instance.PlayBGM(music);
        GameManager.instance.cutSceneActive = true;
    }
	
	// Update is called once per frame
	void Update () {
        PlayerController.instance.canMove = false;
        GameMenu.instance.touchMenuButton.SetActive(false);
        GameMenu.instance.touchController.SetActive(false);
        GameMenu.instance.touchConfirmButton.SetActive(false);

        if (Input.GetButtonDown("RPGConfirmPC") || Input.GetButtonDown("RPGConfirmJoy") || Input.GetButtonDown("Submit"))
            
        {
            if (!mainMenu.activeInHierarchy)
            {
                StopCoroutine(PressStartCo());
                PlayButtonSound(0);

                if (!difficultySettings.activeInHierarchy)
                {
                    ShowMainMenu();
                }
                
            }            
        } 
    }

    public void PlayButtonSound (int buttonSound)
    {
        AudioManager.instance.PlaySFX(buttonSound);
    }

    public void ShowMainMenu()
    {     
        if (ControlManager.instance.mobile == true)
        {
            pressStart.SetActive(false);
            
            mainMenu.SetActive(true);
            
            
            if (PlayerPrefs.HasKey("Current_Scene"))
            {
                continueButton.SetActive(true);

            }
            else
            {
                continueBtn.interactable = false;
            }
        }

        else
        {
            if (PlayerPrefs.HasKey("Current_Scene"))
            {
                continueButton.SetActive(true);
                continueBtn.interactable = false;
                if (ControlManager.instance.mobile == false)
                {
                    es.SetSelectedGameObject(continueBtn.gameObject);
                    // Select the button
                    continueBtn.Select();
                    // Highlight the button
                    continueBtn.OnSelect(null);
                    StartCoroutine(WaitForButton());
                }

            }
            else
            {
                //continueButton.SetActive(false);
                continueButton.SetActive(true);
                continueBtn.interactable = false;
                newGameBtn.interactable = false;

                if (ControlManager.instance.mobile == false)
                {
                    es.SetSelectedGameObject(newGameBtn.gameObject);
                    // Select the button
                    newGameBtn.Select();
                    // Highlight the button
                    newGameBtn.OnSelect(null);
                    StartCoroutine(WaitForButton());
                }
            }
        }     
    }

    public void Continue()
    {
        ScreenFade.instance.fadeScreenObject.SetActive(true);

        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }
        GameManager.instance.cutSceneActive = false;
        SceneManager.LoadScene(loadGameScene);

        
    }

    public void OpenDifficultySettings()
    {
        mainMenu.SetActive(false);
        difficultySettings.SetActive(true);

        if (ControlManager.instance.mobile == false)
        {
            es.SetSelectedGameObject(normalBtn.gameObject);
            // Select the button
            normalBtn.Select();
            // Highlight the button
            normalBtn.OnSelect(null);
        }
    }

    public void NewGame(int difficulty)
    {
        if (difficulty == 0)
        {
            GameManager.instance.easy = true;
            GameManager.instance.normal = false;
            GameManager.instance.hard = false;
        }

        if (difficulty == 1)
        {
            GameManager.instance.easy = false;
            GameManager.instance.normal = true;
            GameManager.instance.hard = false;
        }

        if (difficulty == 2)
        {
            GameManager.instance.easy = false;
            GameManager.instance.normal = false;
            GameManager.instance.hard = true;
        }

        ScreenFade.instance.fadeScreenObject.SetActive(true);

        if (ControlManager.instance.mobile == true)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }
        PlayerController.instance.canMove = true;
        GameManager.instance.cutSceneActive = true;
        SceneManager.LoadScene(newGameScene);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public IEnumerator PressStartCo()
    {
        while (true)
        {
            switch (pressStartText.color.a.ToString())
            {
                case "0":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 1);
                    yield return new WaitForSeconds(0.5f);
                    break;
                case "1":
                    pressStartText.color = new Color(pressStartText.color.r, pressStartText.color.g, pressStartText.color.b, 0);
                    yield return new WaitForSeconds(0.5f);
                    break;
            }
        }
    }

    //Wait before activating the continue/new game button. 
    //If this isn't called after "Press Start", the game will assume you have pressed the either the new game or the continue button! 
    public IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(0.1f);

        newGameBtn.interactable = true;

        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueBtn.interactable = true;
        }

        yield return new WaitForSeconds(0.1f);

        pressStart.SetActive(false);
        mainMenu.SetActive(true);

    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
