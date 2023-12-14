using System;

public class PickupListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += PickupObject;
    }


    /*
     *  Function for disabling object after interacting with it to simulate picking it up
     */
    void PickupObject(object obj, EventArgs args) {

        transform.gameObject.SetActive(false);

        FireTriggered(this, EventArgs.Empty);

    }
}
