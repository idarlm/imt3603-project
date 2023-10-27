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

    private void Start() {
        foreach(var trigger in triggers) {
            trigger.Triggered += OnTriggered;
            trigger.TriggeredFinished += OnTriggeredFinished;
        }
    }

    private void OnTriggered(object sender, EventArgs args) {

        Debug.Log("Hei");


        for (var i = 0; i < 4; i++) {
            var key1Object = triggers[0].transform.GetChild(i);
            Debug.Log(key1Object.name);
            Debug.Log(key1Object.GetComponent<Renderer>().enabled);
            if (key1Object.GetComponent<Renderer>().enabled && key1Object.name == correctKeys[0].name) {
                key1 = true;
            }

            var key2Object = triggers[1].transform.GetChild(i);
            if (key2Object.GetComponent<Renderer>().enabled && key2Object.name == correctKeys[1].name) {
                key2 = true;
            }

            var key3Object = triggers[2].transform.GetChild(i);
            if (key3Object.GetComponent<Renderer>().enabled && key3Object.name == correctKeys[2].name) {
                key3 = true;
            }

        }

        Debug.Log(key1);
        Debug.Log(key2);
        Debug.Log(key3);


        if (key1 && key2 && key3) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("KeypadListener fired");
        }

    }

    private void OnTriggeredFinished(object sender, EventArgs args) {

        for (var i = 0; i < 4; i++) {
            var key1Object = triggers[0].transform.GetChild(i);
            Debug.Log(key1Object.name);
            Debug.Log(key1Object.GetComponent<Renderer>().enabled);
            if (key1Object.GetComponent<Renderer>().enabled && key1Object.name != correctKeys[0].name && key1 == true) {
                key1 = false;
            }

            var key2Object = triggers[1].transform.GetChild(i);
            if (key2Object.GetComponent<Renderer>().enabled && key2Object.name != correctKeys[1].name && key2 == true) {
                key2 = false;
            }

            var key3Object = triggers[2].transform.GetChild(i);
            if (key3Object.GetComponent<Renderer>().enabled && key3Object.name != correctKeys[2].name && key3 == true) {
                key3 = false;
            }

        }

        Debug.Log(key1);
        Debug.Log(key2);
        Debug.Log(key3);

    }
}

