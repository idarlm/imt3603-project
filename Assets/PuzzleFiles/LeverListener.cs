using System;
using UnityEngine;

public class LeverListener : MonoBehaviour
{
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += ActivateLever;  
    }


    /*
     *  Function to activate the object
     */
    void ActivateLever(object obj, EventArgs args) {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
