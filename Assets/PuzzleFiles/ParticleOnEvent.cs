using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnEvent : PuzzleTrigger {

    public ParticleSystem ps;
    public PuzzleTrigger trigger;

    /*
     * Function to disable and enable particle emission
     */
    private void Start() {
        trigger.Triggered += ChangeParticle;
    }

    void ChangeParticle(object obj, EventArgs args) {

        ParticleSystem.EmissionModule em = ps.emission;

        if (!em.enabled) {  //check if emission is disabled
            em.enabled = true;  //enable emission
        } else {
            em.enabled = false; //disable emission
        }

    }
}
