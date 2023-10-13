using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private int index = 0;

    private void Start()
    {
        triggers[index].Triggered += OnTriggered;
    }

    private void OnTriggered(object sender, EventArgs args)
    {
        triggers[index].Triggered -= OnTriggered;
        index++;

        if (index < triggers.Length)
        {
            triggers[index].Triggered += OnTriggered;

        } else
        {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("TriggerGroup fired");
        }
    }
}
