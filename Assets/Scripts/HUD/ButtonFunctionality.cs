using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{


    public GameObject pause;
    GameObject virtualCam;
    GameObject reticle;
    public static bool gameIsPaused = false;

    void Start()
    {
        // Get instances of pause menu and Virtual cam
        pause = GameManager.Instance.pause;
        virtualCam = GameManager.Instance.virtualCam;
        reticle = GameManager.Instance.reticle;
    }


    public void PauseGame()
    {
        Debug.Log("Escape Pressed");
        if (gameIsPaused == false)
        {
            // Turn off Reticle
            reticle.SetActive(false);
            // Turns on Pause menu image
            pause.SetActive(true);
            Debug.Log("Pause Menu Active in Hierarchy: " + pause.activeInHierarchy);
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Freezes time
            Time.timeScale = 0f;
            // Disables virtual camera so player can not look around in the pause menu
            virtualCam.SetActive(false);
            Debug.Log("Game Paused");
            gameIsPaused = true;
        }
        else
        {
            Resume();
            Debug.Log("Resume entered through pause game");
        }
    }
    public void Resume()
    {
        if (virtualCam == null)
        {
            Debug.Log("Reticle is Null");
        }
        // Turn Reticle back on
        reticle.SetActive(true);

        // Set Cursor state back to locked and turn visibility off
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Resumes time
        Time.timeScale = 1f;
        // Re Enables players ability to look around and disables the Pause menu UI image
        if (reticle == null)
        {
            Debug.Log("Virtual Cam is Null");
        }
        virtualCam.SetActive(true);
        pause.SetActive(false);
        gameIsPaused = false;
        Debug.Log("Resuming Level");
    }

    public void Restart()
    {
        // Resume time
        Time.timeScale = 1f;
        // Get instance of Pause menu and turn it off
        pause = GameManager.Instance.pause;
        pause.SetActive(false);
        // Get instance of virtual camera
        virtualCam = GameManager.Instance.virtualCam;
        // find Instance of Reticle
        reticle = GameManager.Instance.reticle;
        // Find active scene and reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Debug.Log("Restarting Level");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Application is Exiting");
    }
}
