using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleTrigger : MonoBehaviour
{
    public EventHandler Triggered;
    public EventHandler TriggeredFinished;

    /*
     * Function for invoking event, used for when event should start
     */
    protected void FireTriggered(object sender, EventArgs args)
    {
        Triggered?.Invoke(sender, args);
    }


    /*
     * Function for invoking event, used for when event should end
     */
    protected void FireTriggeredFinished(object sender, EventArgs args) {
        TriggeredFinished?.Invoke(sender, args);
    }
}
