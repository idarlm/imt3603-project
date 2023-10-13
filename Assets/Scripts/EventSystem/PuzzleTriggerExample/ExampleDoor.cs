using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBox : MonoBehaviour
{
    public PuzzleTrigger[] trigger;

    private void Start()
    {
        trigger[0].Triggered += OpenDoor;
        trigger[1].Triggered += CloseDoor;
    }

    void OpenDoor(object obj, EventArgs args)
    {
        transform.position += Vector3.up * 3;
    }

    void CloseDoor(object obj, EventArgs args)
    {
        transform.position += Vector3.down * 3;
    }
}
