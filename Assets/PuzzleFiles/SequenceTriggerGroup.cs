using System;

public class SequenceTriggerGroup : PuzzleTrigger
{
    public PuzzleTrigger[] triggers;
    private int index = 0;

    private void Start()
    {
        // Subscribing to the first trigger
        triggers[index].Triggered += OnTriggered;
    }

    private void OnTriggered(object sender, EventArgs args)
    {
        // Unsubscribing to the trigger
        triggers[index].Triggered -= OnTriggered;
        index++;

        // Checking if index is smaller than the length of triggers
        if (index < triggers.Length)
        {
            // Subscribing to the next trigger
            triggers[index].Triggered += OnTriggered;

        } else // Firing an event after all triggers have been triggered
        {
            FireTriggered(this, EventArgs.Empty);
        }
    }
}
