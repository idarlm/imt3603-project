using System;
using UnityEngine;

public class KeypadListener : PuzzleTrigger
{

    public PuzzleTrigger[] triggers; // List of all the triggers
    [SerializeField] GameObject[] correctKeys; // List of the correct combination for the puzzle
    private bool key1 = false, key2 = false, key3 = false; // Bool for each symbol of the pinpuzzle
    private Transform key1Object, key2Object, key3Object; // The object for each symbol

    private void Start() {

        // Going through the list of triggers and subscribing to events
        foreach(var trigger in triggers) {
            trigger.Triggered += OnTriggered;
            trigger.TriggeredFinished += OnTriggeredFinished;
        }

    }

    private void OnTriggered(object sender, EventArgs args) {

        // Looping through the different symbols to compare it to the right combination
        for (var i = 0; i < 4; i++) {
            key1Object = triggers[0].transform.GetChild(i);

            // Checking if the object has the same symbol as the symbol inthe same place in the correct combination
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

        // Checking if all objects match the correct combination
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

