using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    [Tooltip("Make the player character face down")]
    public bool facePlayerDown;
    [Tooltip("Make the player character face left")]
    public bool facePlayerLeft;
    [Tooltip("Make the player character face up")]
    public bool facePlayerUp;
    [Tooltip("Make the player character face right")]
    public bool facePlayerRight;

    // Start is called before the first frame update
    void Start()
    {
        if (facePlayerDown)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveY", -1f);
            PlayerController.instance.animator.SetFloat("lastMoveX", 0f);
        }

        if (facePlayerLeft)
        {

            //PlayerController.instance.rigidBody.velocity = new Vector2(-30, 0);
            PlayerController.instance.animator.SetFloat("lastMoveX", -1f);
            PlayerController.instance.animator.SetFloat("lastMoveY", 0f);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
