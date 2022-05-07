using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;

    private bool doorOpen = false;
    private bool isPlaying;

    private void Start()
    {
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    public void Interact()
    {
        isPlaying = (doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen") || doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("DoorClose")) && doorAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;
        
        //check to see if an animation is already playing, if so we don't want to interact
        if(!isPlaying)
        {
            //neither animation is currently playing so...
            if(doorOpen)
            {
                doorAnimator.Play("DoorClose", 0, 0.0f);
                doorOpen = false;
            }
            else
            {
                doorAnimator.Play("DoorOpen", 0, 0.0f);
                doorOpen = true;
            }
        }
    }
}
