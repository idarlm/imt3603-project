using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeverListener : MonoBehaviour
{
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += activateLever;

        Debug.Log(trigger.name);
        Debug.Log(transform.GetChild(0).gameObject.name);

        /*
        Debug.Log(GetComponent<Renderer>());

        GetComponent<Renderer>().enabled = false;
        */

        
        
    }


    void activateLever(object obj, EventArgs args) {

        Debug.Log("N� kj�rer vi Lever");

        //GetComponent<Renderer>().enabled = true;

        transform.GetChild(0).gameObject.SetActive(true);

    }
}
