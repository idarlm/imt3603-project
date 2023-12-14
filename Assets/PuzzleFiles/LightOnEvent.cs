using System;
using UnityEngine;

public class LightOnEvent : PuzzleTrigger {
    public Light light;
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += ChangeLight;
    }

    /*
     * Function to disable and enable light source
     */
    void ChangeLight(object obj, EventArgs args) {

        if (!light.enabled) {   //if light is disabled
            light.enabled = true;   //enable it
            FireTriggered(this, EventArgs.Empty);   //fire event so that door in puzzle can listen if it's done
        } else { 
            light.enabled = false;  //disable it
            FireTriggeredFinished(this, EventArgs.Empty);   //finish fireing event so that door in puzzle know it's not done
        }

    }

}
