using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnEvent : AnimateOnEvent {

    public ParticleSystem ps;


    protected override void Update() {
        base.Update();
    }

    protected override void Animate() {

        ParticleSystem.EmissionModule em = ps.emission;

        if (isActive) {
            em.enabled = false;
        } else {
            em.enabled = true;
        }
    }
}
