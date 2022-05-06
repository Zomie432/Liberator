using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctionality : MonoBehaviour
{


    GameObject pause;
    GameObject mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("PauseMenu");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        //if (pause.activeInHierarchy)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = true;
        //}
    }

    public void PauseGame()
    {
        // Freezes time
        Time.timeScale = 0f;
        // Disables main camera so player can not look around in the pause menu
        mainCamera.SetActive(false);
        Debug.Log("Game Paused");
    }
    public void Resume()
    {
        // Resumes time
        Time.timeScale = 1f;
        // Re Enables players ability to look around and disables the Pause menu UI image
        mainCamera.SetActive(true);
        pause.SetActive(false);
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
