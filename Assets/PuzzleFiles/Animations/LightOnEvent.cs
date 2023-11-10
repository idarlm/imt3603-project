using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnEvent : AnimateOnEvent
{
    public Light light;

    protected override void Update() {
        base.Update();
    }

    protected override void Animate() {

        if (isActive) {
            light.enabled = true;
        } else {
            light.enabled = false;
        }
    }
}
