using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonScript : MonoBehaviour
{

    public bool isHelpOn;
    public GameObject uiHelpObject;

    // Start is called before the first frame update
    void Start()
    {
        uiHelpObject.SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown ("h")) {
 
            OnHelpButtonClicked();
        }
    }

    public void OnHelpButtonClicked()
    {
        uiHelpObject.SetActive(!uiHelpObject.activeInHierarchy);
    }
}
