using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TeleportTo : MonoBehaviour {
    [Tooltip("Enter the scene name")]
    public string scene;
    [Tooltip("Assign a unique ID to this teleport")]
    public string teleportName;

    public TeleportFrom entry;
    [Tooltip("Enter the duration of transition to the new scene in seconds")]
    public float transitionTime = 1f;
    private bool openScene;

    [FormerlySerializedAs("collider")]
    public BoxCollider2D teleportCollider;

    // Use this for initialization
    void Start () {
        entry.teleportName = teleportName;

    }
	
	// Update is called once per frame
	void Update () {
		if(openScene)
        {
            transitionTime -= Time.deltaTime;
            if(transitionTime <= 0)
            {
                openScene = false;
                SceneManager.LoadScene(scene);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            openScene = true;
            GameManager.instance.fadingBetweenAreas = true;

            GameMenu.instance.gotItemMessage.SetActive(false);

            ScreenFade.instance.FadeToBlack();

            PlayerController.instance.areaTransitionName = teleportName;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, .3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(new Vector3(teleportCollider.offset.x, teleportCollider.offset.y, -2), teleportCollider.size);
    }
}
