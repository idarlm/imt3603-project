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
            activated.Add(false);
        }
    }

    private void OnTriggered(object sender, EventArgs args) {

        for(var i=0; i < triggers.Length; i++) {
            if (triggers[i] == sender) {
                Debug.Log("index = " + i);
                if (!activated[i]) {
                    activated[i] = true;
                } else {
                    activated[i] = false;
                }
                
            }
        }

        if (activated.All(t => t)) {
            allActivated = true;
        }

        if (allActivated) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("TriggerGroup fired");
        }
    }
    
    
}
