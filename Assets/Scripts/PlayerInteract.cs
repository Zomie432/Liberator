using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Tooltip("Distance the player can interact with objects from")]
    [SerializeField] private float interactRange = 3f;
    private bool securingHostage = false;
    public void ProcessInteraction(bool pressOrHoldBehavior)
    {
        //get where the player is looking from the game manager
        Vector3 forward = GameManager.Instance.playerAimVector;

        //send raycast and store whatever it collides with to check and see if it is something the player can 
        //interact with by comparing the gameObject's tag
        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, interactRange))
        {
            if (pressOrHoldBehavior) //press interactions go here VVVVVVVVVVVVVVVVVV
            {
                //player interacts with a door
                if (hit.collider.CompareTag("Door"))
                {
                    DoorController doorScript = hit.collider.gameObject.GetComponent<DoorController>();

                    //interact method will decide whether that specific door needs to be opened or closed
                    doorScript.Interact();
                }
            }
            else //hold interactions go here VVVVVVVVVVVVVVVVVVVVVVVV
            {
                if (hit.collider.CompareTag("Hostage"))
                {
                    //if the player pressed E on the hostage, disable their movement until they hold for enough time
                    //to secure the hostage or if they "cancel" the action (done in methods below)
                    PlayerMotor.MovementEnabled = false;
                    securingHostage = true;
                }
            }
            
        }

    }

    public void CancelHostageSecure()
    {
        if (securingHostage)
        {
            //reenable player's movement
            PlayerMotor.MovementEnabled = true;

            //anything we want to do when the player cancels securing the hostage goes here

            //break player out of causing cancel/perform events when they aren't interacting with the hostage
            securingHostage = false;
        }
    }

    public void PerformHostageSecure()
    {
        //this runs if the player successfully completed the hold interaction (should win the floor)
        if (securingHostage)
        {
            //reenable player's movement
            PlayerMotor.MovementEnabled = true;

            //add code to win the level

            //break player out of causing cancel/perform events when they aren't interacting with the hostage
            securingHostage = false;
        }
    }
}
