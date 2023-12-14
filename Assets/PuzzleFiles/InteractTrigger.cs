using System;
using UnityEngine;

public class InteractTrigger : PuzzleTrigger {

    private bool inRange = false;   //indicate if player is ini range or not

    private void Update() {
        //continuously check if key E is pressed and if player is in range
        if (Input.GetKeyDown(KeyCode.E) && inRange) {
            FireTriggered(this, EventArgs.Empty);
        }
    }

    // When entering the box collider, check if the collider is the player, and set inRange to true
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") { 
            inRange = true;
            InteractionTextUI.ShowUI(); //text showing indicating interaction option
        }
    }

    // When exiting the box collider, check if the collider is the player, and set inRange to false
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player") {
            inRange = false;
            InteractionTextUI.CloseUI();
        }
    }

}
