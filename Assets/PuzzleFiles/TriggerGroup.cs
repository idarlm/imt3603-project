using System;
using System.Collections.Generic;
using System.Linq;

public class TriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private List<bool> activated = new List<bool>();
    private bool allActivated = false;

    private void Start() {

        // Going through all triggers and subscribing to an event
        foreach (var trigger in triggers) {
            trigger.Triggered += OnTriggered;
            activated.Add(false); // Filling the activated list with false for each trigger
        }
    }

    private void OnTriggered(object sender, EventArgs args) {

        // Going through the triggers 
        for(var i=0; i < triggers.Length; i++) {

            // Finding the triggered trigger
            if (triggers[i] == sender) {
               
                // Updating the activated bool for that trigger
                if (!activated[i]) {
                    activated[i] = true;
                } else {
                    activated[i] = false;
                }
                
            }
        }

        // Checking if all the bools in the activated list is set to true
        if (activated.All(t => t)) {
            allActivated = true;
        }

        // Firing an event if all are activated
        if (allActivated) {
            FireTriggered(this, EventArgs.Empty);
        }
    }
    
    
}
