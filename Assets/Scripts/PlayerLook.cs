using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private CinemachineVirtualCamera FPcam;
    //private float xRotation = 0f;
    [Header("For ySens, go to the FPVirtualCam and find the aim drop down")]
    [SerializeField] private float xSensitivity = 30f;
    

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * xSensitivity * Time.deltaTime;

        //rotate the player to look left and right
        transform.Rotate(mouseX  * Vector3.up);

        //CINEMACHINE VIRTUAL CAM CURRENTLY HANDLES UP AND DOWN ROTATION
        //calculate camera rotation for looking up and down
        //xRotation -= mouseY * Time.deltaTime;

        //keep the player from breaking their neck trying to look too far up or down
        //xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        //cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        //cam.transform.Rotate(Vector3.left, yRotation * Time.deltaTime);
        
    }

    public void LowerXSensitivity()
    {
        xSensitivity -= .25f;
    }

    public void RaiseXSensitivity()
    {
        xSensitivity += .25f;
    }
}
