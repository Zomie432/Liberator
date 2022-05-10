using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionality : MonoBehaviour
{


    public GameObject pause;
    public GameObject hostageSecured;
    GameObject virtualCam;
    public static bool gameIsPaused = false;
   
    void Start()
    {
        pause = GameManager.Instance.pause;
        virtualCam = GameObject.FindGameObjectWithTag("VirtualCam");
    }

 

    public void PauseGame()
    {
        Debug.Log("Escape Pressed");
        if (gameIsPaused == false)
        {
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
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Resumes time
        Time.timeScale = 1f;
        // Re Enables players ability to look around and disables the Pause menu UI image
        virtualCam.SetActive(true);
        pause.SetActive(false);
        gameIsPaused = false;
        Debug.Log("Resuming Level");
    }

    public void Restart()
    {
        pause = GameManager.Instance.pause;
        pause.SetActive(false);
        virtualCam = GameObject.FindGameObjectWithTag("MainCamera");
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
