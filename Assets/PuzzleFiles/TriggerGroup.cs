using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private readonly Dictionary<PuzzleTrigger, bool> _isTriggered = new();

    private void Start()
    {
        foreach (var trigger in triggers)
        {
            trigger.Triggered += OnTriggered;
            _isTriggered.Add(trigger, false);
        }
    }

    private void OnTriggered(object sender, EventArgs args)
    {
        //Debug.Log(_isTriggered.ContainsKey(sender));
        _isTriggered[sender as PuzzleTrigger] = true;

        var allTriggered = true;

        foreach (var pair in _isTriggered) {
            allTriggered &= pair.Value;
        }

        if(allTriggered)
        {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("TriggerGroup fired");
        }
    }
}
