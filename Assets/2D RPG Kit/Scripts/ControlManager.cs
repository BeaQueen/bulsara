using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    //Make instance of this script to be able reference from other scripts!
    public static ControlManager instance;

    public bool mobile = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);

        if (mobile)
        {
            GameMenu.instance.touchMenuButton.SetActive(true);
            GameMenu.instance.touchController.SetActive(true);
            GameMenu.instance.touchConfirmButton.SetActive(true);
        }
        else
        {
            GameMenu.instance.touchMenuButton.SetActive(false);
            GameMenu.instance.touchController.SetActive(false);
            GameMenu.instance.touchConfirmButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
