using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //singleton reference to the input manager
    private static InputManager _instance;
    public static InputManager Instance { get { return _instance; } }
    //reference to the C# class we generated with the input system
    private PlayerInput playerInput;

    //reference to the specific "OnFoot" action map(has specific actions like walk, slow-walk, jump, crouch)
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //we want to make sure there is only ever one input manager instance
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        //set the "Jump" action in the "OnFoot" action map to point to the Jump function in the player motor script
        //basically just says "Hey, if the player jumps call this function"
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.SlowWalk.performed += ctx => motor.SlowWalk();
        onFoot.LowerXSensitivity.performed += ctx => look.LowerXSensitivity();
        onFoot.RaiseXSensitivity.performed += ctx => look.RaiseXSensitivity();

    }

    // Update is called once per frame
    void Update()
    {
        //tell the playerMotor to move using the value from the "movement" action(WASD)
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        //tell the playerLook to turn using the value from the "look" action(mouse)
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        //starts processing the action map "onFoot"
        onFoot.Enable();

    }

    private void OnDisable()
    {
        //disables processing the action map "onFoot"
        onFoot.Disable();
    }
}