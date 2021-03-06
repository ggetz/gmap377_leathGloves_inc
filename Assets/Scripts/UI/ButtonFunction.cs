﻿using UnityEngine;

public class ButtonFunction : MonoBehaviour
{

    /// <summary>
    /// Called every frame
    /// </summary>
    void Update()
    {
        // If xbox start button (Windows/Linux) or the 1 key is pressed, start the game
        if (InputManager.PlayerStartInput > float.Epsilon || Input.GetKeyDown(KeyCode.Return))
        {
            Application.LoadLevel("OriginalScene");
        }
    }
}
