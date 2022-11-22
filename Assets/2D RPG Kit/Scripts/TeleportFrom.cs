using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFrom : MonoBehaviour {

    [HideInInspector]
    public string teleportName;

    [Tooltip("Make the player character face down")]
    public bool facePlayerDown;
    [Tooltip("Make the player character face left")]
    public bool facePlayerLeft;
    [Tooltip("Make the player character face up")]
    public bool facePlayerUp;
    [Tooltip("Make the player character face right")]
    public bool facePlayerRight;

    // Use this for initialization
    void Start () {
		if(teleportName == PlayerController.instance.areaTransitionName)
        {
            PlayerController.instance.transform.position = transform.position;
            PlayerController.instance.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

            if (facePlayerDown)
            {

                //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
                PlayerController.instance.animator.SetFloat("lastMoveY", -1f);

                PlayerController.instance.animator.SetFloat("lastMoveX", 0);
            }

            if (facePlayerLeft)
            {

                //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
                PlayerController.instance.animator.SetFloat("lastMoveX", -1f);

                PlayerController.instance.animator.SetFloat("lastMoveY", 0);
            }

            if (facePlayerUp)
            {

                //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
                PlayerController.instance.animator.SetFloat("lastMoveY", 1f);

                PlayerController.instance.animator.SetFloat("lastMoveX", 0);
            }

            if (facePlayerRight)
            {

                //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
                PlayerController.instance.animator.SetFloat("lastMoveX", 1f);

                PlayerController.instance.animator.SetFloat("lastMoveY", 0);
            }
        }

        ScreenFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
