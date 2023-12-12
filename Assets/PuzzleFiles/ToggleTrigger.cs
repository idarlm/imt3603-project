using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ToggleTrigger : PuzzleTrigger {

    private bool inRange;
    private bool toggledOn;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.E) && inRange) {
            if (toggledOn)
            {
                FireTriggeredFinished(this, EventArgs.Empty);
            }
            else
            {
                FireTriggered(this, EventArgs.Empty);
            }
            toggledOn = !toggledOn;
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

    public void ToggleOff()
    {
        if (toggledOn)
        {
            FireTriggeredFinished(this, EventArgs.Empty);
            toggledOn = false;
        }
            
    }

    public void ToggleOn()
    {
        
        if (!toggledOn)
        {
            FireTriggered(this, EventArgs.Empty);
            toggledOn = true;
        }
    }
}