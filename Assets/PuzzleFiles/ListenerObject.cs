using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerObject : MonoBehaviour
{
    public PuzzleTrigger trigger;
    [SerializeField] Vector3 offset;
    private bool isOpening = false;
    private Vector3 startPos;
    [SerializeField] int closingTime;

    private void Start()
    {
        startPos = transform.position;
        trigger.Triggered += TriggerActivated;
        trigger.TriggeredFinished += TriggerDeactivated;
    }

    private void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, startPos + offset, Time.deltaTime);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime/closingTime);
        }
    }

    void TriggerActivated(object obj, EventArgs args)
    {

        isOpening = true;
    }

    void TriggerDeactivated(object obj, EventArgs args)
    {

        isOpening = false;
    }
}
