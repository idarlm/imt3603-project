using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += PickupObject;
    }


    void PickupObject(object obj, EventArgs args) {

        Debug.Log(gameObject.name);

        transform.gameObject.SetActive(false);

        FireTriggered(this, EventArgs.Empty);
        Debug.Log("PickupListener fired");

    }
}
