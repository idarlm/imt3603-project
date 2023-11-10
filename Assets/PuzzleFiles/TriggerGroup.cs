using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class TriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private int index = 0;
    private List<bool> activated = new List<bool>();
    private bool allActivated = false;

    private void Start() {

        foreach (var trigger in triggers) {
            trigger.Triggered += OnTriggered;
            trigger.TriggeredFinished = OnTriggeredFinished;
            activated.Add(false);
        }
    }

    private void OnTriggered(object sender, EventArgs args) {

        for(var i=0; i < triggers.Length; i++) {
            if (triggers[i] == sender) {
                Debug.Log("index = " + i);
                activated[i] = true;
            }

            allActivated = activated[i] &= true;
        }

        if (allActivated) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("TriggerGroup fired");
            index = 0;
        }
    }

    
    private void OnTriggeredFinished(object sender, EventArgs args) {

        Debug.Log("Trigger finished in TriggerGroup");

        for (var i = 0; i < triggers.Length; i++) {
            if (triggers[i] == sender) {
                activated[i] = false;
            }

            allActivated = activated[i] |= false;
        }

        
    }
    
    
}
