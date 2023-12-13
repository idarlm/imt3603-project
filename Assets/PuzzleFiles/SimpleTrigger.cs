using System;
using UnityEngine;

public class SimpleTrigger : PuzzleTrigger
{

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player") 
        {
            if (!isTriggered)
            {
                FireTriggered(this, EventArgs.Empty);
                Debug.Log("SimpleTrigger fired");
                isTriggered = true;
            } 
            
        } 
    }
    /*
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player") {

            if (isTriggered)
            {
                FireTriggeredFinished(this, EventArgs.Empty);
                Debug.Log("SimpleTrigger done firing");
                isTriggered = false;
            }
        }
    }
    */
}
