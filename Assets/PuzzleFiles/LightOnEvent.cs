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
        } else { 
            light.enabled = false;  //disable it
        }

    }

}
