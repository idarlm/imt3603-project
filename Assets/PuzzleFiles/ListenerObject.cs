using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerObject : MonoBehaviour
{
    public PuzzleTrigger trigger;

    private void Start()
    {
        trigger.Triggered += OpenDoor;
        trigger.TriggeredFinished += CloseDoor;
    }

    void OpenDoor(object obj, EventArgs args)
    {
        transform.position += Vector3.up * 3;
        //trigger.transform.position += Vector3.down * 0.2f;

    }

    void CloseDoor(object obj, EventArgs args)
    {
        transform.position += Vector3.down * 3;
    }
}
