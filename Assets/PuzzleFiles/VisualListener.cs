using System;
using UnityEngine;

public class VisualListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;
    private int i = 0;
    [SerializeField] GameObject correctKey;


    private void Start() {

        // Subscribing to event
        trigger.Triggered += ChangeVisual;

        // Disabling all children except the first
        for(i = 1; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    void ChangeVisual(object obj, EventArgs args) {

        // Going through all children
        for (i = 0; i < transform.childCount; i++) {

            // Getting the state of the child if it is active or not
            var enabledState = transform.GetChild(i).gameObject.activeSelf;

            if (enabledState) { // Checking if its active

                transform.GetChild(i).gameObject.SetActive(false);
                if (i != transform.childCount - 1) { // Checking that we are not at the last child
                    i++;
                    transform.GetChild(i).gameObject.SetActive(true);
                } else {
                    i = 0; // Resetting the index
                    transform.GetChild(i).gameObject.SetActive(true);
                }


                // Checking if the child name matches the correct symbols name
                if (transform.GetChild(i).name == correctKey.name) {
                    FireTriggered(this, EventArgs.Empty);
                } else {
                    FireTriggeredFinished(this, EventArgs.Empty);
                }

            }

        }

    }
}
