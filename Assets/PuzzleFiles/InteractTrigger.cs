using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractTrigger : PuzzleTrigger {

    private bool inRange = false;
    private bool interacted = false;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && inRange && !interacted) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("InteractTrigger fired");
            interacted = true;

        } else if (Input.GetKeyDown(KeyCode.E) && inRange && interacted) {
            FireTriggeredFinished(this, EventArgs.Empty);
            Debug.Log("InteractTrigger finished");
            interacted = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") { 
            inRange = true;
            Debug.Log("In range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            inRange = false;
        }
    }

}
