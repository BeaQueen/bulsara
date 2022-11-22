using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjectActivator : MonoBehaviour
{
    [Tooltip("Drag and drop the game object that should be activated or deactivated")]
    public GameObject objectToActivate;
    [Tooltip("Choose the event whose completion should be checked from the Event Manager")]
    public string eventToCheck;
    [Tooltip("Activate the game object when the chosen event was completed. Leave unchecked if you want to deactivate the game object instead")]
    public bool activeIfComplete;
    [Tooltip("Activate a delay before the activation")]
    public bool waitBeforeActivate;
    [Tooltip("Enter the duration for the delay in seconds")]
    public float waitTime;

    private bool initialCheckDone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!initialCheckDone)
        {
            initialCheckDone = true;

            CheckCompletion();
        }
    }

    public void CheckCompletion()
    {
        if (EventManager.instance.CheckIfComplete(eventToCheck))
        {
            if (waitBeforeActivate)
            {
                StartCoroutine(waitCo());
            }
            else
            {
                objectToActivate.SetActive(activeIfComplete);
            }

        }
    }

    IEnumerator waitCo()
    {
        yield return new WaitForSeconds(waitTime);
        objectToActivate.SetActive(activeIfComplete);
    }
}
