using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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

        activated[index] = true;
        index++;

        for(var i=0; i < triggers.Length; i++) {
            allActivated = activated[i] &= true;
        }

        if (allActivated) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("TriggerGroup fired");
        }
    }
}
