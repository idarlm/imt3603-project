using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateOnEvent : AnimateOnEvent {
    [SerializeField] float animationTime;
    [SerializeField] int closingTime;
    [SerializeField] Vector3 axis;
    [SerializeField] float angle;
    quaternion startAngle;
    quaternion endAngle;
    

    protected override void Start() {
        base.Start();
        startAngle = transform.rotation;
        endAngle = transform.rotation * Quaternion.AngleAxis(angle, axis);
    }

    protected override void Update() {
        base.Update();
    }

    protected override void Animate() {
        if (isActive) {
            transform.rotation = Quaternion.Lerp(transform.rotation, endAngle, Time.deltaTime / animationTime);
        } else {
            transform.rotation = Quaternion.Lerp(transform.rotation, startAngle, Time.deltaTime / closingTime);
        }
    }
}
