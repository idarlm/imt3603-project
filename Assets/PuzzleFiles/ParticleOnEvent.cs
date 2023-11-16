using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnEvent : PuzzleTrigger {

    public ParticleSystem ps;
    public PuzzleTrigger trigger;

    private void Start() {
        trigger.Triggered += ChangeParticle;
    }

    void ChangeParticle(object obj, EventArgs args) {

        ParticleSystem.EmissionModule em = ps.emission;

        if (!em.enabled) {
            em.enabled = true;
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("ChangeParticle fired");
        } else {
            em.enabled = false;
            FireTriggeredFinished(this, EventArgs.Empty);
            Debug.Log("ChangeParticle fired");
        }

    }
}
