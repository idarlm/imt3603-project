using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;
    private bool isActive;

    private void Start() {
        trigger.Triggered += pickupObject;
        isActive = transform.gameObject.activeSelf;
    }


    void pickupObject(object obj, EventArgs args) {

        Debug.Log(gameObject.name);

        transform.gameObject.SetActive(false);

        FireTriggered(this, EventArgs.Empty);
        Debug.Log("PickupListener fired");

    }
}
