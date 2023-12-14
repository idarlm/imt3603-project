using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class AnimateOnEvent : MonoBehaviour 
{
    public PuzzleTrigger trigger;
    protected bool isActive = false;    //variable used to determine behavior in animation scripts
    protected Vector3 startPos;

    protected virtual void Start() {
        startPos = transform.position;
        trigger.Triggered += TriggerActivated;
        trigger.TriggeredFinished += TriggerDeactivated;
    }

    protected virtual void Update() {
        Animate();  //continuously run 
    }

    void TriggerActivated(object obj, EventArgs args) {

        isActive = true;
    }

    void TriggerDeactivated(object obj, EventArgs args) {

        isActive = false;
    }

    abstract protected void Animate();
}



