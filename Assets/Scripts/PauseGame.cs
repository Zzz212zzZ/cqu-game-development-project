using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Unpause the game
                Time.timeScale = 1;
                isPaused = false;
            }
            else
            {
                // Pause the game
                Time.timeScale = 0;
                isPaused = true;
            }
        }
    }
}

