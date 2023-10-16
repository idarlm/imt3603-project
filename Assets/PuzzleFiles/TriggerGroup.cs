using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private int index = 0;

    private void Start() {
        triggers[index].Triggered += OnTriggered;
    }

    private void OnTriggered(object sender, EventArgs args) {

        FireTriggered(this, EventArgs.Empty);
        Debug.Log("TriggerGroup unordered fired");

    }
}
