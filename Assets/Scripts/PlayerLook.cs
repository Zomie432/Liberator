using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform cam;
    private float xRotation = 0f;
    [SerializeField] private float xSensitivity = 5f;
    [SerializeField] private float ySensitivity = 15f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * xSensitivity;
        float mouseY = input.y * ySensitivity;

        //rotate the player to look left and right
        transform.Rotate((mouseX * Time.deltaTime) * xSensitivity * Vector3.up);

        //calculate camera rotation for looking up and down
        xRotation -= mouseY * Time.deltaTime;

        //keep the player from breaking their neck trying to look too far up or down
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;

        cam.eulerAngles = targetRotation;
        
        //cam.transform.Rotate(Vector3.left, yRotation * Time.deltaTime);
    }

}
