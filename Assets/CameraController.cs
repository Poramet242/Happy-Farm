using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //BG
    public SpriteRenderer spriteRenderer;
    //main camera
    public Camera orthographicCamera;

    private void Start()
    {
        CalculateOrthographicSize();
    }

    private void CalculateOrthographicSize()
    {
        // Get the size of the sprite
        float spriteHeight = spriteRenderer.bounds.size.x;

        // Get the current aspect ratio of the screen
        float aspectRatio = Screen.width / (float)Screen.height;

        // Calculate the desired orthographic size
        float orthographicSize = spriteHeight / 2f;

        // Adjust orthographic size based on aspect ratio
        if (aspectRatio >= 1f)
        {
            orthographicSize /= aspectRatio;
        }

        // Set the calculated orthographic size to the camera
        orthographicCamera.orthographicSize = orthographicSize;
    }

}
