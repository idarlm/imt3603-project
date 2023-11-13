using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class KeypadListener : PuzzleTrigger
{

    public PuzzleTrigger[] triggers;
    [SerializeField] GameObject[] correctKeys;
    private bool key1, key2, key3 = false;
    private Transform key1Object, key2Object, key3Object;

    private void Start() {
        foreach(var trigger in triggers) {
            trigger.Triggered += OnTriggered;
            trigger.TriggeredFinished += OnTriggeredFinished;
        }

    }

    private void OnTriggered(object sender, EventArgs args) {


        for (var i = 0; i < 4; i++) {
            key1Object = triggers[0].transform.GetChild(i);
            if (key1Object.gameObject.activeSelf && key1Object.name == correctKeys[0].name) {
                key1 = true;
            }

            key2Object = triggers[1].transform.GetChild(i);
            if (key2Object.gameObject.activeSelf && key2Object.name == correctKeys[1].name) {
                key2 = true;
            }

            key3Object = triggers[2].transform.GetChild(i);
            if (key3Object.gameObject.activeSelf && key3Object.name == correctKeys[2].name) {
                key3 = true;
            }

        }

        if (key1 && key2 && key3) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("KeypadListener fired");
        }

    }

    private void OnTriggeredFinished(object sender, EventArgs args) {

        for (var i = 0; i < 4; i++) {
            key1Object = triggers[0].transform.GetChild(i);
            if (key1Object.gameObject.activeSelf && key1Object.name != correctKeys[0].name && key1 == true) {
                key1 = false;
            }

            key2Object = triggers[1].transform.GetChild(i);
            if (key2Object.gameObject.activeSelf && key2Object.name != correctKeys[1].name && key2 == true) {
                key2 = false;
            }

            key3Object = triggers[2].transform.GetChild(i);
            if (key3Object.gameObject.activeSelf && key3Object.name != correctKeys[2].name && key3 == true) {
                key3 = false;
            }

        }

    }
}

