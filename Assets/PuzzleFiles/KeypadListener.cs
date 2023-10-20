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
        }
    }


    private void Update() {

        for (var i = 0; i < 4; i++) {
            var key1Object = triggers[0].transform.GetChild(0).GetChild(i);
            Debug.Log(key1Object.name);
            Debug.Log(key1Object.GetComponent<Renderer>().enabled);
            if (key1Object.GetComponent<Renderer>().enabled && key1Object.name == correctKeys[0].name) {
                key1 = true;
            }

            var key2Object = triggers[1].transform.GetChild(0).GetChild(i);
            if (key2Object.GetComponent<Renderer>().enabled && key2Object.name == correctKeys[1].name) {
                key2 = true;
            }

            var key3Object = triggers[2].transform.GetChild(0).GetChild(i);
            if (key3Object.GetComponent<Renderer>().enabled && key3Object.name == correctKeys[2].name) {
                key3 = true;
            }
        }

        Debug.Log(key1);
        Debug.Log(key2);
        Debug.Log(key3);


        //key1 = triggers[0].transform.GetChild(0).Find(correctKeys[0].name).GetComponent<Renderer>().enabled;
        //key2 = triggers[1].transform.GetChild(0).Find(correctKeys[1].name).GetComponent<Renderer>().enabled;
        //key3 = triggers[2].transform.GetChild(0).Find(correctKeys[2].name).GetComponent<Renderer>().enabled;

        if (key1 && key2 && key3) {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("KeypadListener fired");
        }
    }


    private void OnTriggered(object sender, EventArgs args) {

        Debug.Log("Hei");
        
       
        
    }
}

