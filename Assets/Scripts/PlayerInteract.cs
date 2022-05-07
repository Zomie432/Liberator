using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Tooltip("Distance the player can interact with objects from")]
    [SerializeField] private float interactRange = 3f;

    public void ProcessInteraction()
    {
        //get where the player is looking from the game manager
        Vector3 forward = GameManager.Instance.playerAimVector;

        //send raycast and store whatever it collides with to check and see if it is something the player can 
        //interact with by comparing the gameObject's tag
        if (Physics.Raycast(transform.position, forward, out RaycastHit hit, interactRange))
        {
            //player interacts with a door, can do the same thing with hostage in the future
            if (hit.collider.CompareTag("Door"))
            {
                DoorController doorScript = hit.collider.gameObject.GetComponent<DoorController>();

                //interact method will decide whether that specific door needs to be opened or closed
                doorScript.Interact();
            }


        }

    }
}
