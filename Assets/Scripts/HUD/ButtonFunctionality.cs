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

    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        mainCamera.SetActive(false);
        Debug.Log("Game Paused");
    }
    public void Resume()
    {
        Time.timeScale = 1f;
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
