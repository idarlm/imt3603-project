using System;
using UnityEngine;

public class SimpleTrigger : PuzzleTrigger
{

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other) {

        // On entering a box collider, check if collider is player
        if (other.gameObject.name == "Player") 
        {
            if (!isTriggered)   //if box collider is not already triggered, fire an event and set isTriggered to true
            {
                FireTriggered(this, EventArgs.Empty);
                isTriggered = true;
            } 
            
        } 
    }
   
}
