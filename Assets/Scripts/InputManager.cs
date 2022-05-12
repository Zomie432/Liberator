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
    private PlayerInteract interact;

    // Player class
    private Player player;

    // button Functionality class
     ButtonFunctionality buttonFunc;

    void Awake()
    {
        //we want to make sure there is only ever one input manager instance
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        player = GetComponent<Player>();
        interact = GetComponent<PlayerInteract>();

        //set the "Jump" action in the "OnFoot" action map to point to the Jump function in the player motor script
        //basically just says "Hey, if the player jumps call this function"
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.SlowWalk.performed += ctx => motor.SlowWalk();
        onFoot.LowerXSensitivity.performed += ctx => look.LowerXSensitivity();
        onFoot.RaiseXSensitivity.performed += ctx => look.RaiseXSensitivity();

        //door interaction
        onFoot.Interact.performed += ctx => interact.ProcessInteraction(true); //true for press interactions

        //hostage interaction
        onFoot.HoldInteract.started += ctx => interact.ProcessInteraction(false); //false for hold interactions
        onFoot.HoldInteract.canceled += ctx => interact.CancelHostageSecure(); //if player lets go of E before the hold duration
        onFoot.HoldInteract.performed += ctx => interact.PerformHostageSecure(); //if player completes the "E" hold duration

        // Player input
        onFoot.AttackPressed.performed += ctx => player.OnAttackPressed();
        onFoot.AttackHold.performed += ctx => player.OnAttackHold();

        onFoot.ADSPressed.performed += ctx => player.OnADSPressed();
        onFoot.ADSReleased.performed += ctx => player.OnADSReleased();

        onFoot.ReloadPressed.performed += ctx => player.OnReloadPressed();

        onFoot.EquipNextWeaponPressed.performed += ctx => player.OnEquipNextPressed();
        onFoot.EquipPreviousWeaponPressed.performed += ctx => player.OnEquipPreviousPressed();

        onFoot.EquipWeaponOnePressed.performed += ctx => player.EquipWeaponOnePressed();
        onFoot.EquipWeaponTwoPressed.performed += ctx => player.EquipWeaponTwoPressed();

        onFoot.EquipFlashbangPressed.performed += ctx => player.EquipFlashbang();

        onFoot.PauseGame.performed += ctx => buttonFunc.PauseGame();

        // DEBUG-----------------------------------------------------------------------------------
        onFoot.Test_PlayerDamage.performed += ctx => player.TakeDamageTen();
    }

    private void Start()
    {
        buttonFunc = GameManager.Instance.buttonFuncScript;
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
