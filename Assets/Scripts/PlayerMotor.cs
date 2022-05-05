using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
//this script will contain all player movement functionality
public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity; //used for vertical movement and gravity only
    Vector3 moveDirection = Vector3.zero; //used for horizontal movement
    private bool isGrounded; //whether or not the player is on the ground, used for gravity
    private bool lerpCrouch = false;
    private bool crouching = false;
    private bool slowWalking = false;
    private bool waitingToLandAndCrouch = false;
    private bool waitingToLandAndShiftWalk = false;
    [Header("Player Speed in Worldspace(for animation states)")]
    [SerializeField] private bool track3dSpeed = false;
    [SerializeField] private bool track2dSpeed = true;

    [Header("Speed Variables (do not modify active speed 2d or 3d)")]
    [SerializeField] private float currentActiveSpeed3D = 0f;
    [SerializeField] public float currentActiveSpeed2D = 0f;

    //property to access for animation states
    public float CurrentActiveSpeed2D { get { return currentActiveSpeed2D; } }

    [Tooltip("How often the speed variable(2D/3D) will be updated(in seconds)")]
    [SerializeField] private float speedCheckTimerMax = 0.1f;
    private float speedCheckTimer = 0.1f;

    //current player position used for 2D and 3D speed calculations
    [Tooltip("Used to calculate speed by calculating how the player's transform.position changes over time")]
    [SerializeField] private Transform playerTransform;

    //positions used for 2D speed calculation
    private Vector2 currentPlayerPosition2d;
    private Vector2 lastPlayerPostion2d;

    //previous position used for 3D speed calculation
    private Vector3 lastPlayerPostion3d;

    [Header("Player Movement Regarding WASD Input")]
    [Tooltip("This value will adjust the max speed of the player, but also note that the time it takes to accelerate to that max speed will be the same as before(so you may need to bump up or down the rate of acceleration")]
    [SerializeField] private float maxSpeed = 5f;
    private float currentMaxSpeed; //used for crouch and walk so we can always go back to the original speed^^^
    [Tooltip("This value will be multiplied by the player's max speed while they hold the slow-walk key, so 0.5 means the player moves at half of their max speed when shift walking")]
    [SerializeField] private float shiftWalkSpeedMultiplier = 0.5f;
    [Tooltip("This value will be multiplied by the player's max speed when they press crouch, so 0.5 means the player moves at half of their max speed when crouching")]
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [Tooltip("How long it take the player to reach their maximum speed(smoothly interpolating between user WASD input).")]
    [SerializeField] private float timeToAccelerate = 0.2f;
    [Tooltip("This is the same as time to accelerate, but separate so that the player's input takes longer to affect move direction whle they are in the air.")]
    [SerializeField] private float airTimeToAccelerate = 0.7f;

    [Header("Player Vertical Movement(not world gravity, safe to tweak)")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1.2f;
    [Tooltip("How long it takes the player to crouch and uncrouch")]
    [SerializeField] private float crouchTimer = 1f;
    
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;

    // Start is called before the first frame update
    void Start()
    {
        currentMaxSpeed = maxSpeed;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (track3dSpeed)
        //{
        //    if (speedCheckTimer > 0 && lastPlayerPostion3d != null)
        //    {
        //        //decrement timer until we check the player's speed
        //        speedCheckTimer -= Time.deltaTime;
        //    }
        //    else if (lastPlayerPostion3d != null)
        //    {
        //        //reset timer
        //        speedCheckTimer = speedCheckTimerMax;

        //        //calculate distance between the previous and current transform
        //        currentActiveSpeed3D = Vector3.Distance(transform.position, lastPlayerPostion3d);

        //        //set the current transfrom as the new previous transform for the next iteration
        //        lastPlayerPostion3d = transform.position;
        //    }
        //    else
        //        lastPlayerPostion3d = transform.position;
        //}
        if (track2dSpeed)
        {
            //convert player's 3d position into a 2d vector(doesn't take the player's vertical velocity into account for calculation)
            currentPlayerPosition2d.x = playerTransform.position.x;
            currentPlayerPosition2d.y = playerTransform.position.z;

            if (speedCheckTimer > 0 && lastPlayerPostion2d != null)
            {
                //decrement timer until we check the player's speed
                speedCheckTimer -= Time.deltaTime;
            }
            else if (lastPlayerPostion2d != null)
            {
                //reset timer
                speedCheckTimer = 0.1f;

                //calculate distance between the previous and current transform
                currentActiveSpeed2D = Vector2.Distance(currentPlayerPosition2d, lastPlayerPostion2d);

                //set the current transfrom as the new previous transform for the next iteration
                lastPlayerPostion2d = currentPlayerPosition2d;
            }
            else
            {
                lastPlayerPostion2d.x = playerTransform.position.x;
                lastPlayerPostion2d.y = playerTransform.position.z;
            }
        }


        //use the built in controller property to see if the player is on the ground to see if we need to apply gravity
        isGrounded = controller.isGrounded;

        #region Crouch and uncrouch smoothly using linear interpolation(lerp)
        //gradually crouch the player instead of snapping to the crouched position
        //lerp- linear interpolation, gradually moves between the values and applies them to the passed in variable(height)
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;

            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            //once the player is fully crouched, stop trying to transform the player into a crouched/standing position
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
        #endregion

        
    }

    //receives the inputs from our InputManager.cs and applies them to the character controller component
    //move inputs x and y  can each be either -1, 0, or 1 (ex. pressing 'A' sets input.x to -1, letting go of 'A' resets it to 0)
    public void ProcessMove(Vector2 input)
    {

        if (isGrounded)
        {
            //player started or stopped crouching whle in the air previously, so we need to update their speed accordingly
            if (waitingToLandAndCrouch)
                Crouch();

            //player started or stopped walking whle in the air previously, so we need to update their speed accordingly
            if (waitingToLandAndShiftWalk)
                SlowWalk();

            //smooth between WASD input for more fluid motion(psuedo acceleration/inertia)
            currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, timeToAccelerate);

            //change the 2D movement vector to the new smoothed inputs
            moveDirection.x = currentInputVector.x;
            moveDirection.z = currentInputVector.y;

            //use the CharacterController's built in Move function to move the player
            controller.Move(currentMaxSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
        }
        else
        {
            //if the player is pressed up against an obstacle then set the stored force that would otherwise be applied to 0
            //TO DO-------------------------------------------------------------
            //if (playersCurrentSpeedInXY < 0.05 && transform.position.y <= groundLeftYPos + 0.2f)
            //

            //smooth between WASD input to give player some air control, only difference is the "airTimeToAccelerate"
            currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, airTimeToAccelerate);

            //clamp the input vectors to make sure the player can't move faster than normal by jumping
            moveDirection.x = Mathf.Clamp(currentInputVector.x, -0.8f, 0.8f);
            moveDirection.z = Mathf.Clamp(currentInputVector.y, -0.8f, 0.8f);
            controller.Move(currentMaxSpeed * Time.deltaTime * transform.TransformDirection(moveDirection));
        }

        //apply a downward force on the player, force increases overtime
        playerVelocity.y += gravity * Time.deltaTime;

        //cap the downward force so that the player can jump, if they jump(!isGrounded) then the gravity will increase
        //the player's downward velocity as expected
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -4f;

        //apply the downward vector(which exclusively deals with Y axis movement(jumping and gravity)
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        //if they start crouching in the air, let the movement function know that it should run the given function(crouch)
        //whenever the player lands so that it can actually update the speed as opposed to doing it in the air or not at all
        //i.e. if the player starts crouching in the air, when they land slow down to the correct speed
        if (waitingToLandAndCrouch)
        {
            waitingToLandAndCrouch = false;
        }
        else
            crouching = !crouching;

        //let the player crouch in the air, but don't change the speed they travel at
        if (isGrounded)
        {
            if (crouching)
                currentMaxSpeed = maxSpeed * crouchSpeedMultiplier;
            else
                currentMaxSpeed = maxSpeed;
        }
        else
            waitingToLandAndCrouch = true;
            

        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void SlowWalk()
    {
        //if they start shifting in the air, let the movement function know that it should run the given function(slowwalk)
        //whenever the player lands so that it can actually update the speed as opposed to doing it in the air or not at all
        //i.e. if the player starts shifting in the air, when they land slow down to the correct speed
        if (waitingToLandAndShiftWalk)
        {
            waitingToLandAndShiftWalk = false;
            slowWalking = !slowWalking;
        }
        else
            slowWalking = !slowWalking;

        
        if (isGrounded)
        {
            if (!crouching && slowWalking)
                currentMaxSpeed = maxSpeed * shiftWalkSpeedMultiplier;
            else if (!crouching && !slowWalking)
                currentMaxSpeed = maxSpeed;
        }
        else //don't change the speed they travel at, queue up a function call for whenever the player lands
            waitingToLandAndShiftWalk = true;
    }
}
