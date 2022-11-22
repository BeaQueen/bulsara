using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera mainCamera;
    [Range(0, 1)]
    public float xMovementSpeed = 0.3f;
    [Range(0, 1)]
    public float yMovementSpeed = 0.3f;

    private void Awake()
    {
        mainCamera = mainCamera == null ? Camera.main : mainCamera;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(mainCamera.transform.position.x * xMovementSpeed, mainCamera.transform.position.y * yMovementSpeed);
    }
}
