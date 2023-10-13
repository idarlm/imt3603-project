using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleTrigger : MonoBehaviour
{
    public EventHandler Triggered;
    public EventHandler TriggeredFinished;

    protected void FireTriggered(object sender, EventArgs args)
    {
        Triggered?.Invoke(sender, args);
    }

    protected void FireTriggeredFinished(object sender, EventArgs args) {
        TriggeredFinished?.Invoke(sender, args);
    }
}
