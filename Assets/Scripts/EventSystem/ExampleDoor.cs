using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleBox : MonoBehaviour
{
    public PuzzleTrigger[] trigger;

    private void Start()
    {
        trigger[0].Triggered += OnTriggered;
    }

    void OnTriggered(object obj, EventArgs args)
    {
        transform.position += Vector3.up * 3;
    }
}
