using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleTrigger : MonoBehaviour
{
    public EventHandler Triggered;

    protected void FireTriggered(object sender, EventArgs args)
    {
        Triggered?.Invoke(sender, args);
    }
}
