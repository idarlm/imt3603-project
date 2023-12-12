using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InteractTrigger : PuzzleTrigger {

    private bool inRange = false;
    [SerializeField] InteractionTextUI canvasUI;

    private void Update() {

        if(inRange)
        {
            canvasUI.ShowUI();
            Debug.Log("In range");
        } else
        {
            canvasUI.CloseUI();
        }

        if (Input.GetKeyDown(KeyCode.E) && inRange) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("InteractTrigger fired");
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "Player") { 
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player") {
            inRange = false;
        }
    }

}
