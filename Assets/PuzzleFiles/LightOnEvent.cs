using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnEvent : PuzzleTrigger
{
    public Light light;
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += changeLight;
    }


    void changeLight(object obj, EventArgs args) {

        if (!light.enabled) {
            light.enabled = true;
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("changeLight fired");
        } else { 
            light.enabled = false;
            FireTriggeredFinished(this, EventArgs.Empty);
            Debug.Log("changeLight fired");
        }

    }

}
