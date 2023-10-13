using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObjectListener : PuzzleTrigger
{
    public PuzzleTrigger trigger;
    private Vector3 startPos;
    private bool isMoved = false;
    private bool isFetched = false;
    [SerializeField] Vector3 offset;

    private void Start()
    {
        startPos = transform.position;
        trigger.Triggered += moveObject;
    }

    private void Update()
    {
        if (isMoved)
        {
            transform.position = Vector3.Lerp(transform.position, startPos + offset, Time.deltaTime);
        }

        if (isFetched)
        {
            transform.parent.GetChild(1).gameObject.SetActive(false);
            FireTriggered(this, EventArgs.Empty);
        }


    }

    void moveObject(object obj, EventArgs args)
    {

        Debug.Log(trigger.name);

        if (!isMoved)
        {
            isMoved = true;
        }
        else
        {
            isFetched = true;
        }

    }

}
