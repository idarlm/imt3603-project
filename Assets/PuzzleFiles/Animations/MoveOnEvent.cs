using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnEvent : AnimateOnEvent {
    [SerializeField] Vector3 offset;
    [SerializeField] int closingTime;

    protected override void Update() {
        base.Update();
    }

    protected override void Animate() {
        if (isActive) {
            transform.position = Vector3.Lerp(transform.position, startPos + offset, Time.deltaTime);
        } else {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime / closingTime);
        }
    }

}
