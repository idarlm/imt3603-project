using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;
    private int i = 0;
    [SerializeField] GameObject correctkey;

    private void Start() {
        trigger.Triggered += changeVisual;

        for(i = 1; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }

    }

    void changeVisual(object obj, EventArgs args) {

        for (i = 0; i < transform.childCount; i++) {

            var enabledState = transform.GetChild(i).gameObject.activeSelf;

            if (enabledState) {

                transform.GetChild(i).gameObject.SetActive(false);
                if (i != transform.childCount - 1) {
                    i++;
                    transform.GetChild(i).gameObject.SetActive(true);
                } else {
                    i = 0;
                    transform.GetChild(i).gameObject.SetActive(true);
                }


                if (transform.GetChild(i).name == correctkey.name) {
                    FireTriggered(this, EventArgs.Empty);
                    Debug.Log("VisualListener fired");
                } else {
                    FireTriggeredFinished(this, EventArgs.Empty);
                    Debug.Log("VisualListener finished");
                }

            }

        }

    }
}
