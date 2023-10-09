using System;
using UnityEngine;

public class SimpleTrigger : PuzzleTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            FireTriggered(this, EventArgs.Empty);
            Debug.Log("SimpleTrigger fired");
        }
    }
}
