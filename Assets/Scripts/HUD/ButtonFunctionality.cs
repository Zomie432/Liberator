using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionality : MonoBehaviour
{


    public GameObject pause;
    GameObject mainCamera;
    public static bool gameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("PauseMenu");
        pause.SetActive(false);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

 

    public void PauseGame()
    {
        if (gameIsPaused == false)
        {
            // Turns on Pause menu image
            pause.SetActive(true);
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Freezes time
            Time.timeScale = 0f;
            // Disables main camera so player can not look around in the pause menu
            mainCamera.SetActive(false);
            Debug.Log("Game Paused");
            gameIsPaused = true;
        }
        else
        {
            Resume();
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Resumes time
        Time.timeScale = 1f;
        // Re Enables players ability to look around and disables the Pause menu UI image
        mainCamera.SetActive(true);
        pause.SetActive(false);
        gameIsPaused = false;
        Debug.Log("Resuming Level");
    }

    public void Restart()
    {
        Debug.Log("Restarting Level");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application is Exiting");
    }
}
